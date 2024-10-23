Shader "PostEffect/Vision"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _RevealThreshold ("Reveal Threshold", Range(0, 1)) = 0.1
        _RevealSmoothness ("Reveal Smoothness", Range(0, 1)) = 0.1
        [Enum(UnityEngine.Rendering.Universal.RenderingLayerMask)] _RenderingLayer("Rendering Layer", Float) = 1
    }
    SubShader
    {
        Tags {"RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline"}
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_ST;
        float4 _Color;
        float _RevealThreshold;
        float _RevealSmoothness;
        uint _RenderingLayer;
        CBUFFER_END

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        struct Attributes
        {
            float4 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float2 uv : TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
            float3 normalWS : TEXCOORD1;
            float3 positionWS : TEXCOORD2;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
        };
        ENDHLSL

        // First Pass: Depth Write
        Pass
        {
            Name "DepthOnly"
            Tags {"LightMode"="DepthOnly"}

            ZWrite On
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_TARGET
            {
                return 0;
            }
            ENDHLSL
        }

        // Second Pass: Main Render
        Pass
        {
            Name "ForwardLit"
            Tags {"LightMode"="UniversalForward"}
            
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _LIGHT_LAYERS

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = positionInputs.positionCS;
                OUT.positionWS = positionInputs.positionWS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;
                float3 normalWS = normalize(IN.normalWS);

                // Initialize lighting
                float3 lighting = 0;
                float maxIntensity = 0;

                // Main directional light
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(IN.positionWS));
                if (IsMatchingLightLayer(mainLight.layerMask, _RenderingLayer))
                {
                    lighting += mainLight.color * mainLight.distanceAttenuation * mainLight.shadowAttenuation * saturate(dot(normalWS, mainLight.direction));
                    maxIntensity = max(maxIntensity, mainLight.distanceAttenuation * mainLight.shadowAttenuation);
                }

                // Additional lights (including spotlights)
                #ifdef _ADDITIONAL_LIGHTS
                uint additionalLightsCount = GetAdditionalLightsCount();
                for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
                {
                    Light light = GetAdditionalLight(lightIndex, IN.positionWS);
                    if (IsMatchingLightLayer(light.layerMask, _RenderingLayer))
                    {
                        float intensity = light.distanceAttenuation * light.shadowAttenuation;
                        lighting += light.color * intensity * saturate(dot(normalWS, light.direction));
                        maxIntensity = max(maxIntensity, intensity);
                    }
                }
                #endif

                // Calculate reveal factor
                float revealFactor = smoothstep(_RevealThreshold, _RevealThreshold + _RevealSmoothness, maxIntensity);

                // Apply lighting and reveal factor
                half3 color = texColor.rgb * lighting;
                return half4(color, texColor.a * revealFactor);
            }
            ENDHLSL
        }
    }
}

