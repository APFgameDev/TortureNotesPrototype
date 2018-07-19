Shader "Custom/PostProcessAdjustBrightnessAndContrast"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Contrast("Contrast", Range(0.1,50)) = 1
		_Gamma("_Gamma", Range(1,2)) = 1
		_Brightness("Brightness", Range(0.75,1.25)) = 1
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

			float _Contrast;
			float _Brightness;
			float _Gamma;

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
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);




				col.rgb = pow( col.rgb, _Gamma);

				col.rgb *= _Contrast;

				col.rgb += _Brightness - 1;

				col = clamp(col,0,1);
				return col;
			}
			ENDCG
		}
	}
}
