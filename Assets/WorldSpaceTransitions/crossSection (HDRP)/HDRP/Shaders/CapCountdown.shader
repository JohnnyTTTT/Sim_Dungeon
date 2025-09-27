Shader "CrossSectionHDRP/Renderers/CapCountdown"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,0.0039)
        //_ColorMap("ColorMap", 2D) = "white" {}
        // Transparency
        //_AlphaCutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        [Toggle(INVERSE)] _inverse("inverse", Float) = 0
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    // #pragma enable_d3d11_debug_symbols

    //enable GPU instancing support
    #pragma multi_compile __ CLIP_BOX CLIP_CORNER CLIP_PLANE CLIP_SPHERE_OUT
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
    #pragma shader_feature_local INVERSE

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "FirstPass"
            Tags { "LightMode" = "FirstPass" }

            Blend SrcAlpha One
            ZWrite Off
            ZTest Always

            Cull Off

            HLSLPROGRAM

            // Toggle the alpha test
            #define _ALPHATEST_ON

            // Toggle transparency
            // #define _SURFACE_TYPE_TRANSPARENT

            // Toggle fog on transparent
            //#define _ENABLE_FOG_ON_TRANSPARENT
            
            // List all the attributes needed in your shader (will be passed to the vertex shader)
            // you can see the complete list of these attributes in VaryingMesh.hlsl
            //#define ATTRIBUTES_NEED_TEXCOORD0

            // List all the varyings needed in your fragment shader
            //#define VARYINGS_NEED_TEXCOORD0

                // CrossSection
    #define VARYINGS_NEED_POSITION_WS
    #define VARYINGS_NEED_CULLFACE

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            
            //TEXTURE2D(_ColorMap);

            // Declare properties in the UnityPerMaterial cbuffer to make the shader compatible with SRP Batcher.
CBUFFER_START(UnityPerMaterial)
            //float4 _ColorMap_ST;
            float4 _Color;
            float _sectionIndex;
            //float _AlphaCutoff;
            float _inverse;
CBUFFER_END

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassRenderersV2.hlsl"
            #include "section_clipping_CS.hlsl"//CrossSection

            // If you need to modify the vertex datas, you can uncomment this code
            // Note: all the transformations here are done in object space
            // #define HAVE_MESH_MODIFICATION
            // AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
            // {
            //     input.positionOS += input.normalOS * 0.0001; // inflate a bit the mesh to avoid z-fight
            //     return input;
            // }

            // Put the code to render the objects in your custom pass in this function
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 viewDirection, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
                //float2 colorMapUv = TRANSFORM_TEX(fragInputs.texCoord0.xy, _ColorMap);
                //float4 result = SAMPLE_TEXTURE2D(_ColorMap, s_trilinear_clamp_sampler, colorMapUv) * _Color;
                //float opacity = result.a;
                //float3 color = result.rgb;

#ifdef _ALPHATEST_ON
                //DoAlphaTest(opacity, _AlphaCutoff);
#endif
#ifdef SECTION_CLIPPING_ENABLED //CrossSection
                float3 wpos = GetAbsolutePositionWS(fragInputs.positionRWS);//CrossSection
                #if defined CLIP_BOX||CLIP_SPHERE_OUT||CLIP_PIE
                SECTION_INTERSECT(wpos);//CrossSection
                #else
                SECTION_CLIP(wpos);//CrossSection
                #endif
#endif//CrossSection

                // Write back the data to the output structures
                ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
                ZERO_INITIALIZE(SurfaceData, surfaceData);
                builtinData.opacity = _Color.a;
                builtinData.emissiveColor = float3(0, 0, 0);
                surfaceData.color = (fragInputs.isFrontFace? float3(1,0,0) : float3(0,1,0));
            }

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForwardUnlit.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL
        }
    }
}
