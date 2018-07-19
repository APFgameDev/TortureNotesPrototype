Shader "Custom/OutLineDepth"
{
	Properties
	{
		_OutLineColor("_OutLineColor", Color) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		_Bias("Bias",Range(0,0.01)) = 0.0001
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float2 uv_depth : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv_depth = v.uv.xy;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D_float _DepthTexPlayer;
			sampler2D_float _CameraDepthTexture;
			fixed4 _OutLineColor;
			float _Bias;
			fixed4 frag (v2f i) : SV_Target
			{
				float depthPlayer = DecodeFloatRG(tex2D(_DepthTexPlayer, i.uv_depth));
				
		
				if (depthPlayer < 0.0001)
					return tex2D(_MainTex, i.uv);

				float depthEverything = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));

				if (depthEverything <= depthPlayer + _Bias)
					return tex2D(_MainTex, i.uv);
				else
					return _OutLineColor * _OutLineColor.a + tex2D(_MainTex, i.uv) * (1 - _OutLineColor.a);
			}
			ENDCG
		}
	}
}
