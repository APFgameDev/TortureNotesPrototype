#ifndef Custom_Lighting_Models
#define Custom_Lighting_Models
#include "UnityGlobalIllumination.cginc"
sampler2D _LightWrapTex;
sampler2D _MatalicTex;
half _Gain = 1;
half _Knee = 0.5;
half _Compress = 0.5;
half _Glossiness;// specular power in 0..1 range
half _Specular;// specular intensity
//half _Metallic;
//half _Smoothness;


#define LightingHalfBlinnPhongLightWrap_deferred

#ifndef HalfBlinnPhongLightWrapDiffuseFunction
inline half4 HalfBlinnPhongLightWrapDiffuseFunction(SurfaceOutput s, half3 viewDir, UnityGI gi)
{
	s.Normal = normalize(s.Normal);

	fixed dotPower = dot(s.Normal, gi.light.dir);

	fixed4 c;
	half2 uv_LightWrapTex;
	uv_LightWrapTex.y = 0;
	uv_LightWrapTex.x = dotPower * 0.5 + 0.5;
	c.rgb = (s.Albedo * gi.light.color * tex2D(_LightWrapTex, uv_LightWrapTex)).rgb;
#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
	c.rgb += s.Albedo * gi.indirect.diffuse;
#endif

	c.a = s.Alpha;
	return c;
}

inline half4 HalfBlinnPhongLightWrapLight(SurfaceOutput s, half3 viewDir, UnityGI gi)
{
	s.Normal = normalize(s.Normal);

	fixed dotPower = dot(s.Normal, gi.light.dir);

	fixed4 c;
	half2 uv_LightWrapTex;
	uv_LightWrapTex.y = 0;
	uv_LightWrapTex.x = dotPower * 0.5 + 0.5;
	c.rgb = (s.Albedo * gi.light.color * tex2D(_LightWrapTex, uv_LightWrapTex)).rgb;
#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
	c.rgb += s.Albedo * gi.indirect.diffuse;
#endif

	c.a = s.Alpha;
	return c;
}
#endif

#ifndef HalfBlinnPhongLightWrapSpecFunction
inline fixed HalfBlinnPhongLightWrapSpecFunction(SurfaceOutput s, half3 viewDir, UnityGI gi)
{
	s.Normal = normalize(s.Normal);
	half3 h = normalize(gi.light.dir + viewDir);
	fixed nh = max(0, dot(s.Normal, h));
	fixed spec = max(0, nh / s.Specular);
	return spec;
}


#endif
//LightingHalfBlinnPhongLightWrap Legacy
#ifndef LightingHalfBlinnPhongLightWrap
	inline fixed4 LightingHalfBlinnPhongLightWrapLegacy(SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)
	{
		half3 h = normalize(lightDir + viewDir);
	fixed dotPower = dot(s.Normal, lightDir);
		dotPower = dotPower * 0.5 + 0.5;
		fixed nh = max(0, dot(s.Normal, h));
		fixed spec = max(0, pow(nh, s.Specular * 128) * s.Gloss);

		fixed4 c;
		half2 uv_LightWrapTex;
		uv_LightWrapTex.y = 0;
		uv_LightWrapTex.x = dotPower;
		c.rgb = (s.Albedo * _LightColor0.rgb * (tex2D(_LightWrapTex, uv_LightWrapTex)).rgb  + spec * _LightColor0.rgb) * (atten * 2);
		c.a = s.Alpha;
		return c;
	}
#endif
//LightingHalfBlinnPhongLightWrap forward
#ifndef LightingHalfBlinnPhongLightWrap_forward
	inline half4 LightingHalfBlinnPhongLightWrap(SurfaceOutput s, half3 viewDir, UnityGI gi)
	{
		s.Normal = normalize(s.Normal);

		fixed dotPower = dot(s.Normal, gi.light.dir);
		dotPower = dotPower;
		half3 h = normalize(gi.light.dir + viewDir);
		fixed nh = max(0, dot(s.Normal, h));
		fixed spec = max(0, pow(nh, s.Specular * 128) * s.Gloss);

		fixed4 c;
		half2 uv_LightWrapTex;
		uv_LightWrapTex.y = 0;
		uv_LightWrapTex.x = dotPower * 0.5 + 0.5;
		c.rgb = (s.Albedo * gi.light.color * (tex2D(_LightWrapTex, uv_LightWrapTex)).rgb + spec * gi.light.color);
#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
		c.rgb += s.Albedo * gi.indirect.diffuse;
#endif

		c.a = s.Alpha;
		return c;
	}

	inline void LightingHalfBlinnPhongLightWrap_GI(SurfaceOutput s, UnityGIInput data, inout UnityGI gi)
	{
		gi = UnityGlobalIllumination(data, 1.0, s.Normal);
	}

#endif

//LightingHalfBlinnPhongLightWrap deferred 
#ifndef LightingHalfBlinnPhongLightWrap_deferred 
	inline half4 LightingHalfBlinnPhongLightWrap_Deferred (SurfaceOutput s, half3 viewDir, UnityGI gi, out half4 outGBuffer0, out half4 outGBuffer1, out half4 outGBuffer2)
{
		 half3 specColor;
		 s.Albedo = HalfBlinnPhongLightWrapDiffuseFunction(s, viewDir, gi);
		 s.Specular = HalfBlinnPhongLightWrapSpecFunction(s, viewDir, gi);
    UnityStandardData data;
    data.diffuseColor   = s.Albedo;
    data.occlusion      = 0;
    // PI factor come from StandardBDRF (UnityStandardBRDF.cginc:351 for explanation)
    data.specularColor  = fixed4(1,1,1,1) * s.Gloss;
    data.smoothness     = s.Specular;
    data.normalWorld    = s.Normal;


	outGBuffer0 = half4(HalfBlinnPhongLightWrapDiffuseFunction(s, viewDir, gi).rgb, 0);
	outGBuffer1 = half4(fixed3(1, 1, 1) * s.Gloss, data.smoothness);
		outGBuffer2 = half4(s.Normal * 0.5f + 0.5f, 1.0f);
   // UnityStandardDataToGbuffer(data, outGBuffer0, outGBuffer1, outGBuffer2);

    half4 emission = half4(s.Emission, 1);

    #ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
        emission.rgb += s.Albedo * gi.indirect.diffuse;
    #endif

    return emission;
}
#endif



inline fixed4 LightingHalfBlinnPhongLightWrap_PrePass (SurfaceOutput s, half4 light)
{
    fixed spec = light.a * s.Gloss;

    fixed4 c;
    c.rgb = (s.Albedo * light.rgb + light.rgb * _SpecColor.rgb * spec);
    c.a = s.Alpha;
    return c;
}

#endif