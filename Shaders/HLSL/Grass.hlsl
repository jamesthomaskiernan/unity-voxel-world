#ifndef SHADER_INCLUDED
#define SHADER_INCLUDED


#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

// float4 _BaseColor;
float4 _TipColor;
float _BakedGI;
float _Metal;
float _Smooth;
float _NoiseScale;
float _Test2;
float _WindSpeed;
float _Timer;
float _WindIntensity;
float4 _WindDirection;
float4 _CloudSpotColor;
float _CloudSpeed;
float _CloudScale;


TEXTURE2D(_Noise); SAMPLER(sampler_Noise);



struct VertexOutput
{
    // 0 - UV
    float2 uv                           : TEXCOORD0;

    // 1 - PositionWS
    #if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
        float3 positionWS               : TEXCOORD1;
    #endif

    // 2 - NormalWS
    half3 normalWS                      : TEXCOORD2;

    // 3 - TangentWS (xyz: tangent, w: sign)
    #if defined(REQUIRES_WORLD_SPACE_TANGENT_INTERPOLATOR)
        half4 tangentWS                 : TEXCOORD3;    
    #endif
    
    // 4 - ViwDirWS
    float3 viewDirWS                    : TEXCOORD4;

    // 5 - Fog
    #ifdef _ADDITIONAL_LIGHTS_VERTEX
        // x: fogFactor, yzw: vertex light
        half4 fogFactorAndVertexLight   : TEXCOORD5;
    #else
        half  fogFactor                 : TEXCOORD5;
    #endif

    // 6 - ShadowCoord
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
        float4 shadowCoord              : TEXCOORD6;
    #endif

    // ViewDirTS
    #if defined(REQUIRES_TANGENT_SPACE_VIEW_DIR_INTERPOLATOR)
        half3 viewDirTS                : TEXCOORD7;
    #endif

    // DynamicLightmapUV
    DECLARE_LIGHTMAP_OR_SH(staticLightmapUV, vertexSH, 8);
    #ifdef DYNAMICLIGHTMAP_ON
        float2  dynamicLightmapUV : TEXCOORD9; // Dynamic lightmap UVs
    #endif

    // PositionCS
    float4 positionCS               : SV_POSITION;
};


// Include helper functions
#include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
#include "CustomLitForwardPassHelpers.hlsl"



struct Tri
{
    float3 vertA;
    float3 vertB;
    float3 vertC;
    float2 uvA;
    float2 uvB;
    float2 uvC;
    float3 normal;
};

StructuredBuffer<Tri> Triangles;


#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

VertexOutput vert (uint id : SV_VertexID)
{
    VertexOutput output = (VertexOutput)0;

    float3 p;
    float2 uv;

    int TriangleIndex = floor(id / 3.0);
    int VertIndex = id % 3;
    Tri t = Triangles[TriangleIndex];

    if (VertIndex == 0)
    {
        p = t.vertA;
        uv = t.uvA;
    }

    if (VertIndex == 1)
    {
        p = t.vertB;
        uv = t.uvB;
    }
    

    


    if (VertIndex == 2)
    {
        p = t.vertC;
        uv = t.uvC;


        


        float windNoise = SAMPLE_TEXTURE2D_LOD(_Noise, sampler_Noise, float3(p.x * _NoiseScale + _Timer * _WindSpeed, p.z * _NoiseScale + _Timer * _WindSpeed, 0), 0).r;
        

        if (windNoise > _Test2)
        {
            p += windNoise * t.normal * _WindIntensity / 4.0;


            p.x += windNoise * _WindIntensity * _WindDirection.x;
            p.z += windNoise * _WindIntensity * _WindDirection.y;

        }


    }

    output.positionCS = mul(UNITY_MATRIX_VP, float4(p, 1));
    output.normalWS = float3(0,1,0);
    output.positionWS = p;
    output.uv = uv;

    return output;
}





float4 frag (VertexOutput input) : SV_Target
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
    

    // Find Albedo from texture and set it
    // float3 Albedo = (float3)_BaseColor;
    
    
    float windNoise = SAMPLE_TEXTURE2D_LOD(_Noise, sampler_Noise, float3(input.positionWS.x * _CloudScale + _Timer * _CloudSpeed, input.positionWS.z * _CloudScale + _Timer * _CloudSpeed, 0), 0).r;
    
    

    float3 Albedo = lerp(_BaseColor.rgb, _TipColor.rgb, input.uv.y);
    // float3 Albedo = lerp(_BaseColor.rgb, lerp(_CloudSpotColor.rgb, _TipColor.rgb, windNoise), input.uv.y);










    // float3 Albedo = float3(0,0,0);
    // Albedo.rgb += input.uv.y;
    SurfaceData.albedo = Albedo;
    SurfaceData.smoothness = _Smooth;
    SurfaceData.metallic = _Metal;







    InputData.bakedGI = float3(_BakedGI, _BakedGI, _BakedGI);

    // Find final color
    half4 Color = UniversalFragmentPBR(InputData, SurfaceData);
    Color.rgb = MixFog(Color.rgb, InputData.fogCoord);
    Color.a = OutputAlpha(Color.a, _Surface);
    






    return Color;

}

#endif

