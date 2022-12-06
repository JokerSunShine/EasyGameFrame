Shader "FGQJ/Effect/Alpha Blended" {
	Properties{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_Color("Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
		[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
	}

	SubShader {
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		Blend SrcAlpha OneMinusSrcAlpha
		ColorMask RGB
		Cull [_Cull] Lighting Off ZWrite Off

			Stencil
			{
				Ref 2//参考值为1
				Comp notequal//和模板缓冲区中的数据是否相等
				Pass keep// 测试通过后不改变模板缓冲区中的内容，我不是模板，只负责被遮罩
			}
		Pass {

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			

			CBUFFER_START(UnityPerMaterial)
			//sampler2D _MainTex;
			half4 _TintColor;
			half4 _Color;
			float4 _MainTex_ST;
			CBUFFER_END
				TEXTURE2D(_MainTex);
			SAMPLER(sampler_MainTex);
			struct appdata_t {
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = TransformObjectToHClip(v.vertex);
				o.color = v.color * _TintColor;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				half4 col = 2.0f * i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy);
				col.a = saturate(col.a);
				return col;
			}
			ENDHLSL
		}
	}
}
