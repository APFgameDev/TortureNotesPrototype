Shader "Custom/Particle/Light"
{
	Properties
	{
		_MainTex("Texture", 2D) = "black" {}
			_TintColor("Tint Color", Color) = (1, 1, 1, 1)
			_TexAlphaAmount("_TexAlphaAmount", Range(0, 1)) = 1
			_TexBrightnessAdjust("_Texture Birghtness Adjuster", Range(0.1, 10)) = 2
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		ZTest Less
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		//Blend OneMinusDstColor One // Soft Additive

		//

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
	}
}
