Shader "Hidden/VirtualCircle"
{
    Properties
    {
		_MainTex ("Texture", 2D) = "white" {}
		_CircleColor("CircleColor1", Color) = (1,1,1,1)
		_Center_X("X",Float) = 0.5
		_Cetner_Y("Y",FLoat) = 0.5
		_Width("Width", Float) = 0.2
		
        _StencilComp("Stencil Comparison", Float) = 6.75
        [HideInInspector]_Stencil("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        //[HideInInspector]_ColorMask ("Color Mask", Float) = 15
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
		    Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass [_StencilOp] 
                ReadMask [_StencilReadMask]
                WriteMask [_StencilWriteMask]
            }
            //ColorMask [_ColorMask]
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };
			
			struct v2f
            {
                float4 vertex   : SV_POSITION; 
                fixed4 color : COLOR;
                half2 texcoord  : TEXCOORD0;
                float2 uv : TEXCOORD0;
            };

 
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _CircleColor;
			fixed _Width;
		    fixed _Center_X;
		    fixed _Cetner_Y;
            fixed4 _Color;
 
			v2f vert (appdata_t IN)
			{
			    v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                        OUT.texcoord = IN.texcoord;
                #ifdef UNITY_HALF_TEXEL_OFFSET     
                        OUT.vertex.xy -= (_ScreenParams.zw - 1.0);
                #endif     
                OUT.color = IN.color;
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _MainTex);
				return OUT;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;
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
