Shader "Custom/FX/HeatDistortEffect2Sided
"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black" {}
	_NormalMap("Normalmap", 2D) = "bump" {}
	_DistortAmount("Distorion Amount", Range(0,1000)) = 20
		_TintColor("Tint Color", Color) = (1,1,1,1)
		_TexAlphaAmount("_TexAlphaAmount", Range(0,1)) = 1
		_DistortAlphaBoost("_DistortAlphaBoost", Range(0.1,50)) = 1
		_TexBrightnessAdjust("_Texture Birghtness Adjuster", Range(0.1,10)) = 2

	}
		SubShader
	{
		Tags{ "Queue" = "AlphaTest" "RenderType" = "Transparent" }
		ZTest LEqual
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"



		float4 _MainTex_ST;
	sampler2D _MainTex;
	float4 _TintColor;
	float _TexAlphaAmount;
	float _TexBrightnessAdjust;


	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : COLOR;
	};

	struct v2f
	{
		float4 vertex : SV_POSITION;
		float2 uvmain : TEXCOORD2;
		fixed4 color : COLOR;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uvmain = TRANSFORM_TEX(v.uv, _MainTex);
		o.color = v.color;
		return o;
	}



	half4  frag(v2f i) : COLOR
	{
		half4  col = tex2D(_MainTex, i.uvmain) * _TintColor * i.color;
		col.rgb *= _TexBrightnessAdjust;
		col.a *= _TexAlphaAmount;
		return col;
	}
		ENDCG

	}
		Tags{ "Queue" = "Transparent-1" "RenderType" = "Transparent" }
		// Grab the screen behind the object into _GrabTexture
		GrabPass
	{
		"_GrabTexture"
	}

		Pass
	{
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"



		float _DistortAmount;
	float4 _NormalMap_ST;


	sampler2D _NormalMap;
	sampler2D _GrabTexture;
	float4 _GrabTexture_TexelSize;
	float _DistortAlphaBoost;
	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : COLOR;
	};

	struct v2f
	{
		float4 uvgrab : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float2 uvbump : TEXCOORD1;
		float alpha : TEXCOORD2;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.alpha = v.color.a;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uvgrab = ComputeGrabScreenPos(o.vertex);
		o.uvbump = TRANSFORM_TEX(v.uv, _NormalMap);
		return o;
	}



	half4  frag(v2f i) : COLOR
	{
		half3 bump = UnpackNormal(tex2D(_NormalMap,i.uvbump));

		float2 distort = bump.xy * _DistortAmount * _GrabTexture_TexelSize.xy;

		i.uvgrab.xy = distort * i.uvgrab.z + i.uvgrab.xy;

		half4  col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
		col.a = dot(float3(0,0,1), bump) * i.alpha * _DistortAlphaBoost;
		col.a = clamp(col.a, 0, 1);

		return  col;
	}
		ENDCG
	}



			Tags{ "Queue" = "Transparent-1" "RenderType" = "Transparent" }
		// Grab the screen behind the object into _GrabTexture
		GrabPass
	{
		"_GrabTexture"
	}
		Cull Front
		Pass
	{
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"



		float _DistortAmount;
	float4 _NormalMap_ST;


	sampler2D _NormalMap;
	sampler2D _GrabTexture;
	float4 _GrabTexture_TexelSize;
	float _DistortAlphaBoost;
	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		fixed4 color : COLOR;
	};

	struct v2f
	{
		float4 uvgrab : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float2 uvbump : TEXCOORD1;
		float alpha : TEXCOORD2;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.alpha = v.color.a;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uvgrab = ComputeGrabScreenPos(o.vertex);
		o.uvbump = TRANSFORM_TEX(v.uv, _NormalMap);
		return o;
	}



	half4  frag(v2f i) : COLOR
	{
		half3 bump = UnpackNormal(tex2D(_NormalMap,i.uvbump));

		float2 distort = bump.xy * _DistortAmount * _GrabTexture_TexelSize.xy;

		i.uvgrab.xy = distort * i.uvgrab.z + i.uvgrab.xy;

		half4  col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
		col.a = dot(float3(0,0,1), bump) * i.alpha * _DistortAlphaBoost;
		col.a = clamp(col.a, 0, 1);

		return  col;
	}
		ENDCG
	}
	}
}
