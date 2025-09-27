//section_clipping_CS.cginc

#ifndef SECTION_CLIPPING_INCLUDED
// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
//#pragma exclude_renderers d3d11 gles
#define SECTION_CLIPPING_INCLUDED


//Plane clipping definitions

//uniform half _inverse = 0;
//uniform half4 _SectionColor;

#if CLIP_PLANE || CLIP_PIE || CLIP_SPHERE || CLIP_CUBE || CLIP_TUBES || CLIP_TUBE || CLIP_BOX || CLIP_CORNER || CLIP_SPHERE_OUT || CLIP_SPHERES || CLIP_BOXES
	//SECTION_CLIPPING_ENABLED will be defined.
	//This makes it easier to check if this feature is available or not.
	#define SECTION_CLIPPING_ENABLED 1

#if CLIP_PLANE || CLIP_PIE || CLIP_SPHERE || CLIP_TUBE || CLIP_SPHERE_OUT
	uniform float3 _SectionPoint;

	#if CLIP_PLANE || CLIP_PIE
		uniform float _SectionOffset = 0;
		uniform float3 _SectionPlane;
	#endif

	#if CLIP_PIE
	uniform float3 _SectionPlane2;
	#endif
	#if CLIP_SPHERE || CLIP_SPHERE_OUT || CLIP_TUBE
	uniform float _Radius = 0;
	#endif
#endif

#if CLIP_SPHERES || CLIP_PLANE
	//half _inverse;
#endif

#if CLIP_TUBE
		uniform float4 _AxisDir;
#endif

#if CLIP_TUBES
		uniform float4 _AxisDirs[64];
#endif

#if CLIP_BOXES
		uniform float4x4 _WorldToObjectMatrixes[64];
		uniform float4  _SectionScales[64];
		uniform int _boxCount = 0;
#endif

#if CLIP_TUBES || CLIP_SPHERES
		uniform float4 _centerPoints[64];
		uniform float _Radiuses[64];
		uniform int _centerCount = 0;
#endif

#if CLIP_BOX || CLIP_CORNER || CLIP_CUBE
	float4x4 _WorldToObjectMatrix;
#endif

#if RAY_ORIGIN
	uniform float4 _RayOrigin; //debug in editor with colormask != None
#endif

#if CLIP_BOX || CLIP_CUBE
	float4 _SectionScale;
#endif

#if CLIP_BOX || CLIP_CUBE || CLIP_BOXES
	// boxIntersect - ray intersects the box
	// txx - world-to-box transformation
	// ro is the ray origin in world space
	// rd is the ray direction in world space
	// txx is the world-to-box transformation
	// rad is the half-length of the box
	bool boxIntersect(in float3 ro, in float3 rd, in float4x4 txx, in float3 rad)
	{
		float3 rdd = (mul(txx, float4(rd, 0.0))).xyz;
		float3 roo = (mul(txx, float4(ro, 1.0))).xyz;

		float3 m = 1.0 / rdd;
		float3 n = m * roo;
		float3 k = abs(m)*rad;

		float3 t1 = -n - k;
		float3 t2 = -n + k;

		float tN = max(max(t1.x, t1.y), t1.z);
		float tF = min(min(t2.x, t2.y), t2.z);
		//if (tN > tF || (tF < 0.0 || tN>0.0)) return -1.0;
		if (tN > tF || tF < 0.0) return false;
		//return !(tN > 0.0);
		return true;
	}

	// clipBox - point po is outside box
	// txx - world-to-box transformation
	// po - point in world space
	// poo - point in box object space
	bool clipBox(in float3 po, in float4x4 txx, in float3 rad)
	{
		float3 poo = (mul(txx, float4(po, 1.0))).xyz;
		return (abs(poo.x) - rad.x) > 0 || (abs(poo.y) - rad.y) > 0 || (abs(poo.z) - rad.z) > 0;
	}

	#endif
	#if CLIP_CORNER
		bool clipCorner(in float3 po, in float4x4 txx)
		{
			float3 poo = (mul(txx, float4(po, 1.0))).xyz;
			return (poo.x > 0 && poo.y > 0 && poo.z > 0);
		}
	#endif

#if CLIP_PIE
		static const float vcrossY = cross(_SectionPlane, _SectionPlane2).y;
		static const float dotCam = dot(_WorldSpaceCameraPos - _SectionPoint, _SectionPlane);
		static const float dotCam2 = dot(_WorldSpaceCameraPos - _SectionPoint, _SectionPlane2);
#endif

#if	CLIP_SPHERE_OUT
		static const float sphereDist = length(_WorldSpaceCameraPos - _SectionPoint);
		static const float hideRadius = sqrt(sphereDist*sphereDist -_Radius* _Radius);
		static const float3 coneDir = normalize(_WorldSpaceCameraPos - _SectionPoint);
#endif

	//discard drawing of a point in the world if it is behind any one of the planes.
	void Clip(float3 posWorld) {
		bool _clip = false;
		#if CLIP_PIE
		if (vcrossY >= 0) {//<180
			//bool _clip = false;
			_clip = _clip || (-dot((posWorld - _SectionPoint), _SectionPlane) < 0);
			_clip = _clip || (-dot((posWorld - _SectionPoint), _SectionPlane2) < 0);		
		}
		if (vcrossY < 0) {//>180
			_clip = _clip || ((_SectionOffset - dot((posWorld - _SectionPoint), _SectionPlane) < 0) && (-dot((posWorld - _SectionPoint), _SectionPlane2) < 0));
		}
		//#else //
		#endif
		#if CLIP_PLANE
		_clip = _clip || ((_SectionOffset - dot((posWorld - _SectionPoint),_SectionPlane))*(1-2*_inverse)<0);
		#endif
		#if CLIP_SPHERE
		_clip = _clip || (
				(1-2*_inverse)*
				(dot((posWorld - _SectionPoint),(posWorld - _SectionPoint)) - _Radius*_Radius)<0); //_inverse = 1 : negative to clip the outside of the sphere
		#endif

#if CLIP_SPHERE_OUT
		_clip = _clip || ((dot((posWorld - _SectionPoint), (posWorld - _SectionPoint)) - _Radius * _Radius) > 0);
			//bool belowGround = (posWorld.y < 0);
			//_clip = _clip || !belowGround;
#endif
		#if CLIP_CUBE
		_clip = _clip || (!clipBox(posWorld, _WorldToObjectMatrix, 0.5*_SectionScale));
		#endif

#if CLIP_TUBE
		bool _clipTube = ((dot(posWorld - _SectionPoint - _AxisDir * dot(_AxisDir, posWorld - _SectionPoint), posWorld - _SectionPoint - _AxisDir * dot(_AxisDir, posWorld - _SectionPoint)) - _Radius * _Radius) < 0);

		if (_inverse == 0)
		{
			//if(_clip) discard;
			_clip = _clip || _clipTube;
		}
		else
		{
			//if(!_clip) discard;
			_clip = _clip || !_clipTube;
		}
		_clip = _clip || _clipTube;
#endif

#if CLIP_TUBES
		bool _clipTubes = false;
		int _centerCountTruncated = min(_centerCount, 64);
		for (int i = 0; i < _centerCountTruncated; i++)
		{
			_clipTubes = _clipTubes || ((dot(posWorld - _centerPoints[i] - _AxisDirs[i] * dot(_AxisDirs[i], posWorld - _centerPoints[i]), posWorld - _centerPoints[i] - _AxisDirs[i] * dot(_AxisDirs[i], posWorld - _centerPoints[i])) - _Radiuses[i] * _Radiuses[i]) < 0);
		}

		//}
		if (_inverse == 0)
		{
			//if(_clip) discard;
			_clip = _clip || _clipTubes;
		}
		else
		{
			//if(!_clip) discard;
			_clip = _clip || !_clipTubes;
		}
		_clip = _clip || _clipTubes;
#endif



#if CLIP_SPHERES
		bool _clipSpheres = false;
		int _centerCountTruncated = min(_centerCount, 64);
		for (int i = 0; i < _centerCountTruncated; i++)
		{
			_clipSpheres = _clipSpheres || ((dot(posWorld - _centerPoints[i], posWorld - _centerPoints[i]) - _Radiuses[i] * _Radiuses[i]) < 0);
		}

		if (_inverse == 0)
		{
			//if (_clip) discard;
			_clip = _clip || _clipSpheres;
		}
		else
		{
			//if (!_clip) discard;
			_clip = _clip || !_clipSpheres;
		}
#endif

	#if CLIP_BOX
		bool _clipBox = clipBox(posWorld, _WorldToObjectMatrix, 0.5*_SectionScale.xyz);
		_clip = _clip || _clipBox;
	#endif

	#if CLIP_BOXES
		bool _clipBoxes = false;
		int _boxCountTruncated = min(_boxCount, 64);//let's assume 64 as maximum box count expected
		for (int i = 0; i < _boxCountTruncated; i++)
		{
			_clipBoxes = _clipBoxes || !clipBox(posWorld, _WorldToObjectMatrixes[i], 0.5*_SectionScales[i].xyz);
		}
		_clip = _clip || !_clipBoxes;
	#endif

		#if CLIP_CORNER
		bool _clipCorner = clipCorner(posWorld, _WorldToObjectMatrix);
		_clip = _clip || _clipCorner;
		#endif
		if (_clip) discard;
	}
	#if CLIP_BOX || CLIP_SPHERE_OUT || CLIP_PIE //|| CLIP_BOXES
	void Intersect(float3 posWorld) {
		bool _clip = false;
	#if CLIP_BOX
		_clip = !boxIntersect(posWorld, normalize(
		#if RAY_ORIGIN
					_RayOrigin
		#else
					_WorldSpaceCameraPos
		#endif
			- posWorld), _WorldToObjectMatrix, 0.5*_SectionScale.xyz); //|| !clipBox(posWorld, _WorldToObjectMatrix, 0.5*_SectionScale.xyz);
			//if (_inverse == 1) _clip = !_clip;//
	#endif

// no support for capped section for multiple boxes
/*
	#if CLIP_BOXES
			bool _clipBoxes = false;
			int _boxCountTruncated = min(_boxCount, 64);//let's assume 64 as maximum box count expected
			for (int i = 0; i < _boxCountTruncated; i++)
			{
				_clipBoxes = _clipBoxes || boxIntersect(posWorld, normalize(
#if RAY_ORIGIN
					_RayOrigin
#else
					_WorldSpaceCameraPos
#endif
					- posWorld), _WorldToObjectMatrixes[i], 0.5*_SectionScales[i].xyz); // || !clipBox(posWorld, _WorldToObjectMatrixes[i], 0.5*_SectionScales[i].xyz));
			}
			_clip = _clip || !_clipBoxes;
			if (_clip) discard;
	#endif
*/

	#if CLIP_SPHERE_OUT
			bool inCone = false;
			float3 pointToCam = _WorldSpaceCameraPos - posWorld;
			float coneProj = dot(pointToCam, coneDir);
			float coneDist = length(pointToCam - coneProj*coneDir);
			bool inFront = length(posWorld - _WorldSpaceCameraPos) < hideRadius;//
			bool outsideSphere = length(posWorld - _SectionPoint)>_Radius;
			inCone = (coneDist / coneProj < _Radius / hideRadius);
			_clip = !inCone || inFront && outsideSphere;
			//bool belowGround = (posWorld.y < 0);
			//_clip = _clip && !belowGround;
			//_clip = inFront;
	#endif
#if CLIP_PIE
			float dotProd = dot(posWorld - _SectionPoint, _SectionPlane);
			float dotProd2 = dot(posWorld - _SectionPoint, _SectionPlane2);
			if (vcrossY >= 0) 
			{
				_clip = (dotProd > 0 && dotCam > 0) || (dotProd2 > 0 && dotCam2 > 0);
			}
			else 
			{
				_clip = dotProd > 0 && dotProd2 > 0;
			}
#endif

		if(_clip) discard;
	}

	#define SECTION_INTERSECT(posWorld) Intersect(posWorld); //preprocessor macro that will produce an empty block if no clipping planes are used.
	#endif

//preprocessor macro that will produce an empty block if no clipping planes are used.
#define SECTION_CLIP(posWorld) Clip(posWorld);
    
#else
//empty definitionS
#define SECTION_CLIP(s)
#define SECTION_INTERSECT(s) 
//
#endif


#endif // SECTION_CLIPPING_INCLUDED