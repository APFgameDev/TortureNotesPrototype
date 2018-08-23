// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/2SideRender" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}


		_Smoothness("Smoothness", Range(0,1)) = 0.5
		_LightBoost("LightBoost", Range(0,10)) = 1
	


			_BumpScale("Scale", Float) = 1.0
			_Glossiness("Smoothness", Range(0,1)) = 0.5
			_Metallic("_Metallic", Range(0,1)) = 1.0
			_OcclusionMap("Occlusion", 2D) = "white" {}
		_MetallicGlossMap("_MetallicTex", 2D) = "white" {}
		_BumpMap("NormalMap", 2D) = "bump" {}
		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0

		_CutOffTex("_CutOffTex", 2D) = "white" {}
		_CutOffAmount("_CutOffAmount", Range(0,1)) = 1
			 _Mode("__mode", Float) = 0.000000
			_SrcBlend("__src", Float) = 1.000000
			 _DstBlend("__dst", Float) = 0.000000
			  _ZWrite("__zw", Float) = 1.000000
	}
	SubShader {
		LOD 200
		Tags{ "RenderType" = "TransparentCutout" "Queue" = "AlphaTest"}
			Cull Back
			ZWrite[_ZWrite]
			Blend[_SrcBlend][_DstBlend]
		CGPROGRAM


#pragma surface surf Standard 

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		sampler2D _MainTex;
		sampler2D _CutOffTex;
		float _LightBoost;

		struct Input {
			float2 uv_MainTex;
			float2 uv_CutOffTex;
			float3 viewDir;
		};

		fixed4 _Color;
		fixed _CutOffAmount;
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

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * _LightBoost;
			o.Albedo = c.rgb;
		
			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;

			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
			o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

			fixed a = tex2D(_CutOffTex, IN.uv_CutOffTex);
			o.Alpha = _Color.a;

			clip(a - _CutOffAmount);
				o.Normal =  o.Normal;
		}
		ENDCG
			Cull Front
			CGPROGRAM

#pragma surface surf Standard


			// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0
#pragma vertex vert
			sampler2D _MainTex;
		sampler2D _CutOffTex;
		fixed _CutOffAmount;
		float _LightBoost;
		struct Input {
			float2 uv_MainTex;

			float2 uv_CutOffTex;
			float3 viewDir;
		};

		fixed4 _Color;
		float _Metallic;
		float _BumpScale;
		float _OcclusionStrength;
		float _Glossiness;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;

		void vert(inout appdata_full v) {
			v.normal *= -1;
		}

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * _LightBoost;
			o.Albedo = c.rgb;
		
			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;

			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
			o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

			fixed a = tex2D(_CutOffTex, IN.uv_CutOffTex);
			o.Alpha = _Color.a;

			clip(a - _CutOffAmount);
				o.Normal =  o.Normal;

		
		}
		ENDCG
	}
	FallBack "Diffuse"
}
