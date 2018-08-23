// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/FadeOutKeepAlpha" {
	Properties {
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MetallicGlossMap("_MetallicTex", 2D) = "white" {}
		_BumpMap("NormalMap", 2D) = "bump" {}
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_BumpScale("Scale", Float) = 1.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("_Metallic", Range(0,1)) = 1.0
			_Alpha("_Alpha", Range(0,1)) = 1.0
	}
	SubShader {
			Tags{"Queue" = "Transparent"  "RenderType" = "Transparent" }
		LOD 200
			Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows keepalpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;
		float _Metallic;
		float4 _Color;
		float _BumpScale;
		float _OcclusionStrength;
		float _Glossiness;
		float _Alpha;

		struct Input {
			float2 uv_MainTex;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a * _Alpha;
			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
			o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
