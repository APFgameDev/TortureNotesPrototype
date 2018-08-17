// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/SelectionHighlightTransparent" {
	Properties {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
		    _Color("Tint", Color) = (1.0,1.0,1.0,1.0)
		_MainTexSmoothness("MainTex Smoothness", Range(0,1)) = 0.94
        _MetallicGlossMap("_MetallicTex", 2D) = "white" {}
		_BumpMap("NormalMap", 2D) = "bump" {}
		_OcclusionMap("Occlusion", 2D) = "black" {}
		_OcclusionStrength("Occlusion_Strength", Range(0.0, 1.0)) = 1.0
        _BumpScale("BumpScale", Float) = 1.0
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("_Metallic", Range(0,1)) = 1.0
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
				if(_HighLightSize > 0)
						return _HighlightColor;
						else
						return fixed4(0,0,0,0);

				}

			ENDCG
		}
		
		Tags {"Queue" = "Transparent" "RenderType"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 200
		Cull Back 

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows keepalpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

        sampler2D _MainTex;
        float _Metallic;
        float _BumpScale;
        float _OcclusionStrength;
        float _Glossiness;
		float4 _Color;
	

        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;

		struct Input {
			float2 uv_MainTex;
            INTERNAL_DATA
		};
		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
            fixed4 texCol = tex2D(_MainTex,IN.uv_MainTex);

			o.Albedo = texCol * _Color;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex)) * _BumpScale;
			o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;
            o.Smoothness = _Glossiness;
            o.Occlusion = lerp(1.0, tex2D(_OcclusionMap, IN.uv_MainTex), _OcclusionStrength);
			o.Alpha = _Color.a;
		}
		 
		ENDCG
	}
	FallBack "Diffuse"
}
