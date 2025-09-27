Shader "CrossSectionHDRP/Fullscreen/Outline"
{
    properties
    {
        _outlineThickness ("Outline Width", Float ) = 1
        
        _OuterColor( "Outline Color", Color ) = (1, 1, 0, 1)
		_BackfaceOutlineColor("Backface Outline Color", Color) = (1, 1, 0, 1)
		_depthSensitivity("depthSensitivity", Range(0.0, 1.0)) = 0.0
		_normalsSensitivity("normalsSensitivity", Range(0.0, 10.0)) = 0.0
		_colorSensitivity("colorSensitivity", Range(0.0, 10.0)) = 0.0
		_maskSensitivity("maskSensitivity", Range(0.0, 10.0)) = 0.0
		_backfaceSensitivity("backfaceSensitivity", Range(0.0, 10.0)) = 1
        
        _BehindFactor("Behind Factor", Range(0,1)) = 0.2
		[Toggle(ALL_EDGES)] _all_edges("all edges", Float) = 1
    }

    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 4.5
    #pragma d3d11 playstation xboxone xboxseries vulkan metal switch
	#pragma shader_feature ALL_EDGES

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"
	#include "Packages/com.unity.render-pipelines.high-definition/Runtime/Material/NormalBuffer.hlsl"

    // The PositionInputs struct allow you to retrieve a lot of useful information for your fullScreenShader:
    // struct PositionInputs
    // {
    //     float3 positionWS;  // World space position (could be camera-relative)
    //     float2 positionNDC; // Normalized screen coordinates within the viewport    : [0, 1) (with the half-pixel offset)
    //     uint2  positionSS;  // Screen space pixel coordinates                       : [0, NumPixels)
    //     uint2  tileCoord;   // Screen tile coordinates                              : [0, NumTiles)
    //     float  deviceDepth; // Depth from the depth buffer                          : [0, 1] (typically reversed)
    //     float  linearDepth; // View space Z coordinate                              : [Near, Far]
    // };

    // To sample custom buffers, you have access to these functions:
    // But be careful, on most platforms you can't sample to the bound color buffer. It means that you
    // can't use the SampleCustomColor when the pass color buffer is set to custom (and same for camera the buffer).
    // float3 SampleCustomColor(float2 uv);
    // float3 LoadCustomColor(uint2 pixelCoords);
    // float LoadCustomDepth(uint2 pixelCoords);
    // float SampleCustomDepth(float2 uv);

    // There are also a lot of utility function you can use inside Common.hlsl and Color.hlsl,
    // you can check them out in the source code of the core SRP package.
      

    float _outlineThickness;
    
    float4 _OuterColor;
        
    float _BehindFactor;

	float _depthSensitivity;
	float _normalsSensitivity;
	float _colorSensitivity;
	float _maskSensitivity;
	float _backfaceSensitivity; 
	float4 _BackfaceOutlineColor;

	float SampleClampedDepth(float2 uv) { return SampleCameraDepth(clamp(uv, _ScreenSize.zw, 1 - _ScreenSize.zw)).r; }

    float4 FullScreenPass(Varyings varyings) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);

        float depth = LoadCameraDepth(varyings.positionCS.xy);
        PositionInputs posInput = GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
        //float3 viewDirection = GetWorldSpaceNormalizeViewDir(posInput.positionWS);
        
        
		float2 uvOffsetPerPixel = 1.0 / _ScreenSize.xy;
       
		float halfScaleFloor = floor(_outlineThickness * 0.5);
		float halfScaleCeil = ceil(_outlineThickness * 0.5);
		float2 uvSamples[4];
#if defined(ALL_EDGES)
		float depthSamples[4], maskSamples[4];
		NormalData normalData[4];
		float3	colorSamples[4];
