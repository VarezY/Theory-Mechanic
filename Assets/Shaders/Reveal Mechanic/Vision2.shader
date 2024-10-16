Shader "PostEffect/Vision2"
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

        float3 CalculateLighting(float3 normalWS, float3 positionWS, uint renderingLayer)
        {
            float3 lighting = 0;
            Light mainLight = GetMainLight(TransformWorldToShadowCoord(positionWS));
            if (IsMatchingLightLayer(mainLight.layerMask, renderingLayer))
            {
                lighting += mainLight.color * mainLight.distanceAttenuation * mainLight.shadowAttenuation * saturate(dot(normalWS, mainLight.direction));
            }

            #ifdef _ADDITIONAL_LIGHTS
            uint additionalLightsCount = GetAdditionalLightsCount();
            for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
            {
                Light light = GetAdditionalLight(lightIndex, positionWS);
                if (IsMatchingLightLayer(light.layerMask, renderingLayer))
                {
                    float intensity = light.distanceAttenuation * light.shadowAttenuation;
                    lighting += light.color * intensity * saturate(dot(normalWS, light.direction));
                }
            }
            #endif

            return lighting;
        }

        float CalculateRevealFactor(float3 normalWS, float3 positionWS, uint renderingLayer)
        {
            float maxIntensity = 0;
            Light mainLight = GetMainLight(TransformWorldToShadowCoord(positionWS));
            if (IsMatchingLightLayer(mainLight.layerMask, renderingLayer))
            {
                maxIntensity = max(maxIntensity, mainLight.distanceAttenuation * mainLight.shadowAttenuation);
            }

            #ifdef _ADDITIONAL_LIGHTS
            uint additionalLightsCount = GetAdditionalLightsCount();
            for (uint lightIndex = 0u; lightIndex < additionalLightsCount; ++lightIndex)
            {
                Light light = GetAdditionalLight(lightIndex, positionWS);
                if (IsMatchingLightLayer(light.layerMask, renderingLayer))
                {
                    float intensity = light.distanceAttenuation * light.shadowAttenuation;
                    maxIntensity = max(maxIntensity, intensity);
                }
            }
            #endif

            return smoothstep(_RevealThreshold, _RevealThreshold + _RevealSmoothness, maxIntensity);
        }
        ENDHLSL

        // First Pass: Back faces
        /*Pass
        {
            Name "BackFace"
            Tags {"LightMode"="UniversalForward"}
            
            Blend One OneMinusSrcAlpha
            Cull Front

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
                float3 normalWS = normalize(-IN.normalWS); // Flip normal for back faces

                float3 lighting = CalculateLighting(normalWS, IN.positionWS, _RenderingLayer);
                float revealFactor = CalculateRevealFactor(normalWS, IN.positionWS, _RenderingLayer);

                half3 color = texColor.rgb * lighting;
                return half4(color, texColor.a * revealFactor);
            }
            ENDHLSL
        }*/

        // Second Pass: Front faces
        Pass
        {
            Name "FrontFace"
            Tags {"LightMode"="UniversalForward"}
            
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Back

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

                float3 lighting = CalculateLighting(normalWS, IN.positionWS, _RenderingLayer);
                float revealFactor = CalculateRevealFactor(normalWS, IN.positionWS, _RenderingLayer);

                half3 color = texColor.rgb * lighting;
                return half4(color, texColor.a * revealFactor);
            }
            ENDHLSL
        }
    }
}

