Shader "FGQJ/Scene/Flicker"
{
    Properties
    {
        [Toggle(_Billboard_ON)]_Billboard_ON("_Billboard_ON",float) = 0
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        // _Cutoff("Alpha Cutoff", Range( 0 , 1)) = 0.5
        // _Flicker("_Flicker",Range(0,1)) = 1
        _Frequency("_Frequency",float) = 120
        _Offset("_Offset",float) = 0
        _Lower("_Lower",Range(0,1)) = 0.2
        [Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 1
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", Float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+100" "IgnoreProjector"="True" }
        LOD 100
        Pass
        {
            Tags { "LightMode" = "UniversalForward"}
            Cull[_Cull]
            ZWrite[_ZWrite]
            // Blend One DstAlpha
            Blend [_SrcBlend] [_DstBlend]
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma shader_feature _ _Billboard_ON
            #pragma target 2.0
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float3 uv : TEXCOORD0;
                // float3 lightColor : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;
            // float _Cutoff;
            float _Offset;
            // float _Flicker;
            float _Lower;
            float _Frequency;
            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v,o);
                //billboard
                #if _Billboard_ON
                    float3 offset = mul((float3x3)UNITY_MATRIX_M,v.vertex.xyz);//模型空间内的旋转缩放
                    //o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV, float4(0, 0, 0, 1)) + float4(offset.x, offset.y, 0.0, 0.0));//z轴朝向相机
                    o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_MV,float4(0,0,0,1)) + float4(offset.x,offset.z,0.0,0.0));//y轴朝向相机
                #else
                    o.pos = TransformObjectToHClip(v.vertex.xyz);
                #endif
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
                float3 centerPos = TransformObjectToWorld(float3(0,1,0));
                o.uv.z = centerPos.y;
                // UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            //https://www.desmos.com/calculator
            inline float Flick(float offset)
            {
                offset += _Offset;
                float t = _Time.x*_Frequency+offset;
                t = sin(t)+sin(2.718281828*t);
                t = clamp(t,_Lower,1);
                return t;
            }

            half4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                // clip(col.a-_Cutoff);
                col.rgb *= _Color.rgb*_Color.a;
                float flicker = Flick(i.uv.z);//需要个偏移避免所有灯同步闪烁,to do: 尝试instanceID做偏移，节省计算
                col.rgb *= flicker;
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDHLSL
        }
    }
}
