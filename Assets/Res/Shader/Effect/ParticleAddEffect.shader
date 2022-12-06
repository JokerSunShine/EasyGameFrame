Shader "FGQJ/Effect/ParticleAdd_Effect"
{
    Properties
    {
        _TintColor("Base Color", Color) = (1,1,1,1)
        _MainTex("Base Map", 2D) = "white" {}
        _InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
        _LightIntesity("_LightIntesity",float) = 1
        _ScaleModel("ScaleModel",float) = 1
        [Space(20)]
        [Toggle(_Fresnel)] _Fresnel ("Fresnel开关", float ) = 0
        [MaterialToggle] _FresnelInvert ("Fresnel反向", float) = 0
        _FresnelPower("_FresnelPower",float) = 1
        [space(20)]
        // _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull Mode", Float) = 2
        [Enum(Off, 0, On, 1)] _ZWrite("ZWrite", Float) = 0

        [Toggle(GroundColor)]_TurnOnGroundColor("是否打开环境光贴图",float) = 0
        // _GroundTex("环境光贴图", 2D) = "white" {}
        _GroundTexIntensity("环境光系数",float) = 2

        // [Enum(UnityEngine.Rendering.BlendOp)] _BlendOp("BlendOp", Float) = 0
        // [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend("Src Blend Mode", Float) = 5
        // [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend("Dst Blend Mode", Float) = 1
        // [Enum(UnityEngine.Rendering.BlendMode)] _SrcAlphaBlend("Src Alpha Blend Mode", Float) = 1
        // [Enum(UnityEngine.Rendering.BlendMode)] _DstAlphaBlend("Dst Alpha Blend Mode", Float) = 10
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
        Pass
        {
            // BlendOp[_BlendOp]
            // Blend [_SrcBlend][_DstBlend],[_SrcAlphaBlend][_DstAlphaBlend]
            Blend SrcAlpha One
            ZWrite [_ZWrite]
            Cull[_Cull]
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
			#pragma multi_compile_instancing
            // Particle Keywords
            #pragma shader_feature _ _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON
            #pragma shader_feature _ALPHATEST_ON
            #pragma shader_feature _ _COLOROVERLAY_ON _COLORCOLOR_ON _COLORADDSUBDIFF_ON
            #pragma shader_feature _FLIPBOOKBLENDING_ON
            #pragma shader_feature _SOFTPARTICLES_ON
            #pragma shader_feature _FADING_ON
            #pragma shader_feature _DISTORTION_ON
            #pragma shader_feature _Fresnel
            #pragma	shader_feature _ GroundColor

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
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 pos : SV_POSITION;
                #ifdef GroundColor
                    float4 uv : TEXCOORD0;
                #else
                    float2 uv : TEXCOORD0;
                #endif
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                half4 color : COLOR;
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
            float _InvFade;
            float _FresnelInvert;
            float _LightIntesity;
            float _FresnelPower;
            float _ScaleModel;

            TEXTURE2D(_Terrain_AmbientTex);//需代码传入
            SAMPLER(sampler_Terrain_AmbientTex);
            // float4 _Terrain_AmbientTex_ST;
            float _GroundTexIntensity;
            CBUFFER_END


            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v,o);
                float4x4 scale = float4x4(float4(_ScaleModel, 0, 0, 0), float4(0, _ScaleModel, 0, 0), float4(0, 0, _ScaleModel, 0), float4(0, 0, 0, 1));
                o.pos = TransformObjectToHClip(mul(scale,v.vertex).xyz);
                o.worldPos = TransformObjectToWorld(v.vertex.xyz);
                o.worldNormal = TransformObjectToWorldNormal(v.normal);
                o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);

                // #ifdef GroundColor
                // float4 uv_ST = _GroundTex_ST * 0.0001;//数值给的比较大方便美术调整
                // o.uv.zw = float2(o.worldPos.x, o.worldPos.z) * uv_ST.xy + uv_ST.zw;//跟草一样，世界位置坐标转为对环境光贴图的uv
                // #endif
                o.color = v.color;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);


                #ifdef GroundColor
                    col.rgb *= SAMPLE_TEXTURE2D(_Terrain_AmbientTex,sampler_Terrain_AmbientTex,i.worldPos.xz * 0.00390625) * _GroundTexIntensity;
                #endif

                #ifdef SOFTPARTICLES_ON
                    float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                    float partZ = i.projPos.z;
                    float fade = saturate(_InvFade*(sceneZ-partZ));
                    i.color.a *= fade;
                #endif

                col *= 2*_TintColor*i.color;
                col.rgb *= _LightIntesity;
                col.a = saturate(col.a);
                #ifdef _Fresnel
                    float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                    float fresnel = saturate(dot(i.worldNormal,viewDir));
                    fresnel = pow(fresnel,_FresnelPower);
                    fresnel = lerp(fresnel,(1-fresnel),_FresnelInvert);
                    col.a *= fresnel;
                #endif

                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDHLSL
        }
    }
    // CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.ParticlesUnlitShader"
}
