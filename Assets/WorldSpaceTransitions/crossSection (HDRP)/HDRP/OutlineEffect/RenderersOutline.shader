Shader "CrossSectionHDRP/Renderers/Outline"
{
    Properties
    {
		_SelectionColorFront("Selection Color Front", Color) = (1,1,1,1)
		_SelectionColorBack("Selection Color Back", Color) = (0,1,0,1)
        //_MaxDistance("Max Distance", float) = 15

        // Transparency
        _AlphaCutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    // #pragma enable_d3d11_debug_symbols

    //enable GPU instancing support
    #pragma multi_compile_instancing
    #pragma multi_compile _ DOTS_INSTANCING_ON
	#pragma multi_compile __ CLIP_BOX CLIP_CORNER CLIP_PLANE CLIP_SPHERE_OUT

    ENDHLSL

    SubShader
    {
        
        Pass
        {
            Name "FirstPass"
            Tags { "LightMode" = "FirstPass" }

            Blend Off
            ZWrite On
            ZTest LEqual

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
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT

            // List all the varyings needed in your fragment shader
            //#define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TANGENT_TO_WORLD
            #define VARYINGS_NEED_POSITION_WS
			#define VARYINGS_NEED_CULLFACE

			half _inverse = 0;



            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
			#include "../Shaders/section_clipping_CS.hlsl"

            CBUFFER_START(UnityPerMaterial)
			float4 _SelectionColorFront;
			float4 _SelectionColorBack;
            float _AlphaCutoff;
            CBUFFER_END

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassRenderersV2.hlsl"

            // Put the code to render the objects in your custom pass in this function
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 viewDirection, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
				float3 wpos = GetAbsolutePositionWS(fragInputs.positionRWS);
				float db = LoadCameraDepth(posInput.positionSS);
				float dvd = posInput.deviceDepth;

				#if CLIP_BOX||CLIP_SPHERE_OUT
								SECTION_INTERSECT(wpos);
				#endif
				#if CLIP_CORNER||CLIP_PLANE
								SECTION_CLIP(wpos);
				#endif
								if (dvd < db) discard;

                // Write back the data to the output structures
                ZERO_INITIALIZE(BuiltinData, builtinData); // No call to InitBuiltinData as we don't have any lighting
                ZERO_INITIALIZE(SurfaceData, surfaceData); // No call to InitBuiltinData as we don't have any lighting
                builtinData.opacity = 0;
                builtinData.emissiveColor = float3(0, 0, 0);
                surfaceData.color = fragInputs.isFrontFace? _SelectionColorFront.rgb: _SelectionColorBack.rgb;
                //surfaceData.alpha =0;
            }

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForwardUnlit.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL
        }
    }
}
