#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#if defined(SHADERGRAPH_PREVIEW)
#else
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
#pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
#pragma multi_compile_fragment _ _SHADOWS_SOFT
#pragma multi_compile _ SHADOWS_SHADOWMASK
#pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
#pragma multi_compile _ LIGHTMAP_ON
#pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
#endif

void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float ShadowAtten)
{
#if defined(SHADERGRAPH_PREVIEW)
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
    ShadowAtten = 1;
#else
	float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);

    Light mainLight = GetMainLight(shadowCoord);
    Direction = mainLight.direction;
    Color = mainLight.color;

	#if !defined(_MAIN_LIGHT_SHADOWS) || defined(_RECEIVE_SHADOWS_OFF)
		ShadowAtten = 1.0h;
    #else
	    ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
	    float shadowStrength = GetMainLightShadowStrength();
	    ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
	    sampler_MainLightShadowmapTexture),
	    shadowSamplingData, shadowStrength, false);
    #endif
#endif
}


void DirectSpecular_float(float Smoothness, float3 Direction, float3 WorldNormal, float3 WorldView, out float3 Out)
{
    float4 White = 1;

#if defined(SHADERGRAPH_PREVIEW)
    Out = 0;
#else
    Smoothness = exp2(10 * Smoothness + 1);
    WorldNormal = normalize(WorldNormal);
    WorldView = SafeNormalize(WorldView);
    Out = LightingSpecular(White, Direction, WorldNormal, WorldView, White, Smoothness);
#endif
}

half4 GetShadowMask()
{
    // TODO: not sure if/how we can access inputData.shadowMask here.
    // Should only be relevant for lightmapped surfaces receiving additional light shadows
    // #if defined(SHADOWS_SHADOWMASK) && defined(LIGHTMAP_ON)
    // half4 shadowMask = inputData.shadowMask;
    // #elif !defined (LIGHTMAP_ON)
    #if !defined (LIGHTMAP_ON)
    half4 shadowMask = unity_ProbesOcclusion;
    #else
    half4 shadowMask = half4(1, 1, 1, 1);
    #endif
    return shadowMask;
}

void AdditionalLights_float(float Smoothness, float3 WorldPosition, float3 WorldNormal, float3 WorldView, out float3 Diffuse, out float3 Specular)
{
    float3 diffuseColor = 0;
    float3 specularColor = 0;
    float4 White = 1;

#if !defined(SHADERGRAPH_PREVIEW)
    half4 shadowMask = GetShadowMask();
    Smoothness = exp2(10 * Smoothness + 1);
    WorldNormal = normalize(WorldNormal);
    WorldView = SafeNormalize(WorldView);
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPosition, shadowMask);
        half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
        diffuseColor += LightingLambert(attenuatedLightColor, light.direction, WorldNormal);
        specularColor += LightingSpecular(attenuatedLightColor, light.direction, WorldNormal, WorldView, White, Smoothness);
    }
#endif

    Diffuse = diffuseColor;
    Specular = specularColor;
}

#endif