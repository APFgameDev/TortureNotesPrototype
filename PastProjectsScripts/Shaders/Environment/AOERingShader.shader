Shader "Custom/FX/AOERingShader" {
    Properties
    {
        _MainTex("Texture", 2D) = "black" {}
    _NormalMap("Normalmap", 2D) = "bump" {}
    _DistortAmount("Distorion Amount", Range(0,10000)) = 20
        _TexAlphaAmount("_TexAlphaAmount", Range(0,1)) = 1
        _DistortAlphaBoost("_DistortAlphaBoost", Range(0.1,50)) = 1
        _TexBrightnessAdjust("_Texture Birghtness Adjuster", Range(0.1,10)) = 2

    }
        SubShader
    {
        Tags{ "Queue" = "AlphaTest" "RenderType" = "Transparent" }
        ZTest LEqual
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha


        Tags{ "Queue" = "Transparent-1" "RenderType" = "Transparent" }
        // Grab the screen behind the object into _GrabTexture
        GrabPass
    {
        "_GrabTexture"
    }

        Pass
    {
        CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"



            float _DistortAmount;
        float4 _NormalMap_ST;


        sampler2D _NormalMap;
        sampler2D _GrabTexture;
        float4 _GrabTexture_TexelSize;
        float _DistortAlphaBoost;
        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            fixed4 color : COLOR;
        };

        struct v2f
        {
            float4 uvgrab : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float2 uvbump : TEXCOORD1;
            float alpha : TEXCOORD2;
            fixed4 color : COLOR;
        };

        v2f vert(appdata v)
        {
            v2f o;
            o.alpha = v.color.a;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uvgrab = ComputeGrabScreenPos(o.vertex);
            o.uvbump = TRANSFORM_TEX(v.uv, _NormalMap);
            o.color = v.color;
            return o;
        }



        half4  frag(v2f i) : COLOR
        {
            half3 bump = UnpackNormal(tex2D(_NormalMap,i.uvbump));

            float2 distort = bump.xy * _DistortAmount * _GrabTexture_TexelSize.xy * i.color.a;

            i.uvgrab.xy = distort * i.uvgrab.z + i.uvgrab.xy;

            half4  col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
            col.a = dot(float3(0,0,1), bump) * _DistortAlphaBoost;
            col.a = clamp(col.a, 0, 1);

            float tintAlpha = sqrt(bump.x * bump.x + bump.y * bump.y) * i.color.a;

            col.rgb = col.rgb * (1 - tintAlpha) + i.color.rgb * tintAlpha;
            return  col;
        }
            ENDCG
        }
        Cull Front
            GrabPass
        {
            "_GrabTexture"
        }

            Pass
        {
            CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"



            float _DistortAmount;
        float4 _NormalMap_ST;


        sampler2D _NormalMap;
        sampler2D _GrabTexture;
        float4 _GrabTexture_TexelSize;
        float _DistortAlphaBoost;
        struct appdata
        {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
            fixed4 color : COLOR;
        };

        struct v2f
        {
            float4 uvgrab : TEXCOORD0;
            float4 vertex : SV_POSITION;
            float2 uvbump : TEXCOORD1;
            float alpha : TEXCOORD2;
            fixed4 color : COLOR;
        };

        v2f vert(appdata v)
        {
            v2f o;
            o.alpha = v.color.a;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uvgrab = ComputeGrabScreenPos(o.vertex);
            o.uvbump = TRANSFORM_TEX(v.uv, _NormalMap);
            o.color = v.color;
            return o;
        }



        half4  frag(v2f i) : COLOR
        {
            half3 bump = UnpackNormal(tex2D(_NormalMap,i.uvbump));

            float2 distort = bump.xy * _DistortAmount * _GrabTexture_TexelSize.xy * i.color.a;

            i.uvgrab.xy = distort * i.uvgrab.z + i.uvgrab.xy;

            half4  col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
            col.a = dot(float3(0,0,1), bump) * _DistortAlphaBoost;
            col.a = clamp(col.a, 0, 1);

            float tintAlpha = sqrt(bump.x * bump.x + bump.y * bump.y) * i.color.a;

            col.rgb = col.rgb * (1 - tintAlpha) + i.color.rgb * tintAlpha;
            return  col;
        }
            ENDCG

        }
    }
}