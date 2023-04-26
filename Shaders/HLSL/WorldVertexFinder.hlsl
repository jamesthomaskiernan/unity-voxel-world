#ifndef SHADER_INCLUDED
#define SHADER_INCLUDED

// Include helper functions from URP
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


sampler2D _MainTex;
float _Alpha;
float _BakedGI;



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
    


    // Custom interpolators
    float3 ObjPos : TEXCOORD10;
    int ObjType : TEXCOORD11;
};

struct Tri
{
    float3 vertA;
    float3 vertB;
    float3 vertC;
    float2 uvA;
    float2 uvB;
    float2 uvC;
    float3 normal;
    int ObjType;
    float3 ObjPos;
};

StructuredBuffer<Tri> Triangles;

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
    }

    output.positionCS = mul(UNITY_MATRIX_VP, float4(p, 1));
    output.normalWS = t.normal;
    output.positionWS = p;
    output.uv = uv;
    output.ObjPos = t.ObjPos;
    output.ObjType = t.ObjType;
    return output;
}

float4 frag (VertexOutput input) : SV_Target 
{
    return 0;
}

#endif

