// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/VirtualCircle"
{
    Properties
    {
		_MainTex ("Texture", 2D) = "white" {}
		_CircleColor("CircleColor1", Color) = (1,1,1,1)
		_Center_X("X",Float) = 0.5
		_Cetner_Y("Y",FLoat) = 0.5
		_Width("Width", Float) = 0.2
    }
    SubShader
    {
	    Tags 
		{ 
			"Queue" = "Transparent"
			"RenderType"="Transparent"
			"IgnoreProjector" = "True"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
 
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
 
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
 
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
 
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _CircleColor;
			fixed _Width;
		    fixed _Center_X;
		    fixed _Cetner_Y;
 
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed widthOver2 = _Width * 0.5;
				float2 minPoint = float2(_Center_X - widthOver2,_Cetner_Y - widthOver2);
				float2 maxPoint = float2(_Center_X + widthOver2,_Cetner_Y + widthOver2);
				if(i.uv.x >= minPoint.x && i.uv.x <= maxPoint.x && i.uv.y >= minPoint.y && i.uv.y <= maxPoint.y)
				{
					col *= _CircleColor;
				}
				return col;
			}
			ENDCG
		}
    }
}
