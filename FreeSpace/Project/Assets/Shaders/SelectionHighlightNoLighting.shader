// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/SelectionHighlightNoLighting" {
	Properties {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1.0,1.0,1.0,1.0)
		_HighlightColor("HighlightColor", Color) = (1.0,1.0,1.0,1.0)
		_HighLightSize("HighLightSize", Float) = 1.0

	}
	SubShader {


		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
		Cull Back 
		Pass
		{
			CGPROGRAM
                #pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#pragma target 3.0
           
				struct appdata
				{
					float4 vertex : POSITION;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
				};

				fixed4 _HighlightColor;
				float _HighLightSize;
               
				v2f vert (appdata  v)
				{
				   v2f o;

			   		v.vertex.xyz *= 1 + _HighLightSize;
					o.vertex = UnityObjectToClipPos(v.vertex);

					return o;
				}

				fixed4 frag (v2f i) : SV_Target
				{
					return _HighlightColor;
				}

			ENDCG
		}

		Tags { "RenderType"="Opaque" }
		LOD 200
		Cull Back 
		Pass
		{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
				#include "UnityCG.cginc"
		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

  

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

			float4 _MainTex_ST;     
			sampler2D _MainTex;	
			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex,i.uv) * _Color;
				return col;
			}
				ENDCG
			}
	
	}
	FallBack "Diffuse"
}
