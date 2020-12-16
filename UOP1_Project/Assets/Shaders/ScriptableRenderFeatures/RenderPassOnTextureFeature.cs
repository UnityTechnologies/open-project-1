using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Profiling;

public class RenderPassOnTextureFeature : ScriptableRendererFeature
{
    class RenderPassOnTexture : ScriptableRenderPass
    {
        int kDepthBufferBits = 32;
        private RenderTargetHandle renderPassTextureAttachmentHandle { get; set; }
        internal RenderTextureDescriptor descriptor { get; private set; }

        private Material m_renderPassMaterial = null;
        private FilteringSettings m_FilteringSettings;
        string m_ProfilerTag = "RenderPassTexture Prepass";
        ShaderTagId m_ShaderTagId = new ShaderTagId("SRPDefaultUnlit");
		string m_TextureName;

        public RenderPassOnTexture(RenderQueueRange renderQueueRange, LayerMask layerMask, Material material, string textureName)
        {
            m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
			m_renderPassMaterial = material;
			m_TextureName = textureName;
		}

        public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle renderPassTextureAttachmentHandle)
        {
            this.renderPassTextureAttachmentHandle = renderPassTextureAttachmentHandle;
            baseDescriptor.colorFormat = RenderTextureFormat.ARGB32;
            baseDescriptor.depthBufferBits = kDepthBufferBits;
            descriptor = baseDescriptor;
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in an performance manner.
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(renderPassTextureAttachmentHandle.id, descriptor, FilterMode.Point);
            ConfigureTarget(renderPassTextureAttachmentHandle.Identifier());
            ConfigureClear(ClearFlag.All, Color.black);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);
			
            using (new ProfilingScope(cmd, new ProfilingSampler(m_ProfilerTag)))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                var sortFlags = renderingData.cameraData.defaultOpaqueSortFlags;
                var drawSettings = CreateDrawingSettings(m_ShaderTagId, ref renderingData, sortFlags);
                drawSettings.perObjectData = PerObjectData.None;


                ref CameraData cameraData = ref renderingData.cameraData;
                Camera camera = cameraData.camera;
                if (cameraData.isStereoEnabled)
                    context.StartMultiEye(camera);
				if (m_renderPassMaterial != null)
				{
					drawSettings.overrideMaterial = m_renderPassMaterial;
				}
				context.DrawRenderers(renderingData.cullResults, ref drawSettings,
                    ref m_FilteringSettings);

                cmd.SetGlobalTexture(m_TextureName, renderPassTextureAttachmentHandle.id);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (renderPassTextureAttachmentHandle != RenderTargetHandle.CameraTarget)
            {
                cmd.ReleaseTemporaryRT(renderPassTextureAttachmentHandle.id);
				renderPassTextureAttachmentHandle = RenderTargetHandle.CameraTarget;
            }
        }
    }

	RenderPassOnTexture renderPass;
    RenderTargetHandle renderPassTexture;
	public string textureName;
	public Material renderPassMaterial;
	public LayerMask renderPassLayerMask;

	public override void Create()
    {
		renderPass = new RenderPassOnTexture(RenderQueueRange.opaque, renderPassLayerMask, renderPassMaterial, textureName);
		renderPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
		renderPassTexture.Init(textureName);
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
		renderPass.Setup(renderingData.cameraData.cameraTargetDescriptor, renderPassTexture);
        renderer.EnqueuePass(renderPass);
    }
}

