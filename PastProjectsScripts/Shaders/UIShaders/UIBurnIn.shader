Shader "Custom/UI/BurnInShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_CutOffMap("CutOffMap", 2D) = "white" {}
		_CutOffAmount("Cut Off Amount" ,Range(-1.0, 1.0)) = 0.5

		_GradientScale("Gradient Scale", Range(0.0,1.0)) = 0.2
		_CutOffGradient("Cut Off Gradient", 2D) = "white" {}
		_CutOffGradientVisability("_CutOffGradientVisability", Range(0.0,10.0)) = 1
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
			sampler2D _CutOffMap;
		sampler2D _CutOffGradient;
		float _CutOffAmount;
		float _GradientScale;
		float _CutOffGradientVisability;

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

			half test = tex2D(_CutOffMap, i.uv).rgb - _CutOffAmount;
			clip(test);


		c.rgb = c.rgb * i.color;


			if (test < _GradientScale)
			{
				c *= tex2D(_CutOffGradient, float2(test *(1 / _GradientScale), 0)) * _CutOffGradientVisability;
			}

		return c;
	}
		ENDCG
	}
	}
}