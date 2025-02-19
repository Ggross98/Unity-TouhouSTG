//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

Shader "Cartoon FX/Remaster/Particle Screen Distortion"
{
	Properties
	{ 
		[Toggle(_ALPHATEST_ON)] _UseAlphaClip ("Alpha Clipping (Cutout)", Float) = 0
	//# IF_KEYWORD _ALPHATEST_ON
		_Cutoff ("Cutoff Threshold", Range(0.001,1)) = 0.1
	//# END_IF
	
	//# --------------------------------------------------------
	
		[Toggle(_FADING_ON)] _UseSP ("Soft Particles", Float) = 0
	//# IF_KEYWORD _FADING_ON
		_SoftParticlesFadeDistanceNear ("Near Fade", Float) = 0
		_SoftParticlesFadeDistanceFar ("Far Fade", Float) = 1
	//# END_IF

	//# 

		[Toggle(_CFXR_EDGE_FADING)] _UseEF ("Edge Fade", Float) = 0
	//# IF_KEYWORD _CFXR_EDGE_FADING
		_EdgeFadePow ("Edge Fade Power", Float) = 1
	//# END_IF

	//# ========================================================
	//# Texture
	//#
		[NoScaleOffset] _ScreenDistortionTex ("Distortion Texture", 2D) = "bump" {}
		_ScreenDistortionScale ("Distortion Scale", Range(-0.5, 0.5)) = 0.1
		
	//# ========================================================
	//# Debug
	//# 
		
		[Toggle(_DEBUG_VISUALIZE_DISTORTION)] _DebugVisualize ("Visualize Distortion Particles", Float) = 0 
	}
	
	Category
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}

		Blend SrcAlpha OneMinusSrcAlpha, One One
		ZWrite Off
		Cull  Off

		//================================================================================================================================
		// Includes & Code

		CGINCLUDE

			#include "UnityCG.cginc"
			
			#if CFXR_URP
				#include "CFXR_URP.cginc"
			#endif

			#include "CFXR_SETTINGS.cginc"

			// --------------------------------

			CBUFFER_START(UnityPerMaterial)

			half _ScreenDistortionScale;

			half _Cutoff;

			half _SoftParticlesFadeDistanceNear;
			half _SoftParticlesFadeDistanceFar;
			half _EdgeFadePow;

			CBUFFER_END

			sampler2D _ScreenDistortionTex;
			#if defined(CFXR_URP)
				sampler2D _CameraOpaqueTexture;
				#define SampleScreenTexture(uv) tex2Dproj(_CameraOpaqueTexture, uv)
			#else
				sampler2D _GrabTexture;
				#define SampleScreenTexture(uv) tex2Dproj(_GrabTexture, uv)
			#endif
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);

			// --------------------------------
			// Input/output

			struct appdata
			{
				float4 vertex		: POSITION;
				half4 color			: COLOR;
				float4 texcoord		: TEXCOORD0;	//xy = uv, zw = random
				float4 texcoord1	: TEXCOORD1;	//additional particle data: x = dissolve, y = animFrame
				float4 texcoord2	: TEXCOORD2;	//additional particle data: second color
		#if _CFXR_EDGE_FADING
				float3 normal       : NORMAL;
		#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			// vertex to fragment
			struct v2f
			{
				float4 pos				: SV_POSITION;
				half4 color				: COLOR;
				float4 uv_random		: TEXCOORD0;	//uv + particle data
				float4 custom1			: TEXCOORD1;	//additional particle data
				float4 grabPassPosition	: TEXCOORD2;
		#if !defined(GLOBAL_DISABLE_SOFT_PARTICLES) && ((defined(SOFTPARTICLES_ON) || defined(CFXR_URP) || defined(SOFT_PARTICLES_ORTHOGRAPHIC)) && defined(_FADING_ON))
				float4 projPos			: TEXCOORD3;
		#endif
		#if !PASS_SHADOW_CASTER
				UNITY_FOG_COORDS(4)		//note: does nothing if fog is not enabled
		#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			// --------------------------------

			#include "CFXR.cginc"

			// --------------------------------
			// Vertex

			v2f vertex_program (appdata v)
			{
		#if !PASS_SHADOW_CASTER
				v2f o = (v2f)0;
				#if CFXR_URP
					o = (v2f)0;
				#else
					UNITY_INITIALIZE_OUTPUT(v2f, o);
				#endif
		#else
				o = (v2f_shadowCaster)0;
		#endif

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

		#if !PASS_SHADOW_CASTER
				o.pos = UnityObjectToClipPos(v.vertex);
				o.grabPassPosition = ComputeGrabScreenPos(o.pos);
		#endif

				o.color = GetParticleColor(v.color);
				o.custom1 = v.texcoord1;
				GetParticleTexcoords(o.uv_random.xy, o.uv_random.zw, o.custom1.y, v.texcoord, v.texcoord1.y);
				//o.uv_random = v.texcoord;

				return vert(v, o);
			}

			// --------------------------------
			// Fragment

			half4 fragment_program (v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				
				// ================================================================
				// Screen Distortion

				half4 distortionTex = tex2D(_ScreenDistortionTex, i.uv_random.xy);
				half particleAlpha = i.color.a * distortionTex.b;
				half2 screenDistortion = (distortionTex.rg * 2.0 - 1.0) * _ScreenDistortionScale * particleAlpha;
				float4 grabPosUV = i.grabPassPosition;
				grabPosUV.xy += screenDistortion;
				half3 particleColor = SampleScreenTexture(grabPosUV).rgb;

				#if defined(_DEBUG_VISUALIZE_DISTORTION)
					return half4(distortionTex.rg, 0, particleAlpha);
				#endif

			#if PASS_SHADOW_CASTER
				return frag(i, vpos, particleColor, particleAlpha, dissolveTex, dissolveTime, 0.0);
			#else
				return frag(i, particleColor, particleAlpha, 0, 0, 0.0);
			#endif
			}

		ENDCG

		//====================================================================================================================================
		// Universal Rendering Pipeline

		Subshader
		{
			Pass
			{
				Name "BASE_URP"
				Tags { "LightMode"="UniversalForward" }

				CGPROGRAM

				#pragma vertex vertex_program
				#pragma fragment fragment_program
				
				#pragma target 2.0
				
				// #pragma multi_compile_instancing
				// #pragma instancing_options procedural:ParticleInstancingSetup

				#pragma multi_compile_fog
				//#pragma multi_compile_fwdbase
				//#pragma multi_compile SHADOWS_SCREEN

				#pragma multi_compile CFXR_URP
				
				// Using the same keywords as Unity's Standard Particle shader to minimize project-wide keyword usage
				#pragma shader_feature_local _ _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _DEBUG_VISUALIZE_DISTORTION

				ENDCG
			}
			
			// Same as above with 'Universal2D' instead and DISABLE_SOFT_PARTICLES keyword
			Pass
			{
				Name "BASE_URP"
				Tags { "LightMode"="Universal2D" }

				CGPROGRAM

				#pragma vertex vertex_program
				#pragma fragment fragment_program
				
				#pragma target 2.0
				
				// #pragma multi_compile_instancing
				// #pragma instancing_options procedural:ParticleInstancingSetup

				#pragma multi_compile_fog
				//#pragma multi_compile_fwdbase
				//#pragma multi_compile SHADOWS_SCREEN

				#pragma multi_compile CFXR_URP
				#pragma multi_compile DISABLE_SOFT_PARTICLES
				
				// Using the same keywords as Unity's Standard Particle shader to minimize project-wide keyword usage
				#pragma shader_feature_local _ _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _DEBUG_VISUALIZE_DISTORTION

				ENDCG
			}
		}

		//====================================================================================================================================
		// Built-in Rendering Pipeline

		SubShader
		{
			GrabPass
			{
				Tags { "LightMode" = "Always" }
				"_GrabTexture"
			}
			
			Pass
			{
				Name "BASE"
				Tags { "LightMode"="ForwardBase" }

				CGPROGRAM

				#pragma vertex vertex_program
				#pragma fragment fragment_program

				//vertInstancingSetup writes to global, not allowed with DXC
				// #pragma never_use_dxc
				// #pragma target 2.5
				// #pragma multi_compile_instancing
				// #pragma instancing_options procedural:vertInstancingSetup

				#pragma multi_compile_particles
				#pragma multi_compile_fog
				//#pragma multi_compile_fwdbase
				//#pragma multi_compile SHADOWS_SCREEN
				
				// Using the same keywords as Unity's Standard Particle shader to minimize project-wide keyword usage
				#pragma shader_feature_local _ _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _DEBUG_VISUALIZE_DISTORTION

				#include "UnityStandardParticleInstancing.cginc"

				ENDCG
			}
		}
	}
	
	CustomEditor "CartoonFX.MaterialInspector"
}

