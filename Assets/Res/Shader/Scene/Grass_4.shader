// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//效果LOD分3级，低：100，中：500，高：900
Shader "FGQJ/Scene/Grass_4" {
	Properties{
		[Toggle(DebugGround)]_DebugGround("DebugGround",float) = 0
		//Terrain_HeightMap("Terrain_HeightMap", 2D) = "white" {}
		// Terrain_HeightOffset("Terrain_HeightOffset", float) = 0
		_Color("Main Color", Color) = (1,1,1,1)
		//Terrain_AmbientTex("_GroundTex", 2D) = "white" {}
		_MainTex("MainTex", 2D) = "white" {}
		_UVXStart("X轴偏移起始位置",Range(0,1)) = 0
		_UVXScal("X轴偏移伸缩系数",float) = 1

		_AngleX("AngleX",float) = 15
		// _Cutoff("Alpha Cutoff", Range( 0 , 1)) = 0.5
		//_WaveParams("Wave Params", Vector) = (0.05, 5, 0.8, 1)

		_AmplitudeZ("纵向幅度",float) = 0.6
		_PowZ("纵向幅度缩放",int) = 2
		_WavelengthZ("纵向波长",float) = 0.1
		_WaveSpeedZ("纵向摆动速度",float) = 2
		//_PhaseXScale("初相偏移倍率",float) = 0.1

		_AmplitudeX("横向幅度",float) = 0
		//_WavelengthX("横向波长",Range(0,1000)) = 0.1
		_VertXOffset("横向倾斜",float) = 0

		[Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 0
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", Float) = 10
	}

	SubShader
	{
		Tags
		{
			"Queue"="Transparent-1"
			"RenderType"="Transparent"
			"IgnoreProject"="True"
			"DisableBatching" = "True"
		}
		LOD 500 //中高级带摇动顶点动画
		Cull Off
		// ZWrite Off
		ZWrite [_ZWrite]
		Blend [_SrcBlend] [_DstBlend]
		// Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// #pragma shader_feature _ DebugGround
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct appdata_t
			{
				half4 vertex   : POSITION;
				half2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				half4 vertex : SV_POSITION;
				half4 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			//CBUFFER_START(UnityPerMaterial)
			sampler2D Terrain_HeightMap;
			half4 Terrain_HeightMap_ST;
			float Terrain_HeightOffset;
			sampler2D _MainTex;
			half4 _MainTex_ST;
			sampler2D _Terrain_AmbientTex;
			half4 _Terrain_AmbientTex_ST;
			//half4 _WaveParams;
			half4 _Color;
			// half _Cutoff;
			float _AngleX;

			half _AmplitudeZ;
			int _PowZ;
			half _WavelengthZ;
			half _WaveSpeedZ;
			half _AmplitudeX;
			//half _PhaseXScale;
			//half _WavelengthX;
			//half _WaveSpeedX;
			half _VertXOffset;

			float _GlobalAmbientIntensity;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(half, _UVXStart)
			UNITY_DEFINE_INSTANCED_PROP(half, _UVXScal)
			UNITY_INSTANCING_BUFFER_END(Props)
			//CBUFFER_END


			v2f vert(appdata_t IN)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_TRANSFER_INSTANCE_ID(IN, o);

				half uvxstart = UNITY_ACCESS_INSTANCED_PROP(Props, _UVXStart);
				half uvxscal = UNITY_ACCESS_INSTANCED_PROP(Props, _UVXScal);

				half4 localPos = IN.vertex;
				half4 worldPos = mul(unity_ObjectToWorld, IN.vertex);

				float4 heightCoord = float4((worldPos.x + 2) * 0.00390625 + 0.001, worldPos.z * 0.00390625 + 0.001, 0, 0);
				float4 height = tex2Dlod(Terrain_HeightMap, heightCoord);
				float AngleX = localPos.y * sin(_AngleX * 0.0174);
				localPos.y = localPos.y * cos(_AngleX * 0.0174);
				localPos.y += height.r * 4.1 + Terrain_HeightOffset;

				float wave = worldPos.x;
				float waver = _AmplitudeZ * sin(_WavelengthZ * wave * 0.01 + _WaveSpeedZ * _Time.y) * pow(IN.texcoord.y, _PowZ);
				localPos.z = IN.vertex.z + waver + AngleX;
				waver = _AmplitudeX * waver;
				localPos.x = IN.vertex.x + (waver + _VertXOffset) * IN.texcoord.y / uvxscal ;

				o.vertex = UnityObjectToClipPos(localPos);


				o.texcoord.xy = TRANSFORM_TEX(IN.texcoord, _MainTex);
				o.texcoord.x = o.texcoord.x * uvxscal + uvxstart;

				//float4 uv_ST = _GroundTex_ST * 0.0001;//数值给的比较大方便美术调整
				//float4 uv_ST = _GroundTex_ST;
				//o.texcoord.zw = float2(worldPos.z, -worldPos.x) * uv_ST.xy + uv_ST.zw;

				o.texcoord.zw = half2(worldPos.x * 0.00390625, worldPos.z * 0.00390625);
				return o;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);

				half4 col = tex2D(_MainTex, IN.texcoord.xy);

				half4 groundColor = tex2D(_Terrain_AmbientTex, IN.texcoord.zw);
				// #ifdef DebugGround
				// col = groundColor;
				// col.a = _Cutoff;
				// #else
				// clip(col.a-_Cutoff);
				col.rgb *= groundColor * _GlobalAmbientIntensity;//默认打开环境光
				// #endif
				col *= _Color;

				return col;
			}
			ENDCG
		}
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent-1"
			"RenderType" = "Transparent"
			"IgnoreProject" = "True"
			"DisableBatching" = "True"
		}
		LOD 100 //低级不带摇动顶点动画
		Cull Off
		ZWrite[_ZWrite]
		Blend[_SrcBlend][_DstBlend]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"

			struct appdata_t
			{
				half4 vertex   : POSITION;
				half2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				half4 vertex : SV_POSITION;
				half4 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			//CBUFFER_START(UnityPerMaterial)
			sampler2D Terrain_HeightMap;
			half4 Terrain_HeightMap_ST;
			float Terrain_HeightOffset;
			sampler2D _MainTex;
			half4 _MainTex_ST;
			sampler2D _Terrain_AmbientTex;
			half4 _Terrain_AmbientTex_ST;
			//half4 _WaveParams;
			half4 _Color;
			// half _Cutoff;
			float _AngleX;

			half _AmplitudeZ;
			int _PowZ;
			half _WavelengthZ;
			half _WaveSpeedZ;
			half _AmplitudeX;
			//half _PhaseXScale;
			//half _WavelengthX;
			//half _WaveSpeedX;
			half _VertXOffset;

			float _GlobalAmbientIntensity;

			UNITY_INSTANCING_BUFFER_START(Props)
			UNITY_DEFINE_INSTANCED_PROP(half, _UVXStart)
			UNITY_DEFINE_INSTANCED_PROP(half, _UVXScal)
			UNITY_INSTANCING_BUFFER_END(Props)
				//CBUFFER_END


				v2f vert(appdata_t IN)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(IN);
					UNITY_TRANSFER_INSTANCE_ID(IN, o);

					half uvxstart = UNITY_ACCESS_INSTANCED_PROP(Props, _UVXStart);
					half uvxscal = UNITY_ACCESS_INSTANCED_PROP(Props, _UVXScal);

					half4 localPos = IN.vertex;
					half4 worldPos = mul(unity_ObjectToWorld, IN.vertex);

					float4 heightCoord = float4((worldPos.x + 2) * 0.00390625 + 0.001, worldPos.z * 0.00390625 + 0.001, 0, 0);
					float4 height = tex2Dlod(Terrain_HeightMap, heightCoord);
					float AngleX = localPos.y * sin(_AngleX * 0.0174);
					localPos.y = localPos.y * cos(_AngleX * 0.0174);
					localPos.y += height.r * 4.1 + Terrain_HeightOffset;

					//float wave = worldPos.x;
					//float waver = _AmplitudeZ * sin(_WavelengthZ * wave * 0.01 + _WaveSpeedZ * _Time.y) * pow(IN.texcoord.y, _PowZ);
					localPos.z = IN.vertex.z /*+ waver*/ + AngleX;
					//waver = _AmplitudeX * waver;
					localPos.x = IN.vertex.x + _VertXOffset * IN.texcoord.y / uvxscal;
					o.vertex = UnityObjectToClipPos(localPos);

					o.texcoord.xy = TRANSFORM_TEX(IN.texcoord, _MainTex);
					o.texcoord.x = o.texcoord.x * uvxscal + uvxstart;
					o.texcoord.zw = half2(worldPos.x * 0.00390625, worldPos.z * 0.00390625);
					return o;
				}

				fixed4 frag(v2f IN) : SV_Target
				{
					UNITY_SETUP_INSTANCE_ID(IN);

					half4 col = tex2D(_MainTex, IN.texcoord.xy);

					half4 groundColor = tex2D(_Terrain_AmbientTex, IN.texcoord.zw);
					// #ifdef DebugGround
					// col = groundColor;
					// col.a = _Cutoff;
					// #else
					// clip(col.a-_Cutoff);
					col.rgb *= groundColor * _GlobalAmbientIntensity;//默认打开环境光
					// #endif
					col *= _Color;

					return col;
				}
				ENDCG
			}
	}





	//SubShader
	//{
	//	Tags
	//	{
	//		"Queue"="Transparent"
	//		"RenderType"="Transparent"
	//		"IgnoreProject"="True"
	//		"DisableBatching" = "True"
	//	}
	//	LOD 100 //目前低级砍掉顶点动画效果
	//	Cull Off
	//	ZWrite [_ZWrite]
	//	Blend [_SrcBlend] [_DstBlend]

	//	Pass
	//	{
	//		CGPROGRAM
	//		#pragma vertex vert
	//		#pragma fragment frag
	//		// #pragma shader_feature _ DebugGround
	//		#pragma multi_compile_instancing
	//		#include "UnityCG.cginc"

	//		struct appdata_t
	//		{
	//			half4 vertex   : POSITION;
	//			half2 texcoord : TEXCOORD0;
	//			UNITY_VERTEX_INPUT_INSTANCE_ID
	//		};

	//		struct v2f
	//		{
	//			half4 vertex : SV_POSITION;
	//			half4 texcoord  : TEXCOORD0;
	//			UNITY_VERTEX_INPUT_INSTANCE_ID
	//		};

	//		//CBUFFER_START(UnityPerMaterial)
	//		sampler2D Terrain_HeightMap;
	//		half4 Terrain_HeightMap_ST;
	//		float Terrain_HeightOffset;
	//		sampler2D _MainTex;
	//		half4 _MainTex_ST;
	//		sampler2D Terrain_AmbientTex;
	//		half4 Terrain_AmbientTex_ST;
	//		half4 _WaveParams;
	//		half4 _Color;
	//		half _Cutoff;
	//		half4 _MapInfo;

	//		half _VertXOffset;
	//		float _AngleX;

	//		float _GlobalAmbientIntensity;

	//		UNITY_INSTANCING_BUFFER_START(Props)
	//		UNITY_DEFINE_INSTANCED_PROP(half, _UVXStart)
	//		UNITY_DEFINE_INSTANCED_PROP(half, _UVXScal)
	//		UNITY_INSTANCING_BUFFER_END(Props)
	//		//CBUFFER_END

	//		v2f vert(appdata_t IN)
	//		{
	//			v2f o;
	//			UNITY_SETUP_INSTANCE_ID(IN);
	//			UNITY_TRANSFER_INSTANCE_ID(IN, o);

	//			half uvxstart = UNITY_ACCESS_INSTANCED_PROP(Props, _UVXStart);
	//			half uvxscal = UNITY_ACCESS_INSTANCED_PROP(Props, _UVXScal);
	//			half4 worldPos = mul(unity_ObjectToWorld, IN.vertex);
	//			half4 localPos = IN.vertex;
	//			float4 heightCoord = float4((worldPos.x + 2) * 0.00390625 + 0.001, worldPos.z * 0.00390625 + 0.001, 0, 0);
	//			float4 height = tex2Dlod(Terrain_HeightMap, heightCoord);
	//			float AngleX = localPos.y * sin(_AngleX * 0.0174);
	//			localPos.y = localPos.y * cos(_AngleX * 0.0174);
	//			//localPos.y += height.r * 4.1 + Terrain_HeightOffset;

	//			localPos.x = IN.vertex.x + _VertXOffset * IN.vertex.y / uvxscal;

	//			o.vertex = UnityObjectToClipPos(localPos);

	//			o.texcoord.xy = TRANSFORM_TEX(IN.texcoord, _MainTex);
	//			o.texcoord.x = o.texcoord.x * uvxscal + uvxstart;

	//			o.texcoord.zw = half2(worldPos.x * 0.00390625, worldPos.z * 0.00390625);
	//			return o;
	//		}

	//		fixed4 frag(v2f IN) : SV_Target
	//		{
	//			UNITY_SETUP_INSTANCE_ID(IN);

	//			half4 col = tex2D(_MainTex, IN.texcoord.xy);
	//			return col;
	//			half4 groundColor = tex2D(Terrain_AmbientTex, IN.texcoord.zw);
	//			// #ifdef DebugGround
	//			// col = groundColor;
	//			// col.a = _Cutoff;
	//			// #else
	//			// clip(col.a-_Cutoff);
	//			col.rgb *= groundColor * _GlobalAmbientIntensity;
	//			// #endif
	//			col *= _Color;
	//			return col;
	//		}
	//		ENDCG
	//	}
	//}
}
