// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Projector/ProjectorShader"
{
	Properties{
		_ShadowTex("Projected Image", 2D) = "white" {}
	_Color("Main Color", Color) = (1,1,1,1)
		_FalloffTex("FallOff Texture", 2D) = "" {}
	_Attenuation("Attenuation", Range(0.0, 10.0)) = 1.0
		_AttenuationPow("Attenuation Pow", Range(0.0, 5)) = 1.0
		_FalloffPower("Falloff Texture Power", Range(0.1, 20.0)) = 10.0
		_ProjectorDir("_ProjectorDir", Vector) = (0,0,0,0)
		_Luminosity("Luminosity", Range(0.1, 10.0)) = 1.0
		_NormalFallOff("Normal Fall Off", Range(0.1, 10.0)) = 1.0
	}
		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Pass{
		Blend Zero OneMinusSrcAlpha // attenuate color in framebuffer 
		Blend SrcColor OneMinusSrcColor
		Blend DstColor One
		ZWrite On
		ZTest Equal // by 1 minus alpha of _ShadowTex 

		CGPROGRAM

#pragma vertex vert  
#pragma fragment frag 
#include "UnityCG.cginc"

		// User-specified properties
		uniform sampler2D _ShadowTex;
	sampler2D _FalloffTex;
	fixed4 _Color;
	float4 _ProjectorDir;
	float _Attenuation;
	float _AttenuationPow;
	float _FalloffPower;
	float _Luminosity;
	float _NormalFallOff;

	// Projector-specific uniforms
	uniform float4x4 unity_Projector; // transformation matrix 
	float4x4 unity_ProjectorClip;

	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};
	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 posProj : TEXCOORD0;
		float4 posFalloff : TEXCOORD1;
		float3 localNormal : TEXCOORD3;
	};

	vertexOutput vert(vertexInput input)
	{
		vertexOutput output;

		output.posProj = mul(unity_Projector, input.vertex);
		output.posFalloff = mul(unity_ProjectorClip, input.vertex);
		output.pos = UnityObjectToClipPos(input.vertex);
		output.localNormal = mul((float4x4)unity_ObjectToWorld, input.normal);
		return output;
	}

	bool testClipSpace(float2 clipCoords)
	{
		return clipCoords.x < 1 && clipCoords.x > 0 &&
			clipCoords.y < 1 && clipCoords.y > 0;
	}



	float4 frag(vertexOutput input) : COLOR
	{



	float depth = input.posProj.z / input.posProj.w;
	depth = depth * 0.5 + 0.5;
	float2 uv_ShadowTex = UNITY_PROJ_COORD(input.posProj) / (input.posProj.w);


	if (testClipSpace(uv_ShadowTex) && depth < 1 && depth > 0) // in clip Space?
	{
		fixed4 texF = tex2D(_FalloffTex, float2(input.posProj.x / input.posProj.w,input.posProj.y / input.posProj.w));
		float dotNorm = -dot(input.localNormal, _ProjectorDir);
		dotNorm *= _NormalFallOff;
		dotNorm = min(dotNorm, 1);
		dotNorm = max(dotNorm, -1);
		dotNorm = dotNorm * 0.5 + 0.5;
	
	
		float atten = max( 1- depth /pow(_Attenuation,1-_AttenuationPow), 0);
		float4 texCol = tex2D(_ShadowTex, uv_ShadowTex) + (_Color - 0.5) * 2 * _Color.a;
		float alpha = pow(texF.a, _FalloffPower) * atten;
		return clamp( texCol * alpha * dotNorm  * _Luminosity,0,1 );
	}

	return float4(0,0,0,0);
	}

		ENDCG
	}
	}
}
