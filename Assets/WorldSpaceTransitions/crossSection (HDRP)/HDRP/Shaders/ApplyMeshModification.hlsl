#define HAVE_MESH_MODIFICATION

uniform half _BackfaceExtrusion;

AttributesMesh ApplyMeshModification(AttributesMesh input, float3 timeParameters)
{

	float3 worldPos = TransformObjectToWorld(input.positionOS);
	float3 worldNorm = TransformObjectToWorldDir(input.normalOS);
	float frontface = dot(-worldNorm, worldPos);
	if (frontface < 0)
	{
		float3 incr = worldNorm * _BackfaceExtrusion;
		//worldPos -= incr;
		//input.positionOS = TransformWorldToObject(worldPos);
		float3 objIncr = TransformWorldToObject(incr) - TransformWorldToObject(float3(0,0,0));
		input.positionOS -= objIncr;
	}
	//input.positionOS += input.normalOS * 0.01; // inflate a bit the mesh to avoid z-fight
	return input;
}