
Shader "Custom/UI/TextShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		ZTest Off
		Tags
	{
		"Queue" = "Overlay"
		"RenderType" = "Transparent"
		"IgnoreProjector" = "True"
	}
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		sampler2D _MainTex;
	float4 _MainTex_ST;

	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
		float4 color : COLOR;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 color : COLOR;
		float4 pos : POSITION;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.color = v.color;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}



	fixed4 frag(v2f i) : COLOR
	{

		fixed4 c = tex2D(_MainTex, i.uv);
		c.rgb = (1 - c.rgb) * i.color;
		return c;
	}
		ENDCG
	}
	}
}
