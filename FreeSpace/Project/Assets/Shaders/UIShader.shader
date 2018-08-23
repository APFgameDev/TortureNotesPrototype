Shader "Custom/UI/SurfShader" {
    Properties{
		[PerRendererData]  _MainTex("Texture", 2D) = "white" {}
	// required for UI.Mask
		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
    }
    SubShader{
    Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
    ZTest Off
    Cull Off
    LOD 200
			// required for UI.Mask
			Stencil
		{
			Ref[_Stencil]
			Comp[_StencilComp]
			Pass[_StencilOp]
			ReadMask[_StencilReadMask]
			WriteMask[_StencilWriteMask]
		}
			ColorMask[_ColorMask]

    CGPROGRAM
#pragma surface surf NoLighting alpha
    
        sampler2D _MainTex;
        fixed4 _Color;

	


        fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            fixed4 c;
            c.rgb = s.Albedo;
            c.a = s.Alpha;
            return c;
        }

    struct Input {
        float2 uv_MainTex;
		float4 color : COLOR;
    };

    void surf(Input IN, inout SurfaceOutput o) {
        fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

        o.Albedo =  c.rgb * IN.color.rgb;
        o.Alpha = c.a;


		

    }
    ENDCG
    }
}