Shader "Custom/EnviromentBurning" {
	Properties{
	_Color("Color", Color) = (1,1,1,1)
	_MainTex("Albedo (RGB)", 2D) = "white" {}
	_MainTexSmoothness("MainTex Smoothness", Range(0,1)) = 0.94
	_BurnTex("_BurnTex", 2D) = "white" {}
	_BurnTexOpacity("_BurnTexOpacity", 2D) = "white" {}
	_BurnMagma("_BurnMagma", 2D) = "white" {}
	_MagmaNoise("_MagmaNoise", 2D) = "white" {}

	_DecalTex("Decal (RGB)", 2D) = "white" {}

	_AlphaFallOff("_AlphaFallOff",  Range(0.1,100)) = 1
	_DecalIntensity("_DecalIntensity", Range(0.1,10)) = 1


	_MagmaNoiseUVScale("_MagmaNoiseUVScale",Range(0,10)) = 0
	_MagmaNoiseSecondPassScaleAdd("_MagmaNoiseSecondPassScaleAdd",Range(0,10)) = 5


	_MagmaPassAlphaAdjuster("_MagmaPassAlphaAdjuster",Range(0,1)) = 0.1
	_MagmaPassBrightnessAdjuster("_MagmaPassBrightnessAdjuster",Range(0,2)) = 1

		_MinEmission("_EmissionAmount",float) = 0.5
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
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		ZTest On



		CGPROGRAM
#pragma surface surf Standard addshadow 
#pragma shader_feature _EMISSION
#include "CalculateDecalCol.cginc"

		sampler2D _DecalTex;
	sampler2D _BurnTex;
	sampler2D _MainTex;
	sampler2D _BurnTexOpacity;
	sampler2D _BurnMagma;
	sampler2D _MagmaNoise;
	float _AlphaFallOff;
	float _MagmaNoiseUVScale;
	float _MinEmission;

	float _EmissionAmount;
	int _NumDecals0;
	float4 _DecalWorldPositions0[50];
	float4 _Color;
	float _DecalIntensity;
	float _MainTexSmoothness;

	float _MagmaNoiseSecondPassScaleAdd;

	float _Metallic;
	float _BumpScale;
	float _OcclusionStrength;
	float _Glossiness;

	sampler2D _MetallicGlossMap;
	sampler2D _BumpMap;
	sampler2D _OcclusionMap;


	float4 _HoleLocations[50];

	float2 _HoleSizes[50];

	int _numHoles;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BurnTex;
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
		decalParams.shineDistance = 40;
		decalParams.shineFactor = 2;
		decalParams.tintColor = _Color;
		decalParams.maxShine = 0.6;

		half4 c = GetDecalColor(_DecalTex, _NumDecals0, _DecalWorldPositions0, decalInput, decalParams, o.Smoothness);

		fixed4 texCol;

		half smoothness = 0.0;
		half metallic = 0.0;

		o.Smoothness = _Glossiness;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
		o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
		o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

		float lerpWeight = tex2D(_BurnTexOpacity, IN.uv_BurnTex);
		float emissionAmount = tex2D(_MagmaNoise, IN.uv_BurnTex * _MagmaNoiseUVScale + _Time.x);
		emissionAmount = max(emissionAmount, tex2D(_MagmaNoise, IN.uv_BurnTex + _MagmaNoiseSecondPassScaleAdd * _MagmaNoiseUVScale - _Time.x));
		emissionAmount *= max(_MinEmission, _SinTime.x) * _EmissionAmount;
		o.Emission = tex2D(_BurnMagma, IN.uv_BurnTex) * (1 - lerpWeight) * emissionAmount;

		texCol = tex2D(_BurnTex, IN.uv_BurnTex);

		o.Albedo = c * c.a + texCol * (1 - c.a);
		o.Alpha = 1;
		
		for (int i = 0; i < _numHoles; i++)
		{
			float xDisplacement = abs(_HoleLocations[i].x - IN.worldPos.x) * (1 / _HoleSizes[i].x);
			float zDisplacement = abs(_HoleLocations[i].z - IN.worldPos.z) * (1 / _HoleSizes[i].y);

			if (xDisplacement * xDisplacement + zDisplacement * zDisplacement < 1)
				clip(-1);
		}
	}
	ENDCG

	}
		FallBack "Diffuse"
}
