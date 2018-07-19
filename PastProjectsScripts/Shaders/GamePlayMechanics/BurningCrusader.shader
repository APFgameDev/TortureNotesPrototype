Shader "Custom/BurningCrusader" {
	Properties {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    _MainTexSmoothness("MainTex Smoothness", Range(0,1)) = 0.94
    _BurnTexOpacity("_BurnTexOpacity", 2D) = "white" {}
    _BurnMagma("_BurnMagma", 2D) = "white" {}
    _MagmaNoise("_MagmaNoise", 2D) = "white" {}
    _LavaSpeedA("_LavaSpeedA",float) = 1
        _LavaSpeedB("_LavaSpeedB",float) = 1
        _CrackAmount("_CrackAmount",Range(0,1)) = 1

        _MagmaNoiseUVScale("_MagmaNoiseUVScale",Range(0,10)) = 0
        _MagmaNoiseSecondPassScaleAdd("_MagmaNoiseSecondPassScaleAdd",Range(0,10)) = 5

        _EmissionAmount("_EmissionAmount",float) = 1
        _MetallicGlossMap("_MetallicTex", 2D) = "white" {}
    _BumpMap("NormalMap", 2D) = "bump" {}
    _OcclusionMap("Occlusion", 2D) = "white" {}
    _OcclusionStrength("Occlusion_Strength", Range(0.0, 1.0)) = 1.0
        _BumpScale("BumpScale", Float) = 1.0
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("_Metallic", Range(0,1)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BurnTexOpacity;
        sampler2D _BurnMagma;
        sampler2D _MagmaNoise;
        float _MagmaNoiseUVScale;
        float _LavaSpeedA;
        float _LavaSpeedB;
        float _CrackAmount;

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

		void surf (Input IN, inout SurfaceOutputStandard o) {


            fixed4 texCol;

            half smoothness = 0.0;
            half metallic = 0.0;

            o.Smoothness = _Glossiness;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
            o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
            o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

            float lerpWeight = tex2D(_BurnTexOpacity, IN.uv_MainTex);
            float emissionAmount = tex2D(_MagmaNoise, IN.uv_MainTex * _MagmaNoiseUVScale + _Time.x * _LavaSpeedA);
            emissionAmount = max(emissionAmount, tex2D(_MagmaNoise, IN.uv_MainTex + _MagmaNoiseSecondPassScaleAdd * _MagmaNoiseUVScale - _Time.x * _LavaSpeedB));
            emissionAmount *= _EmissionAmount;
            o.Emission = tex2D(_BurnMagma, IN.uv_MainTex) * (1 - lerpWeight) * emissionAmount;

            half4 crackCol = 1- tex2D(_BurnTexOpacity, IN.uv_MainTex);
            crackCol *= _CrackAmount;
            crackCol = 1 - crackCol;
            texCol = tex2D(_MainTex, IN.uv_MainTex) * crackCol;

            o.Albedo = texCol;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
