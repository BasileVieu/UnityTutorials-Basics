Shader "Graph/Point Surface GPU"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        #pragma instancing_options assumeuniformscaling procedural:ConfigureProcedural
        #pragma editor_sync_compilation
        #pragma target 4.5

        #include "Point GPU.hlsl"

        struct Input
        {
            float3 worldPos;
        };

        float _Smoothness;

        void ConfigureSurface(Input _input, inout SurfaceOutputStandard _surface)
        {
            _surface.Albedo = saturate(_input.worldPos * 0.5 + 0.5);
            _surface.Smoothness = _Smoothness;
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}