Shader "FGQJ/Effect/ParticleAddFlowing"
{
    Properties
    {
        [Toggle(_ALPHAASMASK)]_ALPHAASMASK("_ALPHAASMASK",float) = 0
        _TintColor("Base Color", Color) = (1,1,1,1)
        _MainTex("Base Map", 2D) = "white" {}
        [Toggle(_SEQUENCE_ON)]_SEQUENCE_ON("开启序列帧",float) = 0
        _Sequence_X("序列帧水平帧数", Float) = 1
        _Sequence_Y("序列帧垂直帧数", Float) = 1
        _SequenceSpeed("_Speed",float) = 1
        [Space(20)]
        _ScaleModel("ScaleModel", Float) = 1
        _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
        _FlowFactor("_FlowFactor",vector) = (0,1,0,0)
        // _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

        // [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp("BlendOp", Float) = 0
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 0
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", Float) = 1
        // [Enum(UnityEngine.Rendering.BlendMode)] _SrcAlphaBlend("Src Alpha Blend Mode", Float) = 1
        // [Enum(UnityEngine.Rendering.BlendMode)] _DstAlphaBlend("Dst Alpha Blend Mode", Float) = 10
    }

    SubShader
    {
        Tags { "Queue"="Transparent+10" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Pass
        {

            // BlendOp[_BlendOp]
            // Blend [_SrcBlend][_DstBlend],[_SrcAlphaBlend][_DstAlphaBlend]
            // Blend SrcAlpha One
            Cull[_Cull]
            Blend [_SrcBlend] [_DstBlend]
            ZWrite Off
            ColorMask RGB

            Stencil
            {
                Ref 2//参考值为1
                Comp notequal//和模板缓冲区中的数据是否相等
                Pass keep// 测试通过后不改变模板缓冲区中的内容，我不是模板，只负责被遮罩
            }

            HLSLPROGRAM
            // Required to compile gles 2.0 with standard SRP library
            // All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0

            // Particle Keywords
            #pragma shader_feature _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
            #pragma shader_feature _FLIPBOOKBLENDING_ON
            #pragma shader_feature _SOFTPARTICLES_ON
            #pragma shader_feature _FADING_ON
            #pragma shader_feature _DISTORTION_ON
            #pragma shader_feature _ _ALPHAASMASK
            #pragma shader_feature _ _SEQUENCE_ON

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog

            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            // #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitInput.hlsl"
            // #include "Packages/com.unity.render-pipelines.universal/Shaders/Particles/ParticlesUnlitForwardPass.hlsl"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 pos : SV_POSITION;
                half4 color : COLOR;
                float4 uv : TEXCOORD0;
                // UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                    float4 projPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _TintColor;
            float _ScaleModel;
            float _InvFade;
            float _Sequence_X;
            float _Sequence_Y;
            float _SequenceSpeed;
            float4 _FlowFactor;
            CBUFFER_END


            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v,o);
                float4x4 scale = float4x4(float4(_ScaleModel, 0, 0, 0), float4(0, _ScaleModel, 0, 0), float4(0, 0, _ScaleModel, 0), float4(0, 0, 0, 1));
                o.pos = TransformObjectToHClip(mul(scale, v.vertex).xyz);
                float2 uv = v.uv;
                #ifdef _SEQUENCE_ON
                    float time = floor(_Time.y*_SequenceSpeed);
                    float r = floor(time/_Sequence_X);
                    float c = time - r*_Sequence_Y;
                    // uv = uv + float2(c,-r);
                    // uv.x /= _Sequence_X;
                    // uv.y /= _Sequence_Y;

                    uv.x += c / _Sequence_X;//模型上uv范围并非(0,1)
                    uv.y += r / _Sequence_Y;
                #endif
                o.uv.xy = TRANSFORM_TEX(uv, _MainTex)+_Time.xx*_FlowFactor.xy;
                #ifdef _ALPHAASMASK
                    o.uv.zw = TRANSFORM_TEX(v.uv, _MainTex);
                #endif
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy);
                #ifdef _ALPHAASMASK
                    col.a = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.zw).a;
                #endif
                #ifdef SOFTPARTICLES_ON
                    float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                    float partZ = i.projPos.z;
                    float fade = saturate(_InvFade*(sceneZ-partZ));
                    i.color.a *= fade;
                #endif
                col *= 2*_TintColor*i.color;
                col.a = saturate(col.a);

                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDHLSL
        }
    }
    // CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.ParticlesUnlitShader"
}
