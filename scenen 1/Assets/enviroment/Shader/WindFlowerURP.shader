Shader "Custom/WindFlowerURP"
{
    Properties
    {
        _BaseMap("Texture (Ảnh màu)", 2D) = "white" {}
        _BaseColor("Màu sắc đè", Color) = (1, 1, 1, 1)
        
        [Header(Wind Settings)]
        _WindSpeed("Tốc độ gió", Range(0, 10)) = 2.0
        _WindStrength("Độ mạnh của gió", Range(0, 1)) = 0.1
    }
    SubShader
    {
        // Khai báo cho Unity biết đây là Shader của hệ thống URP
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _WindSpeed;
                float _WindStrength;
            CBUFFER_END

            // HÀM CAN THIỆP LƯỚI 3D TẠO GIÓ
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                // Toán học tạo gió: Bẻ cong các điểm Y > 0
                float heightFactor = max(0, IN.positionOS.y);
                float wave = sin(_Time.y * _WindSpeed + IN.positionOS.x + IN.positionOS.z);
                
                IN.positionOS.x += wave * _WindStrength * heightFactor;
                IN.positionOS.z += wave * (_WindStrength * 0.5) * heightFactor;

                // Xuất tọa độ ra màn hình
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            // HÀM TÔ MÀU (Unlit - Không tính toán ánh sáng để siêu nhẹ máy)
            half4 frag(Varyings IN) : SV_Target
            {
                half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                return color;
            }
            ENDHLSL
        }
    }
}