Shader "Custom/WallStencilHDRP"
{
    SubShader
    {
        Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" }
        Pass
        {
            Name "Wall"
            Stencil
            {
                Ref 1
                Comp NotEqual   // 只在 stencil ≠ 1 的区域绘制
                Pass Keep
            }

            // 用 Unity 默认的光照管线，不写任何 HLSL
        }
    }
}
