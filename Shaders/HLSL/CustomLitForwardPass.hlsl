#ifndef UNIVERSAL_FORWARD_LIT_PASS_INCLUDED
#define UNIVERSAL_FORWARD_LIT_PASS_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "CustomLitForwardPassHelpers.hlsl"



sampler2D _NormTex;



half4 LitPassFragment(VertexOutput input) : SV_Target
{


    // Random included stuff
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
    #if defined(_PARALLAXMAP)
    #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
        half3 viewDirTS = input.viewDirTS;
    #else
        half3 viewDirWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
        half3 viewDirTS = GetViewDirectionTangentSpace(input.tangentWS, input.normalWS, viewDirWS);
    #endif
        ApplyPerPixelDisplacement(viewDirTS, input.uv);
    #endif

    // Create SurfaceData
    SurfaceData SurfaceData;
    InitializeStandardLitSurfaceData(input.uv, SurfaceData);

    // Create InputData
    InputData InputData;
    InitializeInputData(input, SurfaceData.normalTS, InputData);
    SETUP_DEBUG_TEXTURE_DATA(InputData, input.uv, _BaseMap);


    // Find Albedo from texture and set it
    float3 Albedo = tex2D(_MainTex, input.uv);
    SurfaceData.albedo = Albedo;
    InputData.bakedGI = float3(_BakedGI, _BakedGI, _BakedGI);

    // Find final color
    half4 Color = UniversalFragmentPBR(InputData, SurfaceData);
    Color.rgb = MixFog(Color.rgb, InputData.fogCoord);
    Color.a = OutputAlpha(Color.a, _Surface);

    return Color;
}

#endif
