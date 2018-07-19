Shader "Custom/WaterBufferSurface" 
{
	Properties 
    {
		ScrollDirX("ScrollDirX", Range(-1.0,1.0)) = 0.0
		ScrollDirZ("ScrollDirZ", Range(-1.0,1.0)) = 0.0
		ScrollSpeed("ScrollSpeed", Float) = 0.0

		WaterWarpFreq("WaterWarpFrequency", Float) = 0.0
		WaterWarpAmp("WaterWarpAmplitued", Float) = 0.0
		WaterWarpSpeed("WaterWarpSpeed", Float) = 0.0

		WaterTex("WaterTex", 2D) = "white" {}
		CrestTex("CrestTex", 2D) = "white" {}
		CrestRatioOffset("CrestRatioOffset", Float) = 0.0
        RenderHeightMap ("RenderHeightMap", 2D) = "black" {}
        MaxOffset ("MaxNormalHeight", Float) = 1
        MaxOffset ("MaxRemainderHeight", Float) = 1
        SpecularAmount ("Specular", Float) = 1
		EmmissiveAmount ("EmmissiveAmmount", Range(0.0,1.0) ) = 0.1
        Gloss ("Gloss", Float) = 1

        //Note: even though this is not referenced in this code, it is still needed to make the specular work
        _SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0) 
            _BaseTintColor("_tintColor", Color) = (0.5, 0.5, 0.5, 0)
            _CrestTintColor("crest Tint", Color) = (1,1,1,1)
	}
    
	SubShader 
    {
		Tags { "RenderType"="Opaque" "RenderQueue"="Geometry"}
		Cull Off
            //Blend One OneMinusSrcAlpha
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong vertex:vert// keepalpha
        #pragma target 5.0
        
		sampler2D WaterTex;
		sampler2D CrestTex;

        sampler2D RenderHeightMap;
        
        samplerCUBE ReflectionCubeMap;
        
		float ScrollDirX;
		float ScrollDirZ;
		float ScrollSpeed;

		float WaterWarpFreq;
		float WaterWarpAmp;
		float WaterWarpSpeed;

		float CrestRatioOffset;
        float MaxNormalHeight;
        float MaxRemainderHeight;
        float2 CellSizePercent;
        
        float SpecularAmount;
        float Gloss;

        float4 _BaseTintColor;
        float4 _CrestTintColor;

		float EmmissiveAmount;
		struct Input 
        {
			float2 uvWaterTex;
			float2 uvCrestTex;
            float2 uvRenderHeightMap;
            float3 worldNormal;
            float3 worldRefl;
            float3 viewDir;
            float3 worldPos;
		};  
        
        float SampleHeight(float2 uv)
        {
			float height = tex2Dlod(RenderHeightMap, float4(uv, 0, 0)).r;

			float heightOffset = abs(height);
            if (heightOffset > MaxNormalHeight)
            {
                float remainder = heightOffset - MaxNormalHeight;

				float adjustedRemainder = MaxRemainderHeight *(-exp(-remainder) + 1);

                height = sign(height) * (MaxNormalHeight + adjustedRemainder);
            }


            return height;
        }
        
        float3 CalcGridNormalHoriz(float centerHeight, float offsetHeight, float dir)
        {
            float heightDiff = centerHeight - offsetHeight;

            float3 normal = float3(heightDiff * dir, 1.0f, 0.0f);

            return normalize(normal);
        }

        float3 CalcGridNormalVert(float centerHeight, float offsetHeight, float dir)
        {
            float heightDiff = centerHeight - offsetHeight;

            float3 normal = float3(0.0f, 1.0f, heightDiff * dir);

            return normalize(normal);
        }
        
        void vert (inout appdata_full v)
        {
            //Get height
            float centerHeight = SampleHeight(v.texcoord.xy);
        
            v.vertex.y = centerHeight;
        
            //Calculate normal
            float2 horizOffset = float2(CellSizePercent.x, 0.0f);
            float2 vertOffset = float2(0.0f, CellSizePercent.y);
            
            float3 avgNormal = CalcGridNormalHoriz(
                centerHeight,
                SampleHeight(v.texcoord.xy - horizOffset),
                -1.0f
                );

            avgNormal += CalcGridNormalHoriz(
                centerHeight,
                SampleHeight(v.texcoord.xy + horizOffset),
                1.0f
                );

            avgNormal += CalcGridNormalVert(
                centerHeight,
                SampleHeight(v.texcoord.xy - vertOffset),
                -1.0f
                );

            avgNormal += CalcGridNormalVert(
                centerHeight,
                SampleHeight(v.texcoord.xy + vertOffset),
                1.0f
                );

            avgNormal = normalize(avgNormal);
            
			
            v.normal = avgNormal;
        }
        
		void surf (Input IN, inout SurfaceOutput o) 
        {
			float warpT = (_Time.y * WaterWarpSpeed);

			float2 scrollOffset = float2(ScrollDirX * _Time.y * ScrollSpeed, ScrollDirZ * _Time.y * ScrollSpeed);
			float2 texWaveWater = sin(IN.uvWaterTex * WaterWarpFreq + warpT) * WaterWarpAmp;
			float2 texWaveCrest = sin(IN.uvCrestTex * WaterWarpFreq + warpT) * WaterWarpAmp;

            half4 col = tex2D(WaterTex, (IN.uvWaterTex + texWaveWater) + scrollOffset);
			half4 crest = tex2D(CrestTex, (IN.uvCrestTex + texWaveCrest) + scrollOffset);

            float height = SampleHeight(IN.uvRenderHeightMap);
            half ratio = height / (MaxNormalHeight + CrestRatioOffset);

            col = (1 - col) * _BaseTintColor;
            crest = (1 - crest) *_CrestTintColor;

            o.Albedo = lerp(col, crest, ratio);

            
            

            o.Specular = lerp(SpecularAmount, 0.0, ratio);
            o.Gloss = lerp(Gloss,0.0, ratio);
            

			half4 emmissive = lerp(col, crest, ratio);
			o.Emission = emmissive * EmmissiveAmount;

			o.Alpha = 0.8;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
