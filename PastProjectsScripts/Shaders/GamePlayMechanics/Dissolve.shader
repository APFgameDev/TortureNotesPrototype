Shader "Custom/Dissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_DissolveTex ("Dissolve Tex", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_DissolveAmount("Dissolve Amount", Range(0,1)) = 0
            _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
			_ShadowColor("ShadowColor", Color) = (1,1,1,1)
	}
	SubShader {
           
        Tags { "ForceNoShadowCasting" = "True" "Queue" = "Transparent"  "IgnoreProjector" = "True"  "RenderType" = "Transparent" }
        LOD 200

            ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM
          

            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Lambert alpha:fade 

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            

            sampler2D _MainTex;
            sampler2D _DissolveTex;

            struct Input {
                float2 uv_MainTex;
                float2 uv_DissolveTex;
            };

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            fixed _DissolveAmount;
			fixed4 _ShadowColor;


            void surf(Input IN, inout SurfaceOutput o)
             {
               
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				
                o.Albedo = c.rgb;
				_Color.a = o.Alpha = clamp( _DissolveAmount,0,1);
            }
            
		ENDCG
	}
            Fallback "Transparent/Cutout/Diffuse"
}
