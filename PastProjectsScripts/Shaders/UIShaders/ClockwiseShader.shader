// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ClockwiseShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_PlayerHealth ("PlayerHealth", Range(-1,1)) = 1
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
			#define PI 3.1415926535897932384626433832795
			
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

			v2f vert (appdata v)
			{
				v2f o;
				o.color = v.color;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			uniform float _PlayerHealth;
		   	
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 col = i.color * tex2D(_MainTex, i.uv);

				float sinX = sin(-PI / 2);
				float cosX = cos(-PI / 2);
				float sinY = sin(-PI / 2);
				float2x2 rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);

				float2 displacement = i.uv - float2(0.5f, 0.5f);
				displacement = mul(displacement, rotationMatrix);
				float angleRads = atan2(displacement.y, displacement.x);
				if (angleRads > PI * _PlayerHealth)
				{
					col.a = 0;
				}

				return col;
			}
			ENDCG
		}
	}
}
