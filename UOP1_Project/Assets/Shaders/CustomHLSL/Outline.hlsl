// This code is an adaptation of the open-source work by Alexander Ameye
// From a tutorial originally posted here:
// https://alexanderameye.github.io/outlineshader
// Code also available on his Gist account
// https://gist.github.com/AlexanderAmeye

TEXTURE2D(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);
float4 _CameraDepthTexture_TexelSize;

TEXTURE2D(_CameraDepthNormalsTexture);
SAMPLER(sampler_CameraDepthNormalsTexture);

TEXTURE2D(_CameraOutlineThicknessTexture);
SAMPLER(sampler_CameraOutlineThicknessTexture);
 
float3 DecodeNormal(float4 enc)
{
    float kScale = 1.7777;
    float3 nn = enc.xyz*float3(2*kScale,2*kScale,0) + float3(-kScale,-kScale,1);
    float g = 2.0 / dot(nn.xyz,nn.xyz);
    float3 n;
    n.xy = g*nn.xy;
    n.z = g-1;
    return n;
}

// This method checks where the sampling pixels are regarding to the outlined object mask renderer pass
// If all the points are out of the mask (black pixels) then the funtion return 0 (Force no outline)
// If at least one pixel is black and another is white the function return (Force no outline)
// If all the pixels are white (inside an area covered by outlined objects). Return 0.5 half outline size.
float getTicknessBiasing(float2 UV, float OutlineThickness) {
	float halfScaleFloor = floor(OutlineThickness * 0.5);
	float halfScaleCeil = ceil(OutlineThickness * 0.5);

	float2 uvSamples[4];
	float allSamplesIn = 1.0;
	float atLeastOneSampleIn = 0.0;

	uvSamples[0] = UV - float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleFloor;
	uvSamples[1] = UV + float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleCeil;
	uvSamples[2] = UV + float2(_CameraDepthTexture_TexelSize.x * halfScaleCeil, -_CameraDepthTexture_TexelSize.y * halfScaleFloor);
	uvSamples[3] = UV + float2(-_CameraDepthTexture_TexelSize.x * halfScaleFloor, _CameraDepthTexture_TexelSize.y * halfScaleCeil);
	
	float maskSample;
	
	for (int i = 0; i < 4; i++)
	{
		maskSample = SAMPLE_TEXTURE2D(_CameraOutlineThicknessTexture, sampler_CameraOutlineThicknessTexture, uvSamples[i]).r;
		allSamplesIn = allSamplesIn * maskSample;
		atLeastOneSampleIn = atLeastOneSampleIn + maskSample;
	}
	return atLeastOneSampleIn == 0 ? 0.0 : (1.0 - 0.5 * allSamplesIn);
}

void OutlineObject_float(float2 UV, float OutlineThickness, float DepthSensitivity, float NormalsSensitivity, float DepthNormalSensitivity, float DepthNormalThresholdScale, float3 viewDir, out float Out)
{
	float biasedThickness = getTicknessBiasing(UV, OutlineThickness);
	if (biasedThickness == 0.0) //The considered pixel is far from an outlined object so no need to draw outline.
	{
		Out = 0.0;
	}
	else if (biasedThickness == 1.0) // The considered pixel is on the border between outlined and not outlined object. Always draw an outline.
	{
		Out = 1.0;
	}
	else // Inner outline and outline between overlapping outlined objects. Apply the depth/normal technique with half outline thickness to balance that all outline width will be drawn 
	{
		float halfScaleFloor = floor(biasedThickness * OutlineThickness * 0.5);
		float halfScaleCeil = ceil(biasedThickness * OutlineThickness * 0.5);

		float2 uvSamples[5];
		float depthSamples[5];
		float3 normalSamples[5];

		uvSamples[0] = UV;
		uvSamples[1] = UV - float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleFloor;
		uvSamples[2] = UV + float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleCeil;
		uvSamples[3] = UV + float2(_CameraDepthTexture_TexelSize.x * halfScaleCeil, -_CameraDepthTexture_TexelSize.y * halfScaleFloor);
		uvSamples[4] = UV + float2(-_CameraDepthTexture_TexelSize.x * halfScaleFloor, _CameraDepthTexture_TexelSize.y * halfScaleCeil);

		for (int i = 0; i < 5; i++)
		{
			depthSamples[i] = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uvSamples[i]).r;
			normalSamples[i] = DecodeNormal(SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, uvSamples[i]));
		}

		// Depth
		float depthFiniteDifference0 = depthSamples[2] - depthSamples[1];
		float depthFiniteDifference1 = depthSamples[4] - depthSamples[3];
		float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;

		// Thresholding with view direction. Balance normal difference based on the camera direction for flat surface viewed from a grazing angle
		float NdotV = 1 - dot(normalSamples[0], viewDir);
		float normalThreshold01 = saturate((NdotV - DepthNormalSensitivity) / (1 - DepthNormalSensitivity));
		float normalThreshold = normalThreshold01 * DepthNormalThresholdScale + 1;
		float depthThreshold = (biasedThickness / DepthSensitivity) * depthSamples[0] * normalThreshold;
		edgeDepth = edgeDepth > depthThreshold ? 1 : 0;

		// Normals
		float3 normalFiniteDifference0 = normalSamples[2] - normalSamples[1];
		float3 normalFiniteDifference1 = normalSamples[4] - normalSamples[3];
		float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
		edgeNormal = edgeNormal > (1 / NormalsSensitivity) ? 1 : 0;

		float edge = max(edgeDepth, edgeNormal);
		Out = edge;
	}
}

