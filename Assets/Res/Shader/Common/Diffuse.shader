Shader "FGQJ/Common/Diffuse"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
		_LightIntensity("_LightIntensity",float) = 1
		_LightThreshold("_LightThreshold",Range(0,1)) = 0.0
		[Toggle(_AlphaCutOff)]_AlphaCutOff("_AlphaCutOff",float) = 0
		_Cutoff("Alpha Cutoff", Range( 0 , 1)) = 0.5
		[Toggle(GroundColor)]_TurnOnGroundColor("是否打开环境光贴图",float) = 1
		// _GroundTex ("环境光贴图", 2D) = "white" {}
		_GroundTexIntensity("环境光系数",float) = 2
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
		[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 1
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", Float) = 10
		// [Enum(UnityEngine.Rendering.BlendMode)] _SrcAlphaBlend("Src Alpha Blend Mode", Float) = 1
		// [Enum(UnityEngine.Rendering.BlendMode)] _DstAlphaBlend("Dst Alpha Blend Mode", Float) = 10
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry" "IgnoreProjector"="True" }
		LOD 100

		Pass
		{
			Name "SimpleLit"
			Tags { "LightMode" = "UniversalForward"}
			Cull[_Cull]
			ZWrite [_ZWrite]
			ZTest [_ZTest]
			Blend [_SrcBlend] [_DstBlend]

			Stencil
			{
				Ref 2//参考值为1
				Comp notequal//和模板缓冲区中的数据是否相等
				Pass keep// 测试通过后不改变模板缓冲区中的内容，我不是模板，只负责被遮罩
			}

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#pragma prefer_hlslcc gles
			#pragma	shader_feature _ GroundColor
			#pragma	multi_compile _ _AlphaCutOff
			#pragma exclude_renderers d3d11_9x
			#pragma target 2.0
			// #include "LitInput.hlsl"
			// #include "LitForwardPass.hlsl"
			#include "../Lib/SceneHeightFog.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "../Lib/TerrainAmbient.hlsl"
			// #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			// #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			// #include "Packages/com.unity.render-pipelines.universal/Shaders/BakedLitInput.hlsl"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float3 ambient : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			CBUFFER_START(UnityPerMaterial)
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			float4 _MainTex_ST;
			// #ifdef GroundColor //不统一的UnityPerMaterial尺寸会打断合批
			// TEXTURE2D(_GroundTex);
			// SAMPLER(sampler_GroundTex);
			// float4 _GroundTex_ST;
			float _GroundTexIntensity;
			// #endif
			float4 _Color;
			float _Cutoff;
			float _LightIntensity;
			float _LightThreshold;
			CBUFFER_END

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(float, _EffectAlpha)
			UNITY_INSTANCING_BUFFER_END(Props)
			v2f vert (appdata v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v,o);
				// float4 worldPos = mul(UNITY_MATRIX_M,v.vertex);
				// o.pos = mul(UNITY_MATRIX_VP,worldPos);
				o.pos = TransformObjectToHClip(v.vertex.xyz);
				o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
				float3 worldNormal = TransformObjectToWorldNormal(v.normal);
				o.worldNormal = normalize(worldNormal);
				o.color = v.color;
				// Light L  = GetMainLight();
				// half light = dot(worldNormal,L.direction);
				// light = max(_LightThreshold,light);
				o.ambient = SampleSH(worldNormal);
				// o.lightColor = lerp(ambient,L.color.rgb,light);
				// UNITY_TRANSFER_FOG(o,o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				// #ifdef GroundColor
				// 	float4 uv_ST = _GroundTex_ST*0.0001;//数值给的比较大方便美术调整
				// 	o.uv.zw = float2(o.worldPos.x,o.worldPos.z)*uv_ST.xy+uv_ST.zw;//跟草一样，世界位置坐标转为对环境光贴图的uv
				// #endif
				// o.uv.zw = TRANSFORM_TEX(float2(worldPos.x,-worldPos.z),_GroundTex);
				// UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			half3 AddLighting(Light light, half3 normalWS)
			{
				half nol = dot(normalWS, light.direction);
				half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
				half3 diffuse = attenuatedLightColor * saturate(nol);
				return diffuse;
			}

			half4 frag (v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy);
				#if _AlphaCutOff
					clip(col.a-_Cutoff);
				#endif
				col *= _Color*i.color;

				Light L = GetMainLight();
				half light = dot(i.worldNormal,L.direction);
				light = max(_LightThreshold,light);
				col.rgb *= lerp(i.ambient,L.color.rgb*_LightIntensity,light);

				#ifdef GroundColor
					col.rgb *= GetTerrainAmbientColor(i.worldPos)*_GroundTexIntensity;
				#endif
				// fixed3 lm=fixed3(0,0,0);
				// #ifdef LIGHTMAP_ON
				// 	lm = DecodeLightmap(UNITY_SAMPLE_TEX2D(unity_Lightmap, i.lightmapUV)).rgb;
				// 	lm += i.lightColor;
				// 	col.rgb *= lm;
				// #else

				// #ifdef _ADDITIONAL_LIGHTS
				half3 addColor = 0;
				uint pixelLightCount = GetAdditionalLightsCount();
				for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
				{
					Light light = GetAdditionalLight(lightIndex, i.worldPos);
					addColor += AddLighting(light, i.worldNormal);
				}
				col.rgb += col.rgb * addColor;
				// #endif

				// return half4(i.worldNormal,1);
				// col.rgb = i.lightColor*_LightIntensity;
				// #endif
				// UNITY_APPLY_FOG(i.fogCoord, col);
				SCENE_HIGHT_FOG(col,i.worldPos.y);

				float effectAlpha = UNITY_ACCESS_INSTANCED_PROP(Props, _EffectAlpha);
				col.a *= (1 - effectAlpha);

				return col;
			}
			ENDHLSL
		}
	}
}
