Shader "FGQJ/Terrain/TerrainInit"
{
	Properties
	{
		//_HeightMap("HeightMap",2D) = "white" {}
		_HollowMap("HollowMap",2D) = "black" {}
		_MainTex("Main Texture", 2DArray) = "white" {}
		_ControlTex ("_ControlTex", 2D) = "black" {}
		_WeightArr("Weight Texture", 2DArray) = "white" {}
		[Toggle(GroundColor)]_TurnOnGroundColor("是否打开环境光贴图",float) = 1
		//_AmbientTex("环境光贴图", 2D) = "white" {}
		_GroundTexIntensity("环境光系数",float) = 2
		//_TilingMap0("TilingMap0",Vector) = (1,1,1,1)
		//_TilingMap1("TilingMap1",Vector) = (1,1,1,1)
		//_TilingMap2("TilingMap2",Vector) = (1,1,1,1)
		//_TilingMap3("TilingMap3",Vector) = (1,1,1,1)
		_WaterIndex("WaterIndex",Float) = -1
		_WaterFlowCutOff("WaterFlowCutOff",Float) = 0.15
		[Toggle(WATERON)]_WaterOn("WaterExist",float) = 0
		[Toggle(HOLLOWON)]_HollowMapOn("HollowMapOn",float) = 0
		_HeightMapScale_MinY("HeightMapScale_MinY",Float) = 0
		_HeightMapScale("HeightMapScale",Float) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+100"  "IgnoreProjector" = "True" }
		LOD 500
		Pass
		{
			Tags { "LightMode" = "UniversalForward"}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			//alphatest greater 0.5
			Stencil
			{
				Ref 5
				Comp Always
				Pass Replace
			}
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma	shader_feature _ GroundColor
			#pragma shader_feature _ WATERON
			#pragma shader_feature _ HOLLOWON

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "../Lib/SceneHeightFog.hlsl"
			#include "../Lib/TerrainAmbient.hlsl"
			//#include "../Lib/LightingEffect.hlsl"

			CBUFFER_START(terrain)
			SAMPLER(Terrain_HeightMap);
			TEXTURE2D(_HollowMap);
			TEXTURE2D_ARRAY(_MainTex);   SAMPLER(sampler_MainTex);   float4 _MainTex_ST;
			TEXTURE2D(_ControlTex);    SAMPLER(sampler_ControlTex);   // float4 _ControlTex_ST;
			TEXTURE2D_ARRAY(_WeightArr);   SAMPLER(sampler_WeightArr); //  float4 _WeightArr_ST;
			//TEXTURE2D(_WeightTex1);
			//TEXTURE2D(Terrain_AmbientTex);

			//float4 _Offset;
			//float4 _TilingMap0;		   float4 _TilingMap1;			   float4 _TilingMap2;		float4 _TilingMap3;
			float _WaterIndex;
			float _WaterFlowCutOff;
			float _GroundTexIntensity;
			float4 _MainTex_STs[16];
			float _HeightMapScale_MinY;
			float _HeightMapScale;
			CBUFFER_END

		struct appdata
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float2 uv : TEXCOORD0;
			//float4 tangent : TANGENT;
			//float4 color : COLOR;
			//UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f
		{
			float4 pos : SV_POSITION;
			float4 uv : TEXCOORD0;
			float3 worldPos : TEXCOORD1;
			float3 worldNormal : TEXCOORD2;
			//UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		v2f vert(appdata v)
		{
			v2f o;
			half4 worldPos = mul(unity_ObjectToWorld, v.vertex);
			float4 heightMapColor = tex2Dlod(Terrain_HeightMap, float4(float2((2 + worldPos.x) * 0.00390625, worldPos.z * 0.00390625), 0, 0));
			v.vertex.y = _HeightMapScale_MinY+heightMapColor.r*_HeightMapScale;
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uv.xy = float2(worldPos.x * 0.00390625, worldPos.z * 0.00390625);
			//o.uv.zw = TRANSFORM_TEX(o.uv, _MainTex);
			float3 worldNormal = TransformObjectToWorldNormal(v.normal);
			o.worldNormal = normalize(worldNormal);
			//o.uv.zw = TRANSFORM_TEX(o.uv, _MainTex);
			o.worldPos = worldPos.xyz;
			return o;
		}

		half3 getColor(int index, float4 uv, float2 offset,float weight)
		{
			float4 st = _MainTex_STs[index];
			float2 uv_new = uv.xy * st.xy + st.zw;
			//float2 uv_new = uv.xy * 128;
#ifdef WATERON
			uv_new += step(abs(index - _WaterIndex), 0) * offset * step(_WaterFlowCutOff,weight);
			//return SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, uv_new +isWater, index).rgb * weight;
#endif
			return SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, uv_new, index).rgb * weight;
		}
		half3 AddLighting(Light light, half3 normalWS)
		{
			half nol = dot(normalWS, light.direction);
			half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
			half3 diffuse = attenuatedLightColor * saturate(nol);
			return diffuse;
		}
		half4 frag(v2f i) : SV_Target
		{
			UNITY_SETUP_INSTANCE_ID(i);

			//#ifdef HOLLOWON
				//clip(0.99 - SAMPLE_TEXTURE2D(_HollowMap, sampler_ControlTex, i.uv.xy).r);
			//#endif
			//clip(step((i.worldPos.x - 256)* (i.worldPos.x),0.0001)* step((i.worldPos.z - 256)* (i.worldPos.z),0.0001) - 0.001);

			float alpha = step((i.worldPos.x - 256) * (i.worldPos.x), 0.0001) * step((i.worldPos.z - 256) * (i.worldPos.z), 0.0001);
#ifdef HOLLOWON
			alpha *= step(0, 0.99 - SAMPLE_TEXTURE2D(_HollowMap, sampler_ControlTex, i.uv.xy).r);
#endif
			clip(alpha - 0.001);


			half4 controlTex = SAMPLE_TEXTURE2D(_ControlTex, sampler_ControlTex, i.uv.xy) * 16.5;
			half4 col = half4(0, 0, 0, 1);

			int indexs1 = floor(controlTex.r);
			int indexs2 = floor(controlTex.g);
			int indexs3 = floor(controlTex.b);
			half weight1 = SAMPLE_TEXTURE2D_ARRAY(_WeightArr, sampler_WeightArr, i.uv.xy, indexs1).r;
			//half weight2 = 1 - weight1;
			half weight2 = SAMPLE_TEXTURE2D_ARRAY(_WeightArr, sampler_WeightArr, i.uv.xy, indexs2).r;
			half weight3 = 1 - weight1 -weight2;
			//float weight3 = SAMPLE_TEXTURE2D_ARRAY(_WeightArr, sampler_WeightArr, i.uv.xy, indexs3).r;
			//float rate = 1 / (weight1 + weight2 + weight3);
			//weight1 *= rate;
			//weight2 *= rate;
			//weight3 *= rate;

			float2 offset = float2(0,0);

			#ifdef WATERON
				float sinValue = sin((_Time.y + i.uv.x * 128) * 2);
				float uvz = (_Time.y * 0.2);
				offset.x = sinValue * 0.02;
				offset.y = offset.x;
				offset.x += uvz;
			#endif

			col.rgb += getColor(indexs1, i.uv, offset, weight1);
			col.rgb += getColor(indexs2, i.uv, offset, weight2);
			col.rgb += getColor(indexs3, i.uv, offset, weight3);
			//col.rgb *= SAMPLE_TEXTURE2D(Terrain_AmbientTex,sampler_MainTex,i.uv.xy).rgb;
			#ifdef GroundColor
				col.rgb *= GetTerrainAmbientColor(i.worldPos)*_GroundTexIntensity;
			#endif
			//half3 addColor = 0;
			//uint pixelLightCount = GetAdditionalLightsCount();
			/*for (uint lightIndex = 0u; lightIndex < pixelLightCount; ++lightIndex)
			{
				Light light = GetAdditionalLight(lightIndex, i.worldPos);
				addColor += AddLighting(light, i.worldNormal);
			}
			col.rgb += col.rgb * addColor;*/

			SCENE_HIGHT_FOG(col,i.worldPos.y);
			return col;
		}
		ENDHLSL
		}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+100"  "IgnoreProjector" = "True" }
		LOD 100
		Pass
		{
			Tags { "LightMode" = "UniversalForward"}
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite On
			Stencil
			{
				Ref 5
				Comp Always
				Pass Replace
			}
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma	shader_feature _ GroundColor
			//#pragma shader_feature _ WATERON
			#pragma shader_feature _ HOLLOWON

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "../Lib/SceneHeightFog.hlsl"
			#include "../Lib/TerrainAmbient.hlsl"

			CBUFFER_START(terrain)
			SAMPLER(Terrain_HeightMap);
			TEXTURE2D(_HollowMap);
			TEXTURE2D_ARRAY(_MainTex);   SAMPLER(sampler_MainTex);   float4 _MainTex_ST;
			TEXTURE2D(_ControlTex);    SAMPLER(sampler_ControlTex);   
			TEXTURE2D_ARRAY(_WeightArr);   SAMPLER(sampler_WeightArr);

			float _WaterIndex;
			float _WaterFlowCutOff;
			float _GroundTexIntensity;
			float4 _MainTex_STs[16];
			float _HeightMapScale_MinY;
			float _HeightMapScale;
			CBUFFER_END

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float4 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float3 worldNormal : TEXCOORD2;
			};

			v2f vert(appdata v)
			{
				v2f o;
				half4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				float4 heightMapColor = tex2Dlod(Terrain_HeightMap, float4(float2((2 + worldPos.x) * 0.00390625, worldPos.z * 0.00390625), 0, 0));
				v.vertex.y = _HeightMapScale_MinY + heightMapColor.r * _HeightMapScale;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv.xy = float2(worldPos.x * 0.00390625, worldPos.z * 0.00390625);
				float3 worldNormal = TransformObjectToWorldNormal(v.normal);
				o.worldNormal = normalize(worldNormal);
				o.worldPos = worldPos.xyz;
				return o;
			}

			half3 getColor(int index, float4 uv, /*float2 offset,*/ float weight)
			{
				float4 st = _MainTex_STs[index];
				float2 uv_new = uv.xy * st.xy + st.zw;
				return SAMPLE_TEXTURE2D_ARRAY(_MainTex, sampler_MainTex, uv_new, index).rgb * weight;
			}
			half3 AddLighting(Light light, half3 normalWS)
			{
				half nol = dot(normalWS, light.direction);
				half3 attenuatedLightColor = light.color * (light.distanceAttenuation * light.shadowAttenuation);
				half3 diffuse = attenuatedLightColor * saturate(nol);
				return diffuse;
			}
			half4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);

				//#ifdef HOLLOWON
				//clip(0.99 - SAMPLE_TEXTURE2D(_HollowMap, sampler_ControlTex, i.uv.xy).r);
				//#endif
				//clip(step((i.worldPos.x - 256)* (i.worldPos.x),0.0001)* step((i.worldPos.z - 256)* (i.worldPos.z),0.0001) - 0.001);

				float alpha = step((i.worldPos.x - 256) * (i.worldPos.x), 0.0001) * step((i.worldPos.z - 256) * (i.worldPos.z), 0.0001);
#ifdef HOLLOWON
				alpha *= step(0, 0.99 - SAMPLE_TEXTURE2D(_HollowMap, sampler_ControlTex, i.uv.xy).r);
#endif
				clip(alpha - 0.001);

				half4 controlTex = SAMPLE_TEXTURE2D(_ControlTex, sampler_ControlTex, i.uv.xy) * 16.5;
				half4 col = half4(0, 0, 0, 1);

				int indexs1 = floor(controlTex.r);
				int indexs2 = floor(controlTex.g);
				int indexs3 = floor(controlTex.b);
				half weight1 = SAMPLE_TEXTURE2D_ARRAY(_WeightArr, sampler_WeightArr, i.uv.xy, indexs1).r;
				half weight2 = SAMPLE_TEXTURE2D_ARRAY(_WeightArr, sampler_WeightArr, i.uv.xy, indexs2).r;
				half weight3 = 1 - weight1 - weight2;

				//float2 offset = float2(0,0);

				col.rgb += getColor(indexs1, i.uv, /*offset,*/ weight1);
				col.rgb += getColor(indexs2, i.uv, /*offset,*/ weight2);
				col.rgb += getColor(indexs3, i.uv, /*offset,*/ weight3);
				#ifdef GroundColor
				col.rgb *= GetTerrainAmbientColor(i.worldPos) * _GroundTexIntensity;
				#endif

				SCENE_HIGHT_FOG(col,i.worldPos.y);
				return col;
		}
		ENDHLSL
		}
	}
}
