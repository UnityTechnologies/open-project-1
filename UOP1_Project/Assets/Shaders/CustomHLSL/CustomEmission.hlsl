#ifndef CUSTOM_EMISSION_INCLUDED
#define CUSTOM_EMISSION_INCLUDED

/***********************************************************
 * Emission_float scales an input color, InColor, based on the emission variables:
 *	 bool   Emission  - Flag that tells shader to apply emission scaling
 *	 float  Intensity - The amount the scale InColor. Higher values = more emission
 *	 float3 Mask	  - Texture mask to control areas of emission on the model. 0. (Black) = No Emission, 1. (White) = full Emission
 *	 float3 InColor	  - Current value of the material before emission
 * And outputs the desired color, OutColor
 *   float3 OutColor  - Output color, masked to apply targeted emission, and scaled for intensity
 ***********************************************************/
void Emission_float(bool Emission, float Intensity, float3 Mask, float3 InColor, out float3 OutColor)
{
	float minEmit = 0.;
	float3 emit = Mask * Intensity;			// Scale Emission Mask value by the Intensity
	if(!Emission || (emit.r <= minEmit && emit.g <= minEmit && emit.b <= minEmit))
	{
		OutColor = InColor;				// Don't do emission
	}
	else
	{
		OutColor = 1.5 + emit * InColor;		// Do emission (Even black will emit at 1.5)
	}
}

#endif