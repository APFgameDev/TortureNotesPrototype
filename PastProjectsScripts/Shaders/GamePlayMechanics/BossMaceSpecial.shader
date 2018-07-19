Shader "Custom/BossMaceSpecial" {
	Properties {
        _RimColour ("Rim Colour", Color) = (1,0,0,1)
        _RimPower("Rim Power", Range(0.5,8.0)) = 3.0
		_FallOffRadius("FallOffRadius", Float) = 1
	
		_ChargeOrigin("charge origin", Vector) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent"  "ForceNoShadowCasting" = "True" }

		LOD 200
          //  Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
            float3 viewDir;
			float3 worldPos;
		};

        float4 _RimColour;
        float _RimPower;

		float _ChargeRadius = 0.0;
		float4 _ChargeOrigin;
		float _FallOffRadius;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) 
        {
            float rim = 1.0 - abs(dot(normalize(IN.viewDir), o.Normal));
            rim = pow(rim, _RimPower);


			float fragDist = distance(IN.worldPos, _ChargeOrigin);

			float fade = (fragDist - _ChargeRadius) / _FallOffRadius;

			if (fragDist >= _ChargeRadius)
			{
				fade = saturate(1 - fade);
				rim = rim * fade;
			}

			o.Albedo = _RimColour;
            o.Alpha = rim;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
