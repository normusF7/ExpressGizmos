Shader "ExpressGizmos/Line"
{
    Properties
    {
        _LineWidth ("Line Width", Float) = 3
        _Color ("Line Color", Color) = (1,1,1,1)
        _DistanceScale ("Distance Scale", Float) = 1.0
        _MinWidth ("Minimum Width", Float) = 3
    }

    SubShader
    {
        Tags {"RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" "Queue"="Overlay"}

        Pass
        {
            Name "ForwardLit"
            Tags {"LightMode" = "UniversalForward"}

            ZWrite On
            ZTest LEqual
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag

            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float fogFactor : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float _LineWidth;
                float4 _Color;
                float _DistanceScale;
                float _MinWidth;
            CBUFFER_END

            #define WIDTH_SCALE 0.001

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.color = _Color;
                output.fogFactor = ComputeFogFactor(output.positionCS.z);
                return output;
            }

            float CalculateDistanceScale(float4 positionCS)
            {
                float scaledLineWidth = _LineWidth * WIDTH_SCALE;
                float scaledMinWidth = _MinWidth * WIDTH_SCALE;
                float distanceScale = 1.0 / (positionCS.w * _DistanceScale);
                return max(distanceScale, scaledMinWidth / scaledLineWidth);
            }

            [maxvertexcount(4)]
            void geom(line Varyings input[2], inout TriangleStream<Varyings> outStream)
            {
                float4 p0 = input[0].positionCS;
                float4 p1 = input[1].positionCS;

                float2 dir = normalize(p1.xy/p1.w - p0.xy/p0.w);
                float2 normal = float2(-dir.y, dir.x);

                float w0 = p0.w;
                float w1 = p1.w;

                float distanceScale0 = CalculateDistanceScale(p0);
                float distanceScale1 = CalculateDistanceScale(p1);

                float scaledLineWidth = _LineWidth * WIDTH_SCALE;
                float2 offset0 = normal * scaledLineWidth * 0.5 * distanceScale0;
                float2 offset1 = normal * scaledLineWidth * 0.5 * distanceScale1;

                Varyings v[4];

                v[0].positionCS = float4(p0.xy + offset0 * w0, p0.zw);
                v[1].positionCS = float4(p0.xy - offset0 * w0, p0.zw);
                v[2].positionCS = float4(p1.xy + offset1 * w1, p1.zw);
                v[3].positionCS = float4(p1.xy - offset1 * w1, p1.zw);

                [unroll]
                for (int i = 0; i < 4; ++i)
                {
                    v[i].color = input[0].color;
                    v[i].fogFactor = input[0].fogFactor;
                    outStream.Append(v[i]);
                }
            }

            float4 frag(Varyings input) : SV_Target
            {
                float4 color = input.color;
                color.rgb = MixFog(color.rgb, input.fogFactor);
                return color;
            }
            ENDHLSL
        }
    }
}