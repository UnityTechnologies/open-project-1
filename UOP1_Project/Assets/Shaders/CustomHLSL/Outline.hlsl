// This code is an adaptation of the open-source work by Alexander Ameye
// From a tutorial originally posted here:
// https://alexanderameye.github.io/outlineshader
// Code also available on his Gist account
// https://gist.github.com/AlexanderAmeye

TEXTURE2D(_CameraDepthTexture);
SAMPLER(sampler_CameraDepthTexture);
float4 _CameraDepthTexture_TexelSize;

TEXTURE2D(_CameraDepthNormalsTexture);
TEXTURE2D(_CameraDepthOutlineTexture);
 
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

float3 ViewDirectionFromScreenUV(float2 UV)
{
	float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
	return normalize(float3((UV * 2 - 1) / p11_22, 1));
}

float InnerOutlineObject(float2 UV, float OutlineThickness, float DepthSensitivity, float NormalsSensitivity, float DepthNormalSensitivity, float DepthNormalThresholdScale)
{
	float halfScaleFloor = floor(OutlineThickness * 0.5);
	float halfScaleCeil = ceil(OutlineThickness * 0.5);
	 
	float2 uvSamples[9];
	float depthMaskSample[9];
	float depthSamples[9];
	float3 normalSamples[9];

	uvSamples[1] = UV + float2(-_CameraDepthTexture_TexelSize.x * halfScaleFloor, _CameraDepthTexture_TexelSize.y * halfScaleCeil);
	uvSamples[2] = UV + float2(0, _CameraDepthTexture_TexelSize.y * halfScaleCeil);
	uvSamples[3] = UV + float2(_CameraDepthTexture_TexelSize.x * halfScaleCeil, _CameraDepthTexture_TexelSize.y * halfScaleCeil);

	uvSamples[4] = UV + float2(-_CameraDepthTexture_TexelSize.x * halfScaleFloor, 0);
	uvSamples[0] = UV;
	uvSamples[5] = UV + float2(_CameraDepthTexture_TexelSize.x * halfScaleCeil, 0);

	uvSamples[6] = UV + float2(-_CameraDepthTexture_TexelSize.x * halfScaleFloor, -_CameraDepthTexture_TexelSize.y  * halfScaleFloor);
	uvSamples[7] = UV + float2(0, -_CameraDepthTexture_TexelSize.y  * halfScaleFloor);
	uvSamples[8] = UV + float2(+_CameraDepthTexture_TexelSize.x * halfScaleCeil, -_CameraDepthTexture_TexelSize.y  * halfScaleFloor);

	for (int i = 0; i < 9; i++)
	{
		depthMaskSample[i] = SAMPLE_TEXTURE2D(_CameraDepthOutlineTexture, sampler_CameraDepthTexture, uvSamples[i]).r;
		depthSamples[i] = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, uvSamples[i]).r;
		normalSamples[i] = DecodeNormal(SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthTexture, uvSamples[i]));
	}

	bool allSamplesInAndSame = true;
	float edgeDepth = 0;

	for (int i = 0; i < 9; i++)
	{
		allSamplesInAndSame = allSamplesInAndSame && (depthMaskSample[0] == depthMaskSample[i]);
	}
	if (allSamplesInAndSame) {
		// Depth
		float depthFiniteDifference0 = (-depthSamples[1] - 2 * depthSamples[4] - depthSamples[6] + depthSamples[3] + 2 * depthSamples[5] + depthSamples[8]) / 4;
		float depthFiniteDifference1 = (-depthSamples[1] - 2 * depthSamples[2] - depthSamples[3] + depthSamples[6] + 2 * depthSamples[7] + depthSamples[8]) / 4;
		edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 100;

		// Thresholding with view direction. Balance normal difference based on the camera direction for flat surface viewed from a grazing angle
		float NdotV = 1 - dot(normalSamples[0]*2 - 1, -ViewDirectionFromScreenUV(UV));
		float normalThreshold01 = saturate((NdotV - DepthNormalSensitivity) / (1 - DepthNormalSensitivity));
		float normalThreshold = normalThreshold01 * DepthNormalThresholdScale + 1;
		float depthThreshold = (1 / DepthSensitivity) * depthSamples[0] * normalThreshold;

		edgeDepth = edgeDepth > depthThreshold ? 1 : 0;
	}

	// Normals
	float3 normalFiniteDifference0 = (-normalSamples[1] - 2 * normalSamples[4] - normalSamples[6] + normalSamples[3] + 2 * normalSamples[5] + normalSamples[8]) / 4;
	float3 normalFiniteDifference1 = (-normalSamples[1] - 2 * normalSamples[2] - normalSamples[3] + normalSamples[6] + 2 * normalSamples[7] + normalSamples[8]) / 4;
	float edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
	edgeNormal = edgeNormal > (1 / NormalsSensitivity) ? 1 : 0;

	float edge = max(edgeDepth, edgeNormal);
	return edge;
}

void OutlineObject_float(float2 UV, float OutlineThickness, float DepthSensitivity, float NormalsSensitivity, float DepthNormalSensitivity, float DepthNormalThresholdScale, out float Out)
{
	float halfScaleFloor = floor(OutlineThickness);
	float halfScaleCeil = ceil(OutlineThickness);
	
	float2 uvSamples[5];
	float depthMaskSample[5];
	bool allSamplesInAndSame = true;
	bool atLeastOneSampleIn = false;

	uvSamples[0] = UV;
	uvSamples[1] = UV - float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleFloor;
	uvSamples[2] = UV + float2(_CameraDepthTexture_TexelSize.x, _CameraDepthTexture_TexelSize.y) * halfScaleCeil;
	uvSamples[3] = UV + float2(_CameraDepthTexture_TexelSize.x * halfScaleCeil, -_CameraDepthTexture_TexelSize.y * halfScaleFloor);
	uvSamples[4] = UV + float2(-_CameraDepthTexture_TexelSize.x * halfScaleFloor, _CameraDepthTexture_TexelSize.y * halfScaleCeil);

	for (int i = 0; i < 5; i++)
	{
		depthMaskSample[i] = SAMPLE_TEXTURE2D(_CameraDepthOutlineTexture, sampler_CameraDepthTexture, uvSamples[i]).r;
	}

	float minValue = depthMaskSample[0]; // Used to determine if the object considered is in front of the other objects in case of overlapping.
	for (int i = 1; i < 5; i++)
	{
		allSamplesInAndSame = allSamplesInAndSame && (depthMaskSample[1] == depthMaskSample[i]);
		atLeastOneSampleIn = atLeastOneSampleIn || (depthMaskSample[i] > 0);
		if (depthMaskSample[i] < minValue)
		{
			minValue = depthMaskSample[i];
		}
	}

	if (!atLeastOneSampleIn) //The considered pixel is far from an outlined object so no need to draw outline.
	{
		Out = 0.0;
	}
	else if (!allSamplesInAndSame) // The considered pixel is on the border between outlined and not outlined object.
	{
		if (minValue < depthMaskSample[0]) // Draw outline only on the object the cloeset to the camera
		{
			Out = 1.0;
		}
		else
		{
			Out = 0.0;
		}
	}
	else // Inner outline and outline between overlapping outlined objects. Apply the depth/normal technique with half outline thickness to balance that all outline width will be drawn 
	{
		Out = InnerOutlineObject(UV, OutlineThickness, DepthSensitivity, NormalsSensitivity, DepthNormalSensitivity, DepthNormalThresholdScale);
	}
}

