Shader "Custom/PostProcessing/Blur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_FocusRadius("FocusRadius", Range(0,1)) = 0.5
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
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _FocusRadius;
			static const int KERNEL = 51;

			uniform float _Weights[KERNEL];
			uniform float _OffsetX[KERNEL];
			uniform float _OffsetY[KERNEL];
			uniform int _Kernel = 1;
			uniform int _Dir;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(0, 0, 0, 1.0);

				for(int id = 0; id < _Kernel; id++) {
					float xuv = _OffsetX[id] * _Dir;
					float yuv = _OffsetY[id] * (1 - _Dir);


					float xuvN = i.uv.x / 1 * 2 - 1;
					float yuvN = i.uv.y / 1 * 2 - 1;

					float uvMag = sqrt(xuvN * xuvN + yuvN * yuvN) / 1.41421356237;
				
					float inverseRadius =  1 - _FocusRadius;
					if (inverseRadius == 0)
						inverseRadius = -0.1f;

					float focus = max(uvMag - _FocusRadius, 0) / inverseRadius;

					focus *= focus;

					col += tex2D(_MainTex, i.uv + float2( xuv *focus, yuv *focus)) * _Weights[id];

				

				}

				return col;
			}
			ENDCG
		}
	}
}
