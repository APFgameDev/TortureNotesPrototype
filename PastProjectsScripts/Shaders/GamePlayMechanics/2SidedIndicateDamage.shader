Shader "Custom/2SidedIndicateDamage"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}

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

		_CutOffTex("_CutOffTex", 2D) = "white" {}
	_CutOffAmount("_CutOffAmount", Range(0,1)) = 1
		_Mode("__mode", Float) = 0.000000
		_SrcBlend("__src", Float) = 1.000000
		_DstBlend("__dst", Float) = 0.000000
		_ZWrite("__zw", Float) = 1.000000
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 200




		Cull Back
		ZWrite[_ZWrite]
		Blend[_SrcBlend][_DstBlend]
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard keepalpha 

	

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0

		sampler2D _MainTex;
	sampler2D _CutOffTex;

	struct Input {
		float2 uv_MainTex;
		float2 uv_CutOffTex;
		float3 viewDir;
	};

    		float _Metallic;
		float _BumpScale;
		float _OcclusionStrength;
		float _Glossiness;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;

	float4 _RimColor;
	float _RimPower;
	float4 _Color;
	float _FlashSpeed;
	float _CutOffAmount;
	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_CBUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END


		void surf(Input IN, inout SurfaceOutputStandard o)
	{
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;


		fixed a = tex2D(_CutOffTex, IN.uv_CutOffTex);
		o.Alpha = _Color.a;

        o.Smoothness = _Glossiness;
        o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
        o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
        o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

		clip(a - _CutOffAmount);
		o.Normal = o.Normal;

        half rim = 1.0 - dot(normalize(IN.viewDir), o.Normal);
        float sinTime = abs(sin(_Time.y * _FlashSpeed));
        o.Emission = _RimColor.rgb * pow(rim, _RimPower / sinTime) * _RimColor.a;

	}
	ENDCG


		Cull Front
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard keepalpha 

		void vert(inout appdata_full v) {
		v.normal *= -1;
	}

		// Use shader model 3.0 target, to get nicer looking lighting
#pragma target 3.0
#pragma vertex vert
		sampler2D _MainTex;
	sampler2D _CutOffTex;
	float _CutOffAmount;

	struct Input {
		float2 uv_MainTex;
		float2 uv_CutOffTex;
		float3 viewDir;
	};

    float _Metallic;
    float _BumpScale;
    float _OcclusionStrength;
    float _Glossiness;
    sampler2D _MetallicGlossMap;
    sampler2D _BumpMap;
    sampler2D _OcclusionMap;

	float4 _RimColor;
	float _RimPower;
	float4 _Color;
	float _FlashSpeed;
	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_CBUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END


		void surf(Input IN, inout SurfaceOutputStandard o)
	{
		// Albedo comes from a texture tinted by color
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;


        fixed a = tex2D(_CutOffTex, IN.uv_CutOffTex);
        o.Alpha = _Color.a;

        o.Smoothness = _Glossiness;
        o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
        o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
        o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);

		half rim = 1.0 - dot(normalize(IN.viewDir), o.Normal);
		float sinTime = abs(sin(_Time.y * _FlashSpeed));
		o.Emission = _RimColor.rgb * pow(rim, _RimPower / sinTime) * _RimColor.a;



		clip(a - _CutOffAmount);
		o.Normal = o.Normal;

	}
	ENDCG
	}
		FallBack "Diffuse"
}