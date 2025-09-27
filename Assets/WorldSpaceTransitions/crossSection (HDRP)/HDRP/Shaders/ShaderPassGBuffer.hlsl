#if SHADERPASS != SHADERPASS_GBUFFER
#error SHADERPASS_is_not_correctly_define
#endif

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/VertMesh.hlsl"

PackedVaryingsType Vert(AttributesMesh inputMesh)
{
    VaryingsType varyingsType;
	
#if defined(HAVE_RECURSIVE_RENDERING)
    // If we have a recursive raytrace object, we will not render it.
    // As we don't want to rely on renderqueue to exclude the object from the list,
    // we cull it by settings position to NaN value.
    // TODO: provide a solution to filter dyanmically recursive raytrace object in the DrawRenderer
    if (_EnableRecursiveRayTracing && _RayTracing > 0.0)
    {
        ZERO_INITIALIZE(VaryingsType, varyingsType); // Divide by 0 should produce a NaN and thus cull the primitive.
    }
    else
#endif
    {	
		varyingsType.vmesh = VertMesh(inputMesh);
    }

    return PackVaryingsType(varyingsType);
}

#ifdef TESSELLATION_ON

PackedVaryingsToPS VertTesselation(VaryingsToDS input)
{
    VaryingsToPS output;
    output.vmesh = VertMeshTesselation(input.vmesh);
    return PackVaryingsToPS(output);
}

#include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/ShaderPass/TessellationShare.hlsl"

#include "section_clipping_CS.hlsl"//CrossSection
#endif // TESSELLATION_ON


void Frag(  PackedVaryingsToPS packedInput,
            OUTPUT_GBUFFER(outGBuffer)
            #ifdef _DEPTHOFFSET_ON
            , out float outputDepth : DEPTH_OFFSET_SEMANTIC
            #endif
            )
{
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(packedInput);
    FragInputs input = UnpackVaryingsToFragInputs(packedInput);

    #ifdef SECTION_CLIPPING_ENABLED //CrossSection
    float3 wpos = GetAbsolutePositionWS(input.positionRWS);//CrossSection
    SECTION_CLIP(wpos);//CrossSection
    #endif//CrossSection

    // input.positionSS is SV_Position
    PositionInputs posInput = GetPositionInput(input.positionSS.xy, _ScreenSize.zw, input.positionSS.z, input.positionSS.w, input.positionRWS);

#ifdef VARYINGS_NEED_POSITION_WS
    float3 V = GetWorldSpaceNormalizeViewDir(input.positionRWS);
#else
    // Unused
    float3 V = float3(1.0, 1.0, 1.0); // Avoid the division by 0
#endif

    SurfaceData surfaceData;
    BuiltinData builtinData;

//CrossSection
//SurfaceData definitions
//\Library\PackageCache\com.unity.render - pipelines.high - definition@12.1.0\Runtime\Material\Lit\Lit.cs.hlsl(78) :struct SurfaceData
//\Library\PackageCache\com.unity.render - pipelines.high - definition@12.1.0\Runtime\Material\Builtin\BuiltinData.cs.hlsl(29) :struct BuiltinData
# if USE_SECTION_COLOR && _DOUBLESIDED_ON
	if (input.isFrontFace)
    {
#endif
        GetSurfaceAndBuiltinData(input, V, posInput, surfaceData, builtinData);
# if USE_SECTION_COLOR && _DOUBLESIDED_ON
    }
    else
    {
		ZERO_BUILTIN_INITIALIZE(builtinData); // No call to InitBuiltinData as we don't have any lighting
		ZERO_INITIALIZE(SurfaceData, surfaceData);
		builtinData.emissiveColor = _SectionColor.rgb;
        builtinData.bakeDiffuseLighting = real3(0,0,0);
        builtinData.backBakeDiffuseLighting = real3(0,0,0);
        builtinData.shadowMask0 = 0;
        builtinData.shadowMask1 = 0;
        builtinData.shadowMask2 = 0;
        builtinData.shadowMask3 = 0;
        //real2 motionVector;
        //real2 distortion;
        //real distortionBlur;
        //builtinData.isLightmap = 0;
        builtinData.renderingLayers = -1;
        //surfaceData.materialFeatures = 1;
		surfaceData.normalWS = real3(-1,0,0);
		surfaceData.tangentWS = real3(-1,0,0);    
        surfaceData.baseColor = _SectionColor.rgb;
        surfaceData.specularOcclusion = 1;
        surfaceData.perceptualSmoothness = 0;
        surfaceData.ambientOcclusion = 1;
        surfaceData.metallic = 1;
	}
#endif
//CrossSection

    ENCODE_INTO_GBUFFER(surfaceData, builtinData, posInput.positionSS, outGBuffer);

#ifdef _DEPTHOFFSET_ON
    outputDepth = posInput.deviceDepth;
#endif

}
