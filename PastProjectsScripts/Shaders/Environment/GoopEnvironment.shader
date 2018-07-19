﻿Shader "Custom/GoopEnviroment" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	_DecalTex("Decal (RGB)", 2D) = "white" {}
	_DecalTex2("Decal(RGB)", 2D) = "white" {}
			_AlphaFallOff("_AlphaFallOff",  Range(0.1,100)) = 1
			_DecalIntensity("_DecalIntensity", Range(0.1,10)) = 1

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
#pragma surface surf Standard addshadow 
#include "CalculateDecalCol.cginc"

			sampler2D _DecalTex;
			sampler2D _DecalTex2;
			sampler2D _MainTex;
		float _AlphaFallOff;

		int _NumDecals0;
		float4 _DecalWorldPositions0[50];
		int _NumDecals1;
		float4 _DecalWorldPositions1[50];

		float4 _Color;
		float _DecalIntensity;

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

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			CustomDecalInput decalInput;
			decalInput.worldPos = IN.worldPos;
			decalInput.worldNormal = IN.worldNormal;
			decalInput.screenPos = IN.screenPos;

			DecalParams decalParams;
			decalParams.alphaFallOff = _AlphaFallOff;
			decalParams.decalIntensity = _DecalIntensity;
			decalParams.shineDistance = 80;
			decalParams.shineFactor = 2;
			decalParams.tintColor = float4(1,1,1,1);
			decalParams.maxShine = 0.6;

			half4 c = GetDecalColor(_DecalTex, _NumDecals0, _DecalWorldPositions0, decalInput, decalParams, o.Smoothness);


            decalParams.alphaFallOff = 4;
			half4 c2 = GetDecalColor(_DecalTex2, _NumDecals1, _DecalWorldPositions1, decalInput, decalParams, o.Smoothness);
    
            half4 mix = c2 * c2.a + c * (1 - c2.a) + c * c.a + c2 * (1 - c.a);


            o.Smoothness = _Glossiness;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
            o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
            o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

			o.Albedo = mix * mix.a + tex2D(_MainTex, IN.uv_MainTex ) * _Color * (1- mix.a);
			o.Alpha = 1;
		}
		ENDCG
	}
//	FallBack "Diffuse"
}