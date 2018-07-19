//please define BumpUpAmount


#pragma	target	3.0
sampler2D _BurnTex;
sampler2D _BurnTexOpacity;
sampler2D _BurnMagma;
sampler2D _MagmaNoise;

float _MagmaNoiseUVScale;
sampler2D _NoiseTex;
float _NoiseUV_XScale;
float _NoiseUV_YScale;
float _NoiseScale;
float _NoiseSpeed;

float _MagmaPassAlphaAdjuster;
float _MagmaPassBrightnessAdjuster;

float _BurnDistance;
float4 _BurnOriginWorldPos;
float _MagmaNoiseSecondPassScaleAdd;

struct Input {
	float2 uv_BurnTex;
	float3 worldPos;
	float4 screenPos;
};

void vert(inout	appdata_full	v)
{
	v.vertex.y += BumpUpAmount;
	v.vertex.xz += SlideAmount;
}

void surf(Input IN, inout SurfaceOutputStandard o)
{
	float dist = distance(IN.worldPos, _BurnOriginWorldPos);
	fixed4 col = fixed4(0,0,0,0);

	if (dist < _BurnDistance)
	{
		float lerpWeight = tex2D(_BurnTexOpacity, IN.uv_BurnTex);
		float emissionAmount = tex2D(_MagmaNoise, IN.uv_BurnTex * _MagmaNoiseUVScale + _Time.x);
		emissionAmount = max(emissionAmount, tex2D(_MagmaNoise, IN.uv_BurnTex + _MagmaNoiseSecondPassScaleAdd * _MagmaNoiseUVScale - _Time.x));
		emissionAmount *= abs( _SinTime.x);
		col = tex2D(_BurnMagma, IN.uv_BurnTex ) * (1 - lerpWeight) * emissionAmount;
	
	}
	o.Albedo = col * Brightness * _MagmaPassBrightnessAdjuster;
	o.Alpha = col.a * _MagmaPassAlphaAdjuster;
}