Shader "Shaders/Grass" {
    Properties {
        // Shader properties which are editable in the material
        _BaseColor("Base color", Color) = (0, 0.5, 0, 1) // Color of the lowest layer
        _TipColor("Tip color", Color) = (0, 1, 0, 1) // Color of the highest layer
        _BakedGI("BakedGI", Range(0.0, 1.0)) = 0.75
        _Metal("Metallic", Range(0.0, 1.0)) = 0.75
        _Smooth("Smooth", Range(0.0, 1.0)) = 0.75
        _Timer("Time", float) = 0
        
        _Noise("Noise", 2D) = "white" {}
        _NoiseScale("NoiseScale", Range(0.0, .185)) = .15
        _Test2("Test2", Range(0.0, 1.0)) = 0.75
        _WindSpeed("Wind Speed", Range(0.0, 1.0)) = 0.75
        _WindIntensity("Wind Intensity", Range(0.0, 1.0)) = 0.75
        _WindDirection("Wind Direction", Vector) = (0, 0, 0, 0)
        
        _CloudScale("Cloud Scale", Range(0.0, .185)) = .15
        _CloudSpeed("Cloud Speed", Range(0.0, 1.0)) = 0.75
        _CloudSpotColor("Cloud Spot Color", Color) = (0, 1, 0, 1) // Color of the highest layer
    }
    SubShader{
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        // Forward Lit Pass. The main pass which renders colors
        Pass {

            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            Cull Off
            
            HLSLPROGRAM
            // Signal this shader requires compute buffers
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 5.0

            // Lighting and shadow keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            // Register our functions
            #pragma vertex vert
            #pragma fragment frag

            // Include our logic file
            #include "Assets/Shaders/HLSL/Grass.hlsl"

            ENDHLSL
        }

        // Shadow caster pass. This pass renders a shadow map.
        // We treat it almost the same, except strip out any color/lighting logic
        // Pass {

        //     Name "ShadowCaster"
        //     Tags { "LightMode" = "ShadowCaster" }

        //     HLSLPROGRAM
        //     // Signal this shader requires compute buffers
        //     #pragma prefer_hlslcc gles
        //     #pragma exclude_renderers d3d11_9x
        //     #pragma target 5.0

        //     // This sets up various keywords for different light types and shadow settings
        //     #pragma multi_compile_shadowcaster

        //     // Register our functions
        //     #pragma vertex vert
        //     #pragma fragment frag

        //     // Define a special keyword so our logic can change if inside the shadow caster pass
        //     #define SHADOW_CASTER_PASS

        //     // Include our logic file
        //     #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
        //     #include "Assets/Shaders/HLSL/Grass.hlsl"

        //     ENDHLSL
        // }
    }
}