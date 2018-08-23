// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/BurnAway" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}


	_MetallicGlossMap("_MetallicTex", 2D) = "white" {}
	_BumpMap("NormalMap", 2D) = "bump" {}
	_OcclusionMap("Occlusion", 2D) = "black" {}
	_OcclusionStrength("Occlusion_Strength", Range(0.0, 1.0)) = 1.0
		_BumpScale("Bump_Scale", Float) = 1.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("_Metallic", Range(0,1)) = 1.0


		_CutOffMap("CutOffMap", 2D) = "white" {}
		_CutOffAmount("Cut Off Amount" ,Range(-0.5, 1.5)) = 0.5

		_GradientScale("Gradient Scale", Range(0.0,1.0)) = 0.2
            _EmissiveScale("_EmissiveScale", Range(0.0,10.0)) = 0.2
		_CutOffGradient("Cut Off Gradient", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "RenderType"="Opaque" }
		LOD 200
		Cull Off
		CGPROGRAM

#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _CutOffMap;
		sampler2D _CutOffGradient;
		float _CutOffAmount;
		float _GradientScale;
        float _EmissiveScale;

		struct Input {
			float2 uv_MainTex;
			float2 uv_CutOffMap;
			float2 uv_CutOffGradient;
			float2 uv_BumpMap;
			float2 uv_OcclusionMap;
			float2 uv_MetallicGlossMap;
		};

		fixed4 _Color;

		float _Metallic;
		float _BumpScale;
		float _OcclusionStrength;
		float _Glossiness;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half test = tex2D(_CutOffMap, IN.uv_CutOffMap).rgb - _CutOffAmount;
			clip(test);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;

			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
			o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

			
			if (test < _GradientScale)
			{
				o.Emission = tex2D(_CutOffGradient, float2(test *(1 / _GradientScale), 0));
				o.Albedo *= o.Emission;
                o.Emission *= _EmissiveScale;
			}



			o.Alpha = 1-_CutOffAmount;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
