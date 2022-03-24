Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _BumpTex("Normal Textrue", 2D) = "bump" {}
        _SpecTex("SpecularMap", 2D) = "white" {}
        _RampTex("RampTexture", 2D) = "white" {}

        _SpecCol("Specular val", float) = 30
        _SpecColSmooth("Specular Smooth", float) = 0.01

        [HDR]_Color("Color", Color) = (1,1,1,1)
        [HDR]_SpecularColor("SpecularColor", Color) = (1,1,1,1)
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }

            CGPROGRAM
            #pragma surface surf _CustomRamp noambient

            sampler2D _MainTex;
            sampler2D _BumpTex, _SpecTex;
            sampler2D _RampTex;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_BumpTex, uv_SpecTex;
            };

            float4 _Color, _SpecularColor;
            float _SpecCol, _SpecColSmooth;

            void surf(Input IN, inout SurfaceOutput o)
            {
                float4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
                float4 specTex = tex2D(_SpecTex, IN.uv_SpecTex);

                o.Normal = UnpackNormal(tex2D(_BumpTex, IN.uv_BumpTex));
                o.Albedo = mainTex.rgb;
                o.Gloss = specTex.a;
            }

            float4 Lighting_CustomRamp(SurfaceOutput o, float3 lightDir, float3 viewDir, float atten)
            {
                // Diffuse 
                float fDotl = dot(o.Normal, lightDir) * 0.7 + 0.3;
                float fDotV = saturate(dot(o.Normal, viewDir));
                float4 fRamp_Tex = tex2D(_RampTex, float2(fDotl, fDotV));

                // Specular
                float3 fSpecResult;
                float3 fH = normalize(lightDir + viewDir);
                float fHDot = saturate(dot(o.Normal, fH));

                float fLightSmooth = smoothstep(0, _SpecColSmooth, fDotl);
                fHDot = pow(fHDot * fLightSmooth, _SpecCol * o.Gloss);
                if (fHDot > 0.8)
                {
                    fHDot = 1;
                }
                else
                {
                    fHDot = 0;
                }
                fSpecResult = fHDot * _SpecularColor * o.Gloss;

                // Result
                float4 fResult;
                fResult.rgb = (fRamp_Tex * o.Albedo * _LightColor0.rgb * _Color) + fSpecResult;
                fResult.a = o.Alpha;

                return fResult;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
