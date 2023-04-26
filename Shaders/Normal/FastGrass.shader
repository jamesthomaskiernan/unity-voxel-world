Shader "Shaders/FastGrass" 
{
    Properties
    {
        _BakedGI("BakedGI", Range(0.0, 1.0)) = 0.75
        _Metal("Metallic", Range(0.0, 1.0)) = 0.75
        _Smooth("Smooth", Range(0.0, 1.0)) = 0.75
        

        _Timer("Time", float) = 0
        


        _GrassTex("Grass Tex", 2D) = "white" {}
        _GrassTexScale("Grass Tex Scale", float) = 1
        _GrassTexDimensions("Grass Tex Dimensions", float) = 256.0

        _Test1("Test1", float) = 0.75
        _Test2("Test2", Range(0.0, 1.0)) = 0.75

        _WindTex("Wind Tex", 2D) = "white" {}
        _WindScale("Wind Scale", Range(0.0, 1.0)) = 0.75
        _WindSpeed("Wind Speed", Range(0.0, 1.0)) = 0.75
        _WindIntensity("Wind Intensity", Range(0.0, 1.0)) = 0.75
        _WindDirection("Wind Direction", Vector) = (0, 0, 0, 0)
        
        // _CloudScale("Cloud Scale", Range(0.0, .185)) = .15
        // _CloudSpeed("Cloud Speed", Range(0.0, 1.0)) = 0.75
        // _CloudSpotColor("Cloud Spot Color", Color) = (0, 1, 0, 1) // Color of the highest layers
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 screenPos : TEXCOORD1;
            };

            sampler2D _GrassTex;
            float4 _GrassTex_ST;
            float _GrassTexScale;
            
            float _GrassTexDimensions;

            sampler2D _WindTex;
            float _WindScale;
            float _WindSpeed;
            float _WindIntensity;
            float _Timer;

            float _Test1;



            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _GrassTex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            
                float windNoise = tex2D(_WindTex, float2(i.uv.x * _WindScale + _Timer * _WindSpeed, i.uv.y * _WindScale + _Timer * _WindSpeed)).r;
            
                float2 uv = i.screenPos * _ScreenParams/_GrassTexDimensions;


                uv.x += windNoise * _WindIntensity * _ScreenParams/_GrassTexDimensions;


                return tex2D(_GrassTex, uv);


                




            }
            ENDCG
        }
    }
}
