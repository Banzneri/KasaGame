Shader "Custom/Water" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _BlendColor("Blend Color", Color) = (1, 1, 1, 1)
        _Softness("Softness", Range(0.01, 3.0)) = 1.0
        _FadeLimit("Fade Limit", Range(0.0, 1.0)) = 0.3
        _WaveSpeed("Wave Speed", Range(0.0, 10.0)) = 1.0
        _WaveAmp("Amplitude", Range(0.0, 5.0)) = 1.0
        _WaveOffset("Wave Offset", Range(0.0, 5.0)) = 1.0
    }
        SubShader{
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
 
        CGPROGRAM
#pragma surface surf Standard vertex:vert alpha:fade nolightmap
#pragma target 3.0
 
    struct Input {
        float2 uv_MainTex;
        float4 screenPos;
        float eyeDepth;
    };
 
    sampler2D_float _CameraDepthTexture;
    fixed4 _Color;
    fixed4 _BlendColor;
    float _FadeLimit;
    float _Softness;
    float _WaveSpeed;
    float _WaveAmp;
    float _WaveOffset;
 
    void vert(inout appdata_full v, out Input o) {
        UNITY_INITIALIZE_OUTPUT(Input, o);
        COMPUTE_EYEDEPTH(o.eyeDepth);
 
        float3 v0 = mul(unity_ObjectToWorld, v.vertex).xyz;
        float phase0 = 0.1 * sin((_Time.y * _WaveSpeed) + (v0.x * _WaveOffset) + (v0.z * _WaveOffset));
        float phase0_1 = 0.1 * cos((_Time.y * _WaveSpeed) - (v0.x * -_WaveOffset) - (v0.z * -_WaveOffset));
 
        v.vertex.y += (phase0 + phase0_1) * _WaveAmp;
        v.vertex.x -= (phase0_1 * 1.75) * _WaveAmp;
        v.vertex.z += (phase0_1 * 3.5) * _WaveAmp;
    }
 
    void surf(Input IN, inout SurfaceOutputStandard o) {
        o.Albedo = _Color.rgb;
        o.Alpha = 0.7;
        o.Metallic = 0;
        o.Smoothness = 0;
 
        // Get depth from the depth texture
        float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));
        // Convert depth texture to camera space
        float sceneZ = LinearEyeDepth(rawZ);
        // Get the length of z in camera space
        float partZ = IN.eyeDepth;
 
        float fade = 1.0;
 
        if (rawZ > 0.0)
            fade = saturate(_Softness * (sceneZ - partZ));
 
 
        if (fade < _FadeLimit)
            o.Albedo = _Color.rgb * fade + _BlendColor * (1 - fade);
 
    }
    ENDCG
    }
        FallBack "Diffuse"
}