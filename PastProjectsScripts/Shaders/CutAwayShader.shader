Shader "Custom/CutAwayShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	_MetallicGlossMap("_MetallicTex", 2D) = "white" {}
	_CutOffTex("_CutOffTex", 2D) = "white" {}
		_OcclusionMap("Occlusion", 2D) = "white" {}
	_BumpMap("Bumpmap", 2D) = "bump" {}

	_ParallaxMap("Height Map", 2D) = "white" {}
	_Parallax("Height Amount", Range(-1,1)) = 0.5


	_CutOffAmount("_CutOffAmount", Range(0,1)) = 1
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

			_BumpScale("Scale", Float) = 1.0
			_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard addshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _CutOffTex;
		sampler2D _BumpMap;
		sampler2D _MetallicGlossMap;
		sampler2D _OcclusionMap;
		sampler2D _ParallaxMap;

		fixed _CutOffAmount;
		struct Input {
			float2 uv_MainTex;
			float2 uv_CutOffTex;
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float _BumpScale;
		float _OcclusionStrength;
		float _Parallax;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

			o.Normal += tex2D(_ParallaxMap, IN.uv_MainTex) * _Parallax;
			o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);
			fixed a = tex2D(_CutOffTex, IN.uv_CutOffTex);
			clip(a - _CutOffAmount);
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
