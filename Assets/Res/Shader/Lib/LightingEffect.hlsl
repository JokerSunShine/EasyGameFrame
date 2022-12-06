#ifndef UNIVERSAL_TEST_LIGHTING_INCLUDED
#define UNIVERSAL_TEST_LIGHTING_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


struct Light
{
	half3   direction;
	half3   color;
	half    distanceAttenuation;
};

float DistanceAttenuation(float distanceSqr, half2 distanceAttenuation)
{
	float lightAtten = rcp(distanceSqr);

	half smoothFactor = saturate(distanceSqr * distanceAttenuation.x + distanceAttenuation.y);

	return lightAtten * smoothFactor;
}

Light GetMainLight()
{
	Light light;
	light.direction = _MainLightPosition.xyz;
	light.distanceAttenuation = unity_LightData.z;
	light.color = _MainLightColor.rgb;

	return light;
}

Light GetAdditionalPerObjectLight(int perObjectLightIndex, float3 positionWS)
{
	float4 lightPositionWS = _AdditionalLightsPosition[perObjectLightIndex];
	half3 color = _AdditionalLightsColor[perObjectLightIndex].rgb;
	half4 distanceAndSpotAttenuation = _AdditionalLightsAttenuation[perObjectLightIndex];

	float3 lightVector = lightPositionWS.xyz - positionWS * lightPositionWS.w;
	float distanceSqr = max(dot(lightVector, lightVector), HALF_MIN);

	half3 lightDirection = half3(lightVector * rsqrt(distanceSqr));
	half attenuation = DistanceAttenuation(distanceSqr, distanceAndSpotAttenuation.xy);

	Light light;
	light.direction = lightDirection;
	light.distanceAttenuation = attenuation;
	light.color = color;

	return light;
}

int GetAdditionalLightsCount()
{
	return min(_AdditionalLightsCount.x, unity_LightData.y);
}

int GetPerObjectLightIndex(uint index)
{
	half2 lightIndex2 = (index < 2.0h) ? unity_LightIndices[0].xy : unity_LightIndices[0].zw;
	half i_rem = (index < 2.0h) ? index : index - 2.0h;
	return (i_rem < 1.0h) ? lightIndex2.x : lightIndex2.y;
}

Light GetAdditionalLight(uint i, float3 positionWS)
{
	int perObjectLightIndex = GetPerObjectLightIndex(i);
	return GetAdditionalPerObjectLight(perObjectLightIndex, positionWS);
}

half3 LightingLambert(half3 lightColor, half3 lightDir, half3 normal)
{
	half NdotL = saturate(dot(normal, lightDir));
	return lightColor * NdotL;
}

half4 PointLightColor(half3 worldPos)
{
	uint lightsCount = GetAdditionalLightsCount();
	half3 vertexLightColor = half3(0, 0, 0);
	for (uint lightIndex = 0u; lightIndex < lightsCount; ++lightIndex)
	{
		Light light = GetAdditionalLight(lightIndex, worldPos);
		half3 lightColor = light.color * light.distanceAttenuation;
		vertexLightColor += LightingLambert(lightColor, light.direction, half3(0,1,0));
	}
	return half4(vertexLightColor, 1);
}
#endif

