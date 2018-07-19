Shader "Custom/RadialTextureBurnIn"
{
	Properties
	{
		_MainTex ("MainTexture", 2D) = "white" {}
		_BurnTex("_BurnTex", 2D) = "white" {}
		_BurnDistance("BurnDistance",float) = 0
		_BurnWidth("Burn Width", float) = 5
		_EdgeStartColor("BurnEdgeStartColor" , Color) = (1,0,0,1)
			_LeadSharp("Leading Edge Sharpness", float) = 10
		_EdgeEndColor("BurnEdgeEndColor" , Color) = (1,0,0,0)
			_BurnOriginWorldPos ("_BurnOriginWorldPos",Vector ) = (1,0,0,0)
			_CameraWorldPos("_CameraWorldPos",Vector) = (1,0,0,0)

			_NoiseTex("_NoiseTex", 2D) = "white" {}
		_NoiseUV_YScale("_NoiseUV_YScale", Range(0,1)) = 0.001
			_NoiseUV_XScale("_NoiseUV_XScale", Range(0,1)) = 0.1
			_NoiseScale("_NoiseScale", Range(0,10)) = 1
			_NoiseSpeed("_NoiseSpeed", Range(0,10)) = 0.1
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
				float4 ray : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uv_depth = v.uv.xy;


				o.interpolatedRay = v.ray;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BurnTex;
	
			sampler2D_float _CameraDepthTexture;
			//origin of burn
			float4 _BurnOriginWorldPos;
			float4 _CameraWorldPos;
			fixed4 _EdgeStartColor;
			fixed4 _EdgeEndColor;
			float _BurnDistance;
			float _BurnWidth;
			float _LeadSharp;

			sampler2D _NoiseTex;
			float _NoiseUV_XScale;
			float _NoiseUV_YScale;
			float _NoiseScale;
			float _NoiseSpeed;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;


			col = tex2D(_MainTex, i.uv);


			float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
			float linearDepth = Linear01Depth(rawDepth);

			float4 worldSpaceRay = linearDepth * i.interpolatedRay;
			float3 worldSpacePos = _CameraWorldPos + worldSpaceRay;


			float3 displacement = (worldSpacePos - _BurnOriginWorldPos);

			float noiseUVX = atan2(displacement.x, displacement.z) * _NoiseUV_XScale + _Time.z * _NoiseSpeed;

			float dist = distance(worldSpacePos, _BurnOriginWorldPos);
			dist -= tex2D(_NoiseTex, float2(noiseUVX, _BurnDistance * _NoiseUV_YScale)) * _NoiseScale;

			if (dist < _BurnDistance  && dist > _BurnDistance - _BurnWidth && linearDepth < 1)
			{
				float lerpAmount = 1 - (_BurnDistance - dist) / _BurnWidth;
				fixed4  colAdd = lerp(_EdgeStartColor, _EdgeEndColor, pow(lerpAmount, _LeadSharp));
				col += colAdd * colAdd.a;
			}

		

				return col;
			}
			ENDCG
		}
	}
}
