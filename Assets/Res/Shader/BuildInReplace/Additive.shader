Shader "FGQJ/Effect/Additive"
{
	Properties{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_Color("Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}


		[Header(_____________________UI_____________________)]
		_Stencil("Stencil ID", Float) = 2
		[Enum(UnityEngine.Rendering.CompareFunction)]_StencilComp("Stencil Comparison", Float) = 6
		[Enum(UnityEngine.Rendering.StencilOp)] _StencilOp("Stencil Operation", Float) = 0
		//_StencilWriteMask("Stencil Write Mask", Float) = 255
		//_StencilReadMask("Stencil Read Mask", Float) = 255

		[HideInInspector]_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
		[Enum(Off,0,On,1)] _ZWrite("ZWrite",Float) = 0
		[Enum(Off,0,Front,1,Back,2)] _CullMode("Culling", Float) = 0 //0 = off, 2=back
		//[Enum(Alpha Blend,10,Addtive,1)] _DestBlend("Dest Blend Mode", Float) = 10
		//[Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 4
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" }
		//Blend SrcAlpha One
		ColorMask RGB
		//Cull Off Lighting Off ZWrite Off
		Pass
		{
			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				//ReadMask[_StencilReadMask]
				//WriteMask[_StencilWriteMask]

				//Comp notequal//和模板缓冲区中的数据是否相等
				//Pass keep// 测试通过后不改变模板缓冲区中的内容，我不是模板，只负责被遮罩
			}

			Blend SrcAlpha One
			//Blend SrcAlpha[_DestBlend]
			Cull[_CullMode]
			Lighting off
			ZWrite[_ZWrite]
			ZTest[_ZTest]
			//Offset[_Zoffset],0


			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			CBUFFER_START(UnityPerMaterial)
			
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
				o.color = v.color;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);

				half4 col = 2.0f * i.color * _TintColor * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy); 
				col.a = saturate(col.a); 
				return col;
			}
			ENDHLSL
		}
	}
}
