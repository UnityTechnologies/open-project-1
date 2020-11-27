Shader "UOP1/DepthMask"
{
	SubShader{

		// Don't draw in the RGBA channels
		ColorMask 0
		// Don't write in the Depth Buffer
		ZWrite Off

		// Do nothing specific in the pass
		Pass {}
	}
}
