Shader "Custom/EnergyRibbon" {
    Properties{
        _Tint("_Tint",COLOR) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    _MainTexSmoothness("MainTex Smoothness", Range(0,1)) = 0.94
        _BurnTexOpacity("_BurnTexOpacity", 2D) = "white" {}
    _BurnMagma("_BurnMagma", 2D) = "white" {}
    _MagmaNoise("_MagmaNoise", 2D) = "white" {}
    _LavaSpeedA("_LavaSpeedA",float) = 1
        _SmokeSpeed("_SmokeSpeed",float) = 1
        _SmokeSecondPassSpeed("_SmokeSecondPassSpeed", float) = 1
        _MagmaNoiseUVScale("_MagmaNoiseUVScale",Range(0,10)) = 0

        _EmissionAmount("_EmissionAmount",float) = 1
        _MetallicGlossMap("_MetallicTex", 2D) = "white" {}
    _BumpMap("NormalMap", 2D) = "bump" {}
    _OcclusionMap("Occlusion", 2D) = "white" {}
    _OcclusionStrength("Occlusion_Strength", Range(0.0, 1.0)) = 1.0
        _BumpScale("BumpScale", Float) = 1.0
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("_Metallic", Range(0,1)) = 1.0
    }
        SubShader{
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Lambert alpha
        

        // Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

        sampler2D _MainTex;
    sampler2D _BurnTexOpacity;
    sampler2D _BurnMagma;
    sampler2D _MagmaNoise;
    float _MagmaNoiseUVScale;
    float _LavaSpeedA;
    float _SmokeSpeed;
    float _CrackAmount;
    float4 _Tint;
    float _SmokeSecondPassSpeed;

    float _EmissionAmount;

    float _MagmaNoiseSecondPassScaleAdd;

    float _Metallic;
    float _BumpScale;
    float _OcclusionStrength;
    float _Glossiness;

    sampler2D _MetallicGlossMap;
    sampler2D _BumpMap;
    sampler2D _OcclusionMap;

    struct Input {
        float2 uv_MainTex;
        float2 uv_BurnMagma;
        float2 uv_BurnTexOpacity;
        float2 uv_MagmaNoise;
        float3 worldPos;
        float3 worldNormal;
        float4 screenPos;
        INTERNAL_DATA
    };
    // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
    // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
    // #pragma instancing_options assumeuniformscaling
    UNITY_INSTANCING_CBUFFER_START(Props)
        // put more per-instance properties here
        UNITY_INSTANCING_CBUFFER_END

        void surf(Input IN, inout SurfaceOutput o) {


        fixed4 texCol;

        half smoothness = 0.0;
        half metallic = 0.0;
        o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;

        float lerpWeight = tex2D(_BurnTexOpacity, IN.uv_BurnTexOpacity);

        half2 magUV = IN.uv_MagmaNoise;
        magUV.x += 1 + _Time.x * _LavaSpeedA;

        half2 mainUV = IN.uv_MainTex;
        mainUV.x += _Time.x * _SmokeSpeed;
        texCol = tex2D(_MainTex, mainUV);
        float emissionAmount = tex2D(_MagmaNoise, magUV);
        emissionAmount *= _EmissionAmount;
        o.Emission = tex2D(_BurnMagma, IN.uv_BurnMagma) * (1 - lerpWeight) * emissionAmount * texCol.a* _Tint.a;

        mainUV.x += _Time.x * _SmokeSecondPassSpeed;
        mainUV.y *= -1;
        texCol += tex2D(_MainTex, mainUV );
        texCol *= 0.5;
        o.Albedo = texCol * _Tint;
        o.Alpha = texCol.a* _Tint.a;

    }
    ENDCG
    }
        FallBack "Diffuse"
}
