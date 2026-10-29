Shader "GpuUnitTest/Units"
{
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" "RenderType" = "Opaque" }
        Pass
        {
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Cull Off
            ZWrite On

            HLSLPROGRAM
            #pragma target 4.5
            #pragma vertex Vert
            #pragma fragment Frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "GpuUnitData.hlsl"

            StructuredBuffer<UnitData> _Units;
            float4 _Origin;
            float4 _Color;

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings Vert(uint vertexID : SV_VertexID, uint instanceID : SV_InstanceID)
            {
                const float2 corners[6] = {
                    float2(-1, -1), float2(1, -1), float2(1, 1),
                    float2(-1, -1), float2(1, 1), float2(-1, 1)
                };
                UnitData unit = _Units[instanceID];
                float2 position = unit.position + corners[vertexID] * unit.size;
                Varyings output;
                output.positionCS = TransformWorldToHClip(_Origin.xyz + float3(position, 0));
                return output;
            }

            half4 Frag(Varyings input) : SV_Target
            {
                return _Color;
            }
            ENDHLSL
        }
    }
}
