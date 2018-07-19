struct CustomDecalInput
{
	float3 worldPos;
	float3 worldNormal;
	float4 screenPos;
};

struct DecalParams
{
	float alphaFallOff;
	float decalIntensity;
	float shineDistance;
	float shineFactor;
	float maxShine;
	half4 tintColor;
};


// maximum decals supported is 50
half4 GetDecalColor(sampler2D aDecalTex, int numDecals, float4 worldPositions[50], CustomDecalInput IN, DecalParams PARAMS, out half smoothnessOutput)
{
	half4 c = half4(0, 0, 0, 0);
	for (int i = 0; i < numDecals; i++)
	{
		float3 worldUV = IN.worldPos;
		worldUV = worldUV - worldPositions[i];

		worldUV.x += abs(worldUV.y) * (worldUV.x / worldPositions[i].w);
		worldUV.z += abs(worldUV.y) * (worldUV.z / worldPositions[i].w);

		float falloff = 1 - (dot(IN.worldNormal, float3(0, 1, 0))  * 0.5 + 0.5);

		float sinX = sin(float(i));
		float cosX = cos(float(i));
		float sinY = sin(float(i));
		float2x2 rotationMatrix = float2x2(cosX, -sinX, sinY, cosX);
		worldUV.xz = mul(worldUV.xz, rotationMatrix);

		worldUV.x += 0.5 * worldPositions[i].w;
		worldUV.z += 0.5 * worldPositions[i].w;

		worldUV.x = worldUV.x / worldPositions[i].w;
		worldUV.z = worldUV.z / worldPositions[i].w;


		if (worldUV.x > 0 && worldUV.x < 1 &&
			worldUV.z > 0 && worldUV.z < 1)
		{
			// working color
			float4 col = tex2D(aDecalTex, worldUV.xz);

			col.a = col.a * 1 - abs(worldUV.y) / PARAMS.alphaFallOff - falloff;
			col.a *= PARAMS.decalIntensity;
			col.a = clamp(col.a, 0, 1);

			// blend c with working col
			c.rgb = col.rgb * PARAMS.tintColor * col.a + c.rgb * (1 - col.a);

			c.a = max(c.a, col.a);

			IN.screenPos.w = max(0.1, IN.screenPos.w);
			float decalDepth = Linear01Depth(IN.screenPos.z / IN.screenPos.w ) * PARAMS.shineDistance;
			smoothnessOutput = clamp(c.a * pow(1 - decalDepth, PARAMS.shineFactor), 0, PARAMS.maxShine);
		}
	}
	return c;
}
