Shader "SevenMasters/FakeWorldCloud" 
{
	Properties 
	{
		[Header(Standard)]
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _MetallicGlossMap("Metalic", 2D) = "gray" {}
		_GlossMapScale("Smoothness", Range(0,1)) = 0.5
		[Toggle(ALBEDO_SMOOTHNESS)] _SmoothnessTextureChannel("Smoothness source metallic (unchecked) albedo (checked)", Float) = 0
		[NoScaleOffset][Normal] _BumpMap("Normal", 2D) = "white" {}
		_BumpScale("Normal scale", Range(0,1)) = 1.0
		[NoScaleOffset] _OcclusionMap("Ambient Occlusion", 2D) = "white" {}
		_OcclusionStrength("Occlusion", Range(0,1)) = 1.0
		[NoScaleOffset]_EmissionMap("Emission (RGB)", 2D) = "white" {}
		[HDR] _EmissionColor("Emission", Color) = (0,0,0)

		[Header(FakeWorldCloud)]
		_Cloud("Cloud speed (xz) Cloud position (y) Cloud scale (w)", Vector) = (0.03333,10,0.019,50)
		_CloudRamp("Cloud ramp (RGB)", 2D) = "white" {}
		_CloudNoise("Height noise", 2D) = "black" {}
		_CloudNoiseScale("Height noise scale", Range(0, 100)) = 12
	}
	SubShader 
	{
		Tags 
		{
			"RenderType"="Opaque"
		}
		LOD 500

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Cloud noforwardadd nofog vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		#pragma shader_feature ALBEDO_SMOOTHNESS
		#pragma multi_compile_fog  

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		sampler2D _CloudNoise;
		float4 _CloudNoise_ST;
		float _CloudNoiseScale;
		float4 _Cloud;

		float CloudBlend(float3 worldPos)
		{
			float fadeThreshold = _Cloud.y - tex2D(_CloudNoise, TRANSFORM_TEX(worldPos.xz, _CloudNoise) + (_Time.y * _Cloud.xz)) * _CloudNoiseScale;
			if (worldPos.y < fadeThreshold)
			{
				float distanceY = fadeThreshold - worldPos.y;
				float ratio = distanceY / _Cloud.w;
				return saturate(pow(min(1, ratio), 2));
			}
			return 0;
		}

		struct SurfaceOutputStandardWorld
		{
			fixed3 Albedo;      // base (diffuse or specular) color
			fixed3 Cloud;
			fixed3 Normal;      // tangent space normal, if written
			half3 Emission;
			half Metallic;      // 0=non-metal, 1=metal
								// Smoothness is the user facing name, it should be perceptual smoothness but user should not have to deal with it.
								// Everywhere in the code you meet smoothness it is perceptual smoothness
			half Smoothness;    // 0=rough, 1=smooth
			half Occlusion;     // occlusion (default 1)
			fixed Alpha;        // alpha for transparencies
			float3 worldPos;
			half fogFactor;
		};

		#include "UnityPBSLighting.cginc"

		SurfaceOutputStandard ConvertOutput(SurfaceOutputStandardWorld sosw)
		{
			SurfaceOutputStandard sos;
			sos.Albedo = sosw.Albedo;
			sos.Normal = sosw.Normal;
			sos.Emission = sosw.Emission;
			sos.Metallic = sosw.Metallic;
			sos.Smoothness = sosw.Smoothness;
			sos.Occlusion = sosw.Occlusion;
			sos.Alpha = sosw.Alpha;
			return sos;
		}

		half4 LightingCloud(SurfaceOutputStandardWorld sosw, half3 viewDir, UnityGI gi)
		{
			SurfaceOutputStandard sos = ConvertOutput(sosw);
			half4 shadedColor = LightingStandard(sos, viewDir, gi);
			half4 foggedColor = lerp(unity_FogColor, shadedColor, sosw.fogFactor);
			float blend = (CloudBlend(sosw.worldPos));
			sosw.Albedo = lerp(shadedColor.rgb, sosw.Cloud, blend);
			shadedColor.rgb = lerp(sosw.Albedo, shadedColor.rgb, 1 - blend);
			return shadedColor;
		}

		inline void LightingCloud_GI(SurfaceOutputStandardWorld sosw, UnityGIInput data, inout UnityGI gi)
		{
			SurfaceOutputStandard sos = ConvertOutput(sosw);
			LightingStandard_GI(sos, data, gi);
		}

		sampler2D _MainTex;
		sampler2D _MetallicGlossMap;
		sampler2D _BumpMap;
		sampler2D _OcclusionMap;
		sampler2D _EmissionMap;
		sampler2D _CloudRamp;

		fixed4 _Color;
		half _GlossMapScale;
		half _BumpScale;
		half _OcclusionStrength;
		fixed4 _EmissionColor;

		struct Input 
		{
			float3 worldPos;
			float2 uv_MainTex;
			float1 fogCoord : TEXCOORD0;
		};

		void vert(inout appdata_full v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			UNITY_TRANSFER_FOG(o, UnityObjectToClipPos(v.vertex));
		}

		float4 FogColor(Input IN, float4 color)
		{
			UNITY_APPLY_FOG(IN.fogCoord, color);
			return color;
		}

		float Z0FarFromClipSpace(float coord)
		{
			#if defined(UNITY_REVERSED_Z)
				//D3d with reversed Z => z clip range is [near, 0] -> remapping to [0, far]
				//max is required to protect ourselves from near plane not being correct/meaningfull in case of oblique matrices.
				return max(((1.0 - (coord) / _ProjectionParams.y) * _ProjectionParams.z), 0);
			#elif UNITY_UV_STARTS_AT_TOP
				//D3d without reversed z => z clip range is [0, far] -> nothing to do
				return coord;
			#else
				//Opengl => z clip range is [-near, far] -> should remap in theory but dont do it in practice to save some perf (range is close enought)
				return (coord);
			#endif
		}

		// This function mirrors UnityCG UNITY_CALC_FOG_FACTOR, 
		// but handled so we can store the fog factor ourselves 
		// this is because unity does not with nofog attribute
		float FogFactor(float coord)
		{
			#if defined(FOG_LINEAR)
				// factor = (end-z)/(end-start) = z * (-1/(end-start)) + (end/(end-start))
				return Z0FarFromClipSpace(coord) * unity_FogParams.z + unity_FogParams.w;
			#elif defined(FOG_EXP)
				// factor = exp(-density*z)
				float fogFactor = unity_FogParams.y * Z0FarFromClipSpace(coord);
				fogFactor = exp2(-fogFactor);
				return fogFactor;
			#elif defined(FOG_EXP2)
				// factor = exp(-(density*z)^2)
				float fogFactor = unity_FogParams.x * Z0FarFromClipSpace(coord);
				fogFactor = exp2(-fogFactor *fogFactor);
				return fogFactor;
			#else
				return 0;
			#endif
		}
		
		void surf (Input IN, inout SurfaceOutputStandardWorld o)
		{
			o.worldPos = IN.worldPos;
			o.fogFactor = FogFactor(IN.fogCoord);
			float4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
			float4 c = FogColor(IN, mainTex * _Color);

			float blend = (CloudBlend(IN.worldPos));
			float4 tintColor = tex2D(_CloudRamp, float2(-blend, 0));

			o.Albedo = c.rgb;
			o.Cloud = tintColor;
			#if ALBEDO_SMOOTHNESS
				o.Alpha = _Color.a;
			#else
				o.Alpha = c.a;
			#endif
			o.Alpha = min(1, max(0, lerp(o.Alpha, (tintColor.a * 1.2) - 0.2, blend)));

			//o.Albedo = unity_FogColor.rgb;
			//o.Alpha = 1;
			//return;

			fixed4 e = tex2D(_EmissionMap, IN.uv_MainTex);
			o.Emission = lerp(e.rgb * _EmissionColor.rgb, fixed4(0,0,0,0), blend);

			o.Normal = UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale);
			o.Normal = lerp(o.Normal, fixed3(0, 0, 1), blend);
			
			fixed4 metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex);
			o.Metallic = lerp(metallic.r, 0, blend);
			
			#if ALBEDO_SMOOTHNESS
				o.Smoothness = mainTex.a * _GlossMapScale;
			#else
				o.Smoothness = metallic.a * _GlossMapScale;
			#endif
			o.Smoothness = lerp(o.Smoothness, 0, blend);

			half occ = tex2D(_OcclusionMap, IN.uv_MainTex).g;
			half oneMinusT = 1 - _OcclusionStrength;
			o.Occlusion = oneMinusT + occ * _OcclusionStrength;
			o.Occlusion = lerp(o.Occlusion, 0, blend);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
/*
UNITY_APPLY_FOG(coord, col)
{
	UNITY_APPLY_FOG_COLOR(coord, col, unity_FogColor)
}

UNITY_APPLY_FOG_COLOR(coord, col, unity_FogColor)
{
	UNITY_CALC_FOG_FACTOR((coord).x);
	UNITY_FOG_LERP_COLOR(col, fogCol, unityFogFactor) whatever
}

UNITY_CALC_FOG_FACTOR((coord).x)
{
	UNITY_CALC_FOG_FACTOR_RAW(UNITY_Z_0_FAR_FROM_CLIPSPACE(coord))
}

UNITY_Z_0_FAR_FROM_CLIPSPACE
{
	#if defined(UNITY_REVERSED_Z)
		//D3d with reversed Z => z clip range is [near, 0] -> remapping to [0, far]
		//max is required to protect ourselves from near plane not being correct/meaningfull in case of oblique matrices.
		#define UNITY_Z_0_FAR_FROM_CLIPSPACE(coord) max(((1.0-(coord)/_ProjectionParams.y)*_ProjectionParams.z),0)
	#elif UNITY_UV_STARTS_AT_TOP
		//D3d without reversed z => z clip range is [0, far] -> nothing to do
		#define UNITY_Z_0_FAR_FROM_CLIPSPACE(coord) (coord)
	#else
		//Opengl => z clip range is [-near, far] -> should remap in theory but dont do it in practice to save some perf (range is close enought)
		#define UNITY_Z_0_FAR_FROM_CLIPSPACE(coord) (coord)
	#endif
}

UNITY_CALC_FOG_FACTOR_RAW(
{
	#if defined(FOG_LINEAR)
		// factor = (end-z)/(end-start) = z * (-1/(end-start)) + (end/(end-start))
		float unityFogFactor = (coord) * unity_FogParams.z + unity_FogParams.w;
	#elif defined(FOG_EXP)
		// factor = exp(-density*z)
		float unityFogFactor = unity_FogParams.y * (coord);
		unityFogFactor = exp2(-unityFogFactor)
	#elif defined(FOG_EXP2)
		// factor = exp(-(density*z)^2)
		float unityFogFactor = unity_FogParams.x * (coord);
		unityFogFactor = exp2(-unityFogFactor*unityFogFactor)
	#else
		float unityFogFactor = 0.0
	#endif
}*/