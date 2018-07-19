Shader "Custom/IndicateDamage"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

	_MetallicGlossMap("_MetallicTex", 2D) = "white" {}
		_BumpMap("NormalMap", 2D) = "bump" {}
		_OcclusionMap("Occlusion", 2D) = "white" {}
		_OcclusionStrength("Occlusion_Strength", Range(0.0, 1.0)) = 1.0
		_BumpScale("BumpScale", Float) = 1.0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("_Metallic", Range(0,1)) = 1.0

        _RimColor("Rim Color", Color) = (1,0,0,1)
        _RimPower("Rim Power", Range(0.1,6.0)) = 3
        _FlashSpeed("FlashSpeed", Range(1, 10)) = 5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200


		
		CGPROGRAM

        #pragma surface surf Standard addshadow 

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float4 _Color;
		struct Input {
			float2 uv_MainTex;
            float3 viewDir;
		};

        float4 _RimColor;	
        float _RimPower; 
        float _FlashSpeed;

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
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

  
		void surf (Input IN, inout SurfaceOutputStandard o) 
        {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;

			o.Smoothness = _Glossiness;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;     
			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
			o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

			half rim = 1.0 - dot(normalize(IN.viewDir), o.Normal);
			float sinTime = abs(sin(_Time.y * _FlashSpeed));
			o.Emission = _RimColor.rgb * pow(rim, _RimPower / sinTime) * _RimColor.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
