// This code is an adaptation of the open-source work by Alexander Ameye
// From a tutorial originally posted here:
// https://alexanderameye.github.io/outlineshader
// Code also available on his Gist account
// https://gist.github.com/AlexanderAmeye

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Profiling;

public class OutlineTicknessFeature : ScriptableRendererFeature
{
    class OutlineTicknessPass : ScriptableRenderPass
    {
        int kDepthBufferBits = 32;
        private RenderTargetHandle outlineTicknessAttachmentHandle { get; set; }
        internal RenderTextureDescriptor descriptor { get; private set; }

        private Material outlineTicknessMaterial = null;
        private FilteringSettings m_FilteringSettings;
        string m_ProfilerTag = "OutlineTickness Prepass";
        ShaderTagId m_ShaderTagId = new ShaderTagId("SRPDefaultUnlit");

        public OutlineTicknessPass(RenderQueueRange renderQueueRange, LayerMask layerMask, Material material)
        {
            m_FilteringSettings = new FilteringSettings(renderQueueRange, layerMask);
			outlineTicknessMaterial = material;
        }

        public void Setup(RenderTextureDescriptor baseDescriptor, RenderTargetHandle outlineTicknessAttachmentHandle)
        {
            this.outlineTicknessAttachmentHandle = outlineTicknessAttachmentHandle;
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
            cmd.GetTemporaryRT(outlineTicknessAttachmentHandle.id, descriptor, FilterMode.Point);
            ConfigureTarget(outlineTicknessAttachmentHandle.Identifier());
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

				drawSettings.overrideMaterial = outlineTicknessMaterial;

				context.DrawRenderers(renderingData.cullResults, ref drawSettings,
                    ref m_FilteringSettings);

                cmd.SetGlobalTexture("_CameraOutlineTicknessTexture", outlineTicknessAttachmentHandle.id);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        /// Cleanup any allocated resources that were created during the execution of this render pass.
        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (outlineTicknessAttachmentHandle != RenderTargetHandle.CameraTarget)
            {
                cmd.ReleaseTemporaryRT(outlineTicknessAttachmentHandle.id);
				outlineTicknessAttachmentHandle = RenderTargetHandle.CameraTarget;
            }
        }
    }

	OutlineTicknessPass outlineTicknessPass;
    RenderTargetHandle outlineTicknessTexture;
	public Material outlineTicknessMaterial;
	public LayerMask outlineTicknessLayerMask;

	public override void Create()
    {
		outlineTicknessPass = new OutlineTicknessPass(RenderQueueRange.opaque, outlineTicknessLayerMask, outlineTicknessMaterial);
		outlineTicknessPass.renderPassEvent = RenderPassEvent.AfterRenderingPrePasses;
		outlineTicknessTexture.Init("_CameraOutlineTicknessTexture");
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
		outlineTicknessPass.Setup(renderingData.cameraData.cameraTargetDescriptor, outlineTicknessTexture);
        renderer.EnqueuePass(outlineTicknessPass);
    }
}