#endif	
		float backfaceSamples[4];

		uvSamples[0] = posInput.positionNDC - float2(uvOffsetPerPixel.x, uvOffsetPerPixel.y) * halfScaleFloor;
		uvSamples[1] = posInput.positionNDC + float2(uvOffsetPerPixel.x, uvOffsetPerPixel.y) * halfScaleCeil;
		uvSamples[2] = posInput.positionNDC + float2(uvOffsetPerPixel.x * halfScaleCeil, -uvOffsetPerPixel.y * halfScaleFloor);
		uvSamples[3] = posInput.positionNDC + float2(-uvOffsetPerPixel.x * halfScaleFloor, uvOffsetPerPixel.y * halfScaleCeil);
		for (uint i = 0; i < 4; i++)
		{
			backfaceSamples[i] = SampleCustomColor(uvSamples[i]).g;
#if defined(ALL_EDGES)
			maskSamples[i] = SampleCustomColor(uvSamples[i]).r;
			//colorSamples[i] = SampleCameraColor(uvSamples[i]);
			colorSamples[i] = float4(CustomPassSampleCameraColor(uvSamples[i], 0), 1);
			DecodeFromNormalBuffer(_ScreenSize.xy * uvSamples[i], normalData[i]);
			depthSamples[i] = SampleClampedDepth(uvSamples[i]);
			//depthSamples[i] = LoadCameraDepth(_ScreenSize.xy * uvSamples[i]);
#endif
		}
#if defined(ALL_EDGES)
		// mask
		float maskFiniteDifference0 = maskSamples[1] - maskSamples[0];
		float maskFiniteDifference1 = maskSamples[3] - maskSamples[2];
		float edgeMask = sqrt(pow(maskFiniteDifference0, 2) + pow(maskFiniteDifference1, 2)) * 10;
		float maskThreshold = (1 / _maskSensitivity) * maskSamples[0];
		edgeMask = edgeMask > maskThreshold ? 1 : 0;

		float edgeNormal = 0;
		float edgeDepth = 0;
		float edgeColor = 0;

		if (SampleCustomColor(posInput.positionNDC).r > 0.5)
		{
			// Normals
			float3 normalFiniteDifference0 = normalData[1].normalWS - normalData[0].normalWS;
			float3 normalFiniteDifference1 = normalData[3].normalWS - normalData[2].normalWS;
			edgeNormal = sqrt(dot(normalFiniteDifference0, normalFiniteDifference0) + dot(normalFiniteDifference1, normalFiniteDifference1));
			edgeNormal = edgeNormal > (1 / _normalsSensitivity) ? 1 : 0;
			// Depth
			float depthFiniteDifference0 = depthSamples[1] - depthSamples[0];
			float depthFiniteDifference1 = depthSamples[3] - depthSamples[2];
			edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 1000;
			float depthThreshold = (1 / _depthSensitivity);
			edgeDepth = edgeDepth > depthThreshold ? 1 : 0;
			// Color
			float3 colorFiniteDifference0 = colorSamples[1] - colorSamples[0];
			float3 colorFiniteDifference1 = colorSamples[3] - colorSamples[2];
			edgeColor = sqrt(dot(colorFiniteDifference0, colorFiniteDifference0) + dot(colorFiniteDifference1, colorFiniteDifference1));
			edgeColor = edgeColor > (1/_colorSensitivity) ? 1 : 0;
		}

#endif
		// backfaceMask
		float backfaceMaskFiniteDifference0 = backfaceSamples[1] - backfaceSamples[0];
		float backfaceMaskFiniteDifference1 = backfaceSamples[3] - backfaceSamples[2];
		float edgeBackface = sqrt(pow(backfaceMaskFiniteDifference0, 2) + pow(backfaceMaskFiniteDifference1, 2)) * 10;
		float edgeBackfaceThreshold = (1 / _backfaceSensitivity) * backfaceSamples[0];
		edgeBackface = edgeBackface > edgeBackfaceThreshold ? 1 : 0;

#if !defined(ALL_EDGES)
		float edge = edgeBackface;
		half4 Color = _BackfaceOutlineColor;
#endif
#if defined(ALL_EDGES)
		float edge = max(edgeMask, max(edgeDepth, max(edgeNormal, edgeColor)));
		half4 Color = edgeBackface > 0.1*edge ? _BackfaceOutlineColor : _OuterColor;		
		edge = max(edge, edgeBackface);
#endif

        float4 o = float4(0,0,0,0);

		o = edge * lerp(o, Color, Color.a);
        
        return o;
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "Custom Pass 0"

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
                #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
