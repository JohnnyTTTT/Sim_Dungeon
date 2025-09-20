Shader "Custom/StencilMaskHDRP"
{
    SubShader
    {
        Tags { "RenderPipeline"="HDRenderPipeline" "RenderType"="Opaque" }
        Pass
        {
            Name "Mask"
            Stencil
            {
                Ref 1
                Comp Always
                Pass Replace
            }
            ColorMask 0
            ZWrite Off
        }
    }
}
