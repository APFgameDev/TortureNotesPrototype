// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/WaterSurf" {
	Properties {
		_MainTex("MainTexture", 2D) = "white" {}
		_HeightMap("HeightMap0Tex", 2D) = "black" {}

		_HeightMapMult0("HeightMap0Multiplier", float) = 0.75
		_HeightMapMult1("HeightMap1Multiplier", float) = 1.25
		
		_HeightMapTile("_HeightMapTile", Range(0,0.1)) = 0.01

		_HeightMapSpeed("ScrollingSpeed", float) = 0.1
		_Color ("Color", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0

	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
		//ZWrite Off
		//LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard addshadow vertex:vert //alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _HeightMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_HeightMap0;

		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float _HeightMapMult0;
		float _HeightMapMult1;
		float _HeightMapSpeed;
		float _HeightMapTile;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void vert(inout appdata_full v)
		{
			float4 worldPos = mul(unity_ObjectToWorld, v.vertex);

			//helper values
			float2 tiling = worldPos.xz * _HeightMapTile;
			float offset = _Time.y * _HeightMapSpeed;
			float offset2 = -_Time.y * _HeightMapSpeed * 3.0;

			float2 scrollWorld = tiling;
			float heightMap = 0.0;



			//apply height using our first height map
			scrollWorld.x += offset;
			scrollWorld.y += offset;

			heightMap = tex2Dlod(_HeightMap, float4(scrollWorld,0,0.0)).r;
			worldPos.y += heightMap * _HeightMapMult0;



			//apply height using our second height map
			scrollWorld = tiling;
			scrollWorld.x += offset2;
			scrollWorld.y += offset2;

			heightMap = tex2Dlod(_HeightMap, float4(scrollWorld,0,0.0)).r;
			worldPos.y += heightMap * _HeightMapMult1;



			//convert back to object position
			v.vertex = mul(unity_WorldToObject, worldPos);

		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}

		ENDCG
	}
	FallBack "Diffuse"
}
