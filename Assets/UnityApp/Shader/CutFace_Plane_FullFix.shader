Shader "Custom/CutFace_Plane_FullFix"
{
    Properties
    {
        _CutFaceColor("Cut Face Color", Color) = (1,0,0,1)
        _CutHeight("Cut Height", Float) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Back

        Pass
        {
            Name "CutFacePass"

            CGPROGRAM
            #pragma vertex Vert
            #pragma geometry Geom
            #pragma fragment Frag
            #include "UnityCG.cginc"

            float4 _CutFaceColor;
            float _CutHeight;

            struct appdata
            {
                float3 vertex : POSITION;
            };

            // Vertex Shader 输出结构
            struct v2g
            {
                float4 pos : SV_POSITION;   // 必须写
                float3 posWS : TEXCOORD0;   // 世界坐标传给 GS
            };

            struct g2f
            {
                float4 pos : SV_POSITION;   // 必须写
                float3 normalWS : NORMAL;
            };

            // Vertex Shader
            v2g Vert(appdata IN)
            {
                v2g OUT;
                float4 posWorld = mul(unity_ObjectToWorld, float4(IN.vertex,1));
                OUT.posWS = posWorld.xyz;
                OUT.pos = mul(UNITY_MATRIX_VP, posWorld); // SV_POSITION 必须写
                return OUT;
            }

            // Geometry Shader: 生成平面
            [maxvertexcount(6)]
            void Geom(triangle v2g input[3], inout TriangleStream<g2f> triStream)
            {
                float3 normal = float3(0,1,0);
                float halfSize = 0.5; // 可根据模型 Bounds 调整

                float3 p0 = float3(-halfSize, _CutHeight, -halfSize);
                float3 p1 = float3( halfSize, _CutHeight, -halfSize);
                float3 p2 = float3( halfSize, _CutHeight,  halfSize);
                float3 p3 = float3(-halfSize, _CutHeight,  halfSize);

                // 三角形 1
                g2f v0; v0.pos = mul(UNITY_MATRIX_VP, float4(p0,1)); v0.normalWS = normal; triStream.Append(v0);
                g2f v1; v1.pos = mul(UNITY_MATRIX_VP, float4(p1,1)); v1.normalWS = normal; triStream.Append(v1);
                g2f v2; v2.pos = mul(UNITY_MATRIX_VP, float4(p2,1)); v2.normalWS = normal; triStream.Append(v2);

                triStream.RestartStrip();

                // 三角形 2
                g2f v3; v3.pos = mul(UNITY_MATRIX_VP, float4(p2,1)); v3.normalWS = normal; triStream.Append(v3);
                g2f v4; v4.pos = mul(UNITY_MATRIX_VP, float4(p3,1)); v4.normalWS = normal; triStream.Append(v4);
                g2f v5; v5.pos = mul(UNITY_MATRIX_VP, float4(p0,1)); v5.normalWS = normal; triStream.Append(v5);
            }

            // Fragment Shader
            fixed4 Frag(g2f IN) : SV_Target
            {
                return _CutFaceColor;
            }

            ENDCG
        }
    }
}
