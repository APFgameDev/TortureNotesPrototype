

Shader "Custom/EnviromentBurnAble" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MainTexSmoothness("MainTex Smoothness", Range(0,1)) = 0.94
		_MainTexMetallic("MainTex Metallic", Range(0,1)) = 0.5
		_BurnTex("_BurnTex", 2D) = "white" {}
		_BurnTexOpacity("_BurnTexOpacity", 2D) = "white" {}
		_BurnMagma("_BurnMagma", 2D) = "white" {}
		_MagmaNoise("_MagmaNoise", 2D) = "white" {}

		_DecalTex("Decal (RGB)", 2D) = "white" {}

		_AlphaFallOff("_AlphaFallOff",  Range(0.1,100)) = 1
		_DecalIntensity("_DecalIntensity", Range(0.1,10)) = 1
		_BurnOriginWorldPos("_BurnOriginWorldPos",Vector) = (1,0,0,0)
		_BurnDistance("BurnDistance",float) = 0

			_MagmaNoiseUVScale("_MagmaNoiseUVScale",Range(0,10)) = 0
			_MagmaNoiseSecondPassScaleAdd("_MagmaNoiseSecondPassScaleAdd",Range(0,10)) = 5

			_NoiseTex("_NoiseTex", 2D) = "white" {}
			_NoiseScale("_NoiseScale", Range(0,10)) = 1
			_NoiseSpeed("_NoiseSpeed", Range(0,10)) = 0.1
			_NoiseUV_YScale("_NoiseUV_YScale", Range(0,1)) = 0.001
				_NoiseUV_XScale("_NoiseUV_XScale", Range(0,1)) = 0.1

				_MagmaPassAlphaAdjuster("_MagmaPassAlphaAdjuster",Range(0,1)) = 0.1
				_MagmaPassBrightnessAdjuster("_MagmaPassBrightnessAdjuster",Range(0,2)) = 1
			
				_MinEmission("_MinEmission",float) = 0.5
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
		float _EmissionAmount;
		float _MinEmission;

		int _NumDecals0;
		float4 _DecalWorldPositions0[50];
		float4 _Color;
		float _DecalIntensity;
		float _MainTexMetallic;
		float _MainTexSmoothness;

		sampler2D _NoiseTex;
		float _NoiseUV_XScale;
		float _NoiseUV_YScale;
		float _NoiseScale;
		float _NoiseSpeed;

		float _BurnDistance;
		float4 _BurnOriginWorldPos;
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

			float3 displacement = (IN.worldPos - _BurnOriginWorldPos);
			float noiseUVX = atan2(displacement.x, displacement.z) * _NoiseUV_XScale + _Time.z * _NoiseSpeed;
			float dist = distance(IN.worldPos, _BurnOriginWorldPos);
			dist -= tex2D(_NoiseTex, float2(noiseUVX, _BurnDistance *  _NoiseUV_YScale)) * _NoiseScale;

			fixed4 texCol;

			half smoothness = 0.0;
			half metallic = 0.0;

			      o.Smoothness = _Glossiness;
        o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
        o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
        o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);


			if (dist < _BurnDistance)
			{
				float lerpWeight = tex2D(_BurnTexOpacity, IN.uv_BurnTex);


				float emissionAmount = tex2D(_MagmaNoise, IN.uv_BurnTex * _MagmaNoiseUVScale + _Time.x);

				emissionAmount = max(emissionAmount, tex2D(_MagmaNoise, IN.uv_BurnTex + _MagmaNoiseSecondPassScaleAdd * _MagmaNoiseUVScale - _Time.x));

				emissionAmount *= max(_MinEmission, _SinTime.x) * _EmissionAmount;
				o.Emission = tex2D(_BurnMagma, IN.uv_BurnTex) * (1 - lerpWeight) * emissionAmount;

				texCol = tex2D(_BurnTex, IN.uv_BurnTex);
			}
			else
			{
				texCol = tex2D(_MainTex, IN.uv_MainTex);

				smoothness = _MainTexSmoothness;
				metallic = _MainTexMetallic;
			}

			o.Albedo = c * c.a + texCol * (1- c.a);
			//o.Smoothness = smoothness;
			//o.Metallic = metallic;
			o.Alpha = 1;
		}
		ENDCG
		/*	Cull Off
			Blend One One
			ZTest LEqual
			ZWrite Off
			Tags{ "Queue" = "Transparent-1" "RenderType" = "Transparent" }




			CGPROGRAM
#pragma surface surf Standard alpha:fade
#pragma	 vertex  vert
#define BumpUpAmount 0.0125 * 4
#define SlideAmount  0.025
#define Brightness 10
#include "MagmaPass.cginc"
			ENDCG

			CGPROGRAM
#pragma surface surf Standard alpha:fade
#pragma	 vertex  vert
#define BumpUpAmount  0.0125 * 8
#define SlideAmount 0.025
#define Brightness 10
#include "MagmaPass.cginc"
			ENDCG


			CGPROGRAM
#pragma surface surf Standard alpha:fade
#pragma	 vertex  vert
#define BumpUpAmount 0.0125 * 12
#define SlideAmount 0.025
#define Brightness 10
#include "MagmaPass.cginc"
			ENDCG


			CGPROGRAM
#pragma surface surf Standard alpha:fade
#pragma	 vertex  vert
#define BumpUpAmount 0.0125 * 16
#define SlideAmount 0.025
#define Brightness 10
#include "MagmaPass.cginc"
			ENDCG*/
	}
	FallBack "Diffuse"
}
