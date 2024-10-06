Shader "Graph/Point Surface"
{
    Properties
    {
        _Smoothness ("Smoothness", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        CGPROGRAM
        #pragma surface ConfigureSurface Standard fullforwardshadows
        #pragma target 3.0

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