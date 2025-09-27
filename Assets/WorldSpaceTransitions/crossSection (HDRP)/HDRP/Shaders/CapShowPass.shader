Shader "CrossSectionHDRP/Renderers/CapShowPass"
{
    Properties
    {
		_UnlitColor("Color", Color) = (1,1,1,1)
        _UnlitColorMap("ColorMap", 2D) = "white" {}

		//[Enum(UV0, 0, UV1, 1, UV2, 2, UV3, 3, Planar, 4, Triplanar, 5)] _UVBase("UV Set for base", Float) = 0
		[Enum(UV0, 0, Triplanar, 1, TriplanarNoBlending, 2)] _MappingMode("UV mode", Float) = 0
		_TexWorldScale("Scale to apply on world coordinate", Float) = 1.0
		[HideInInspector] _InvTilingScale("Inverse tiling scale = 2 / (abs(_BaseColorMap_ST.x) + abs(_BaseColorMap_ST.y))", Float) = 1

        // Transparency
        _AlphaCutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendSrc("Blend mode Source", Int) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _BlendDst("Blend mode Destination", Int) = 10
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone vulkan metal switch

    // #pragma enable_d3d11_debug_symbols

    //enable GPU instancing support
    //#pragma multi_compile_instancing

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "FirstPass"
            Tags { "LightMode" = "FirstPass" }

			Blend[_BlendSrc][_BlendDst]
            ZWrite Off
            ZTest LEqual
            Cull Back

            HLSLPROGRAM

            // Toggle the alpha test
            #define _ALPHATEST_ON

			//#define _BLENDMODE_ALPHA 
			
			//#define _BLENDMODE_ADD 
			//_BLENDMODE_PRE_MULTIPLY
			#pragma shader_feature_local _ _BLENDMODE_ALPHA _BLENDMODE_ADD _BLENDMODE_PRE_MULTIPLY

            // Toggle transparency
            #define _SURFACE_TYPE_TRANSPARENT

            // Toggle fog on transparent
            #define _ENABLE_FOG_ON_TRANSPARENT
            
            // List all the attributes needed in your shader (will be passed to the vertex shader)
            // you can see the complete list of these attributes in VaryingMesh.hlsl
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT

            // List all the varyings needed in your fragment shader
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TANGENT_TO_WORLD
			#define VARYINGS_NEED_POSITION_WS
			            
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassRenderers.hlsl"

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Sampling/SampleUVMapping.hlsl"

			#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/BuiltinUtilities.hlsl"

            //TEXTURE2D(_ColorMap);
            //float4 _ColorMap_ST;
            //float4 _Color;

			float _TexWorldScale;
			float _MappingMode;

			real3 TriplanarSharpWeights(real3 normal)
			{
				real3 absNorm = abs(normal.xyz);
				real a = max(absNorm.x, max(absNorm.y, absNorm.z));
				real3 output = real3((a == absNorm.x) ? 1 : 0, (a == absNorm.y) ? 1 : 0, (a == absNorm.z) ? 1 : 0);
				return output;
			}

            // Put the code to render the objects in your custom pass in this function
            void GetSurfaceAndBuiltinData(FragInputs fragInputs, float3 viewDirection, inout PositionInputs posInput, out SurfaceData surfaceData, out BuiltinData builtinData)
            {
				float3 color = float3(1,1,1);
				float opacity = 1;
				if (_MappingMode == 0)
				{
					float2 unlitColorMapUv = TRANSFORM_TEX(fragInputs.texCoord0.xy, _UnlitColorMap);
					color = SAMPLE_TEXTURE2D(_UnlitColorMap, sampler_UnlitColorMap, unlitColorMapUv).rgb * _UnlitColor.rgb;
					opacity = SAMPLE_TEXTURE2D(_UnlitColorMap, sampler_UnlitColorMap, unlitColorMapUv).a * _UnlitColor.a;
				}
				//float Blend = 100;
				//float3 Node_Blend = pow(abs(input.tangentToWorld[2].xyz), Blend);
				//float3 Node_Blend /= dot(Node_Blend, 1.0);
				else
				{
					float3 Node_Blend = (_MappingMode == 2) ? TriplanarSharpWeights(fragInputs.tangentToWorld[2].xyz) : ComputeTriplanarWeights(fragInputs.tangentToWorld[2].xyz);

					float3 wpos = GetAbsolutePositionWS(fragInputs.positionRWS);

					float2 unlitColorMapUvx = _TexWorldScale * TRANSFORM_TEX(wpos.zy, _UnlitColorMap);
					float2 unlitColorMapUvy = _TexWorldScale * TRANSFORM_TEX(wpos.xz, _UnlitColorMap);
					float2 unlitColorMapUvz = _TexWorldScale * TRANSFORM_TEX(wpos.xy, _UnlitColorMap);

					color = (SAMPLE_TEXTURE2D(_UnlitColorMap, s_trilinear_repeat_sampler, unlitColorMapUvx).rgb*Node_Blend.x + SAMPLE_TEXTURE2D(_UnlitColorMap, s_trilinear_repeat_sampler, unlitColorMapUvy).rgb*Node_Blend.y +
						SAMPLE_TEXTURE2D(_UnlitColorMap, s_trilinear_repeat_sampler, unlitColorMapUvz).rgb*Node_Blend.z)* _UnlitColor.rgb;


					opacity = (SAMPLE_TEXTURE2D(_UnlitColorMap, s_trilinear_repeat_sampler, unlitColorMapUvx).a*Node_Blend.x + SAMPLE_TEXTURE2D(_UnlitColorMap, s_trilinear_repeat_sampler, unlitColorMapUvy).a*Node_Blend.y
						+ SAMPLE_TEXTURE2D(_UnlitColorMap, s_trilinear_repeat_sampler, unlitColorMapUvz).a*Node_Blend.z)* _UnlitColor.a;
				}


#ifdef _ALPHATEST_ON
				GENERIC_ALPHA_TEST(opacity, _AlphaCutoff);
                //DoAlphaTest(opacity, _AlphaCutoff);
#endif
				float3 customColor =  SampleCustomColor(posInput.positionNDC);
				if(customColor.r >= customColor.g) discard;

                // Write back the data to the output structures
                ZERO_INITIALIZE(BuiltinData, builtinData); // No call to InitBuiltinData as we don't have any lighting
				ZERO_INITIALIZE(SurfaceData, surfaceData);
                builtinData.opacity = opacity;
                builtinData.emissiveColor = float3(0, 0, 0);
                surfaceData.color = color;
            }

            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/ShaderPassForwardUnlit.hlsl"

            #pragma vertex Vert
            #pragma fragment Frag

            ENDHLSL
        }
    }
}
