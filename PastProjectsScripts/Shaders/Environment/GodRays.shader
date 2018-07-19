Shader "Custom/GodRays"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_UVOffset1("_UVOffset1", Range(0,1)) = 0
		_UVOffset2("_UVOffset2", Range(0,1)) = 0
		_UVOffset3("_UVOffset2", Range(0,1)) = 0
		_TintColor("_Tint", COLOR) = (1,1,1,1)
		_Luminosity("luminosity", Range(0.1,100)) = 2
	}
		SubShader
	{

		Cull Off

		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "ForceNoShadowCasting" = "True" }
		ZTest LEqual
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		Blend SrcColor OneMinusSrcColor
		Blend DstColor One
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			float4 _MainTex_ST;
			sampler2D _MainTex;
			float _UVOffset1;
			float _UVOffset2;
			float _UVOffset3;
			float4 _TintColor;
			float _Luminosity;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
		

			fixed4 frag (v2f i) : SV_Target
			{
			fixed4 col1 = tex2D(_MainTex, i.uv);
			fixed4 col2 = tex2D(_MainTex, float2(i.uv.x + _UVOffset1, i.uv.y));
			fixed4 col3 = tex2D(_MainTex, float2(i.uv.x + _UVOffset2, i.uv.y));
			fixed4 col4 = tex2D(_MainTex, float2(i.uv.x + _UVOffset3, i.uv.y));

			col1 *= max(sin(_Time.y), 0);
			col2 *= max(sin(_Time.y + 1), 0);
			col3 *= max(sin(_Time.y) * -1, 0);
			col4 *= max(sin(_Time.y + 1) * -1, 0);

			fixed4 col = col1 + col2 + col3 + col4;
			return col * _TintColor * _Luminosity;
			}
			ENDCG
		}
	}
}
