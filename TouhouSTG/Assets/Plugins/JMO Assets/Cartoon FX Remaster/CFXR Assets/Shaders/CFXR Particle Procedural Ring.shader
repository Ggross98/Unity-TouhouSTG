//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

Shader "Cartoon FX/Remaster/Particle Procedural Ring"
{
	Properties
	{
	//# Blending
	//#
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Blend Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Blend Destination", Float) = 10
	
	//# --------------------------------------------------------

		[Toggle(_CFXR_DISSOLVE)] _UseDissolve ("Enable Dissolve", Float) = 0
	//# IF_KEYWORD _CFXR_DISSOLVE
		[NoScaleOffset] _DissolveTex ("Dissolve Texture", 2D) = "gray" {}
		_DissolveSmooth ("Dissolve Smoothing", Range(0.0001,0.5)) = 0.1
		[ToggleNoKeyword] _InvertDissolveTex ("Invert Dissolve Texture", Float) = 0
	//# END_IF

	//# --------------------------------------------------------

	//# Textures
	//#

		_MainTex ("Texture", 2D) = "white" {}
		[Toggle(_CFXR_SINGLE_CHANNEL)] _SingleChannel ("Single Channel Texture", Float) = 0

	//# --------------------------------------------------------

	//# Ring
	//#

		[Toggle(_CFXR_RADIAL_UV)] _UseRadialUV ("Enable Radial UVs", Float) = 0
		_RingTopOffset ("Ring Offset", float) = 0.05
		[Toggle(_CFXR_WORLD_SPACE_RING)] _WorldSpaceRing ("World Space", Float) = 0

	//# --------------------------------------------------------

		[Toggle(_CFXR_HDR_BOOST)] _HdrBoost ("Enable HDR Multiplier", Float) = 0
	//# IF_KEYWORD _CFXR_HDR_BOOST
		_HdrMultiply ("HDR Multiplier", Float) = 2
	//# END_IF

	//# --------------------------------------------------------
	
		[Toggle(_FADING_ON)] _UseSP ("Soft Particles", Float) = 0
	//# IF_KEYWORD _FADING_ON
		_SoftParticlesFadeDistanceNear ("Near Fade", Float) = 0
		_SoftParticlesFadeDistanceFar ("Far Fade", Float) = 1
	//# END_IF

	//# ========================================================
	//# Shadows
	//#

		[KeywordEnum(Off,On,CustomTexture)] _CFXR_DITHERED_SHADOWS ("Dithered Shadows", Float) = 0
	//# IF_KEYWORD _CFXR_DITHERED_SHADOWS_ON || _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE
		_ShadowStrength		("Shadows Strength Max", Range(0,1)) = 1.0
		//#	IF_KEYWORD _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE
		_DitherCustom		("Dithering 3D Texture", 3D) = "black" {}
		//#	END_IF
	//# END_IF
	}
	
	Category
	{
		Tags
		{
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
		}
		Blend [_SrcBlend] [_DstBlend]
		Cull  Off
		ZWrite Off

		//================================================================================================================================
		// Includes & Code

		CGINCLUDE

			#include "UnityCG.cginc"
			#include "UnityStandardUtils.cginc"

			// --------------------------------

			#include "CFXR_SETTINGS.cginc"

			// --------------------------------

			CBUFFER_START(UnityPerMaterial)

			float4 _MainTex_ST;

			half _InvertDissolveTex;
			half _DissolveSmooth;

			half _HdrMultiply;

			float _RingTopOffset;

			half _Cutoff;

			half _SoftParticlesFadeDistanceNear;
			half _SoftParticlesFadeDistanceFar;
			half _EdgeFadePow;

		#if !defined(SHADER_API_GLES)
			float _ShadowStrength;
			float4 _DitherCustom_TexelSize;
		#endif

			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _DissolveTex;
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
		#if !defined(SHADER_API_GLES)
			sampler3D _DitherMaskLOD;
			sampler3D _DitherCustom;
		#endif

			// --------------------------------
			// Input/Output

			struct appdata
			{
				float4 vertex		: POSITION;
				half4 color			: COLOR;
				float4 texcoord		: TEXCOORD0;	//uv + particle data
				float4 texcoord1	: TEXCOORD1;	//additional particle data
				float4 texcoord2    : TEXCOORD2;    //procedural ring data: x = width, y = smooth, z = rotation, w = particle size
		#if PASS_SHADOW_CASTER || _CFXR_WORLD_SPACE_RING
				float3 normal : NORMAL;
		#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			// vertex to fragment
			struct v2f
			{
				float4 pos					: SV_POSITION;
				half4 color					: COLOR;
				float4 uv_uv2				: TEXCOORD0;	//uv + particle data
				float4 ringData				: TEXCOORD1;    //procedural ring data
				float4 uvRing_uvCartesian	: TEXCOORD2;
		#if !defined(GLOBAL_DISABLE_SOFT_PARTICLES) && ((defined(SOFTPARTICLES_ON) || defined(CFXR_URP) || defined(SOFT_PARTICLES_ORTHOGRAPHIC)) && defined(_FADING_ON))
				float4 projPos				: TEXCOORD3;
		#endif
				UNITY_FOG_COORDS(4)		//note: does nothing if fog is not enabled
		#if _CFXR_DISSOLVE
				float4 custom1				: TEXCOORD5;
		#endif
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			// vertex to fragment (shadow caster)
			struct v2f_shadowCaster
			{
				V2F_SHADOW_CASTER_NOPOS
				half4 color					: COLOR;
				float4 uv_uv2				: TEXCOORD1;	//uv + particle data
				float4 ringData				: TEXCOORD2;    //procedural ring data
				float4 uvRing_uvCartesian	: TEXCOORD3;
		#if _CFXR_DISSOLVE
				float4 custom1				: TEXCOORD4;
		#endif
			};

			// --------------------------------

			#include "CFXR.cginc"

			// --------------------------------
			// Vertex

		#if PASS_SHADOW_CASTER
			void vertex_program (appdata v, out v2f_shadowCaster o, out float4 opos : SV_POSITION)
		#else
			v2f vertex_program (appdata v)
		#endif
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

				//--------------------------------
				// procedural ring

				float ringWidth = v.texcoord2.x;
				float ringSmooth = v.texcoord2.y;
				float ringRotation = v.texcoord2.z;
				float particleSize = v.texcoord2.w;

				// avoid artifacts when vertex are pushed too much
				ringWidth = min(particleSize, ringWidth);

				// constants calculated per vertex
				o.ringData.x = pow(1 - ringWidth / particleSize, 2);
				o.ringData.y = 1 - _RingTopOffset;
				o.ringData.z = ringSmooth / particleSize; // smoothing depends on particle size
				o.ringData.w = ringRotation;

				// regular ring UVs
				float2 uv = v.texcoord.xy + float2(ringRotation, 0);
				o.uvRing_uvCartesian.xy = 1 - TRANSFORM_TEX(uv, _MainTex);


			#if _CFXR_WORLD_SPACE_RING
					// to clip space with width offset
					v.vertex.xyz = v.vertex.xyz - v.normal.xyz * v.texcoord.y * ringWidth;
				#if !PASS_SHADOW_CASTER
					o.pos = UnityObjectToClipPos(v.vertex);
				#endif
			#else
					// to clip space with width offset
					float4 m = mul(UNITY_MATRIX_V, v.vertex);
					m.xy += -v.texcoord.zw * v.texcoord.y * ringWidth;
				#if !PASS_SHADOW_CASTER
					o.pos = mul(UNITY_MATRIX_P, m);
				#endif
			#endif

				//------------------------------------------
				/*
				//v.vertex.xy += -v.texcoord.zw * v.texcoord.y * ringWidth;
				v.vertex.xy += v.texcoord.zw * v.texcoord.y * 0.5;
			#if !PASS_SHADOW_CASTER
				o.pos = UnityObjectToClipPos(v.vertex);
			#endif
				*/
				//------------------------------------------

				// calculate cartesian UVs to accurately calculate ring in fragment shader
				o.uvRing_uvCartesian.zw = v.texcoord.zw - v.texcoord.zw * v.texcoord.y * ringWidth / particleSize;

				//--------------------------------

				o.color = v.color;
				o.uv_uv2 = v.texcoord;

				//--------------------------------

		#if _CFXR_DISSOLVE
				o.custom1 = v.texcoord1;
		#endif

		#if PASS_SHADOW_CASTER
				vert(v, o, opos);
		#else
				return vert(v, o);
		#endif
			}

			// --------------------------------
			// Fragment

		#if PASS_SHADOW_CASTER
			float4 fragment_program (v2f_shadowCaster i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
		#else
			half4 fragment_program (v2f i) : SV_Target
		#endif
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

				// ================================================================
				// Color & Alpha

				//--------------------------------
				// procedural ring

				float b = i.ringData.x; // bottom
				float t = i.ringData.y; // top
				float smooth = i.ringData.z; // smoothing
				float gradient = dot(i.uvRing_uvCartesian.zw, i.uvRing_uvCartesian.zw);
				float ring = saturate( smoothstep(b, b + smooth, gradient) - smoothstep(t - smooth, t, gradient) );

			#if _CFXR_RADIAL_UV
				// approximate polar coordinates
				float2 radialUv = float2
				(
					(atan2(i.uvRing_uvCartesian.z, i.uvRing_uvCartesian.w) / UNITY_PI) * 0.5 + 0.5 + 0.23 - i.ringData.w,
					(gradient * (1.0 / (t - b)) - (b / (t - b))) * 0.9 - 0.92 + 1
				);
				radialUv.xy = radialUv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float dx = ddx(i.uvRing_uvCartesian.x);
				//float dy = ddx(i.uvRing_uvCartesian.x);
				#define TEX2D_MAIN_TEXCOORD(sampler) tex2Dgrad(sampler, radialUv, dx, dx)
			#else
				#define TEX2D_MAIN_TEXCOORD(sampler) tex2D(sampler, i.uvRing_uvCartesian.xy)
			#endif

				#if _CFXR_SINGLE_CHANNEL
				half4 mainTex = half4(1, 1, 1, TEX2D_MAIN_TEXCOORD(_MainTex).r);
				#else
				half4 mainTex = TEX2D_MAIN_TEXCOORD(_MainTex);
				#endif

				mainTex *= ring;

				//--------------------------------
										
				half3 particleColor = mainTex.rgb * i.color.rgb;
				half particleAlpha = mainTex.a * i.color.a;

			#if _CFXR_HDR_BOOST
				#ifdef UNITY_COLORSPACE_GAMMA
					_HdrMultiply = LinearToGammaSpaceApprox(_HdrMultiply);
				#endif
				particleColor.rgb *= _HdrMultiply * GLOBAL_HDR_MULTIPLIER;
			#endif

				// ================================================================
				// Dissolve

			#if _CFXR_DISSOLVE
				half dissolveTex = TEX2D_MAIN_TEXCOORD(_DissolveTex).r;
				dissolveTex = _InvertDissolveTex <= 0 ? 1 - dissolveTex : dissolveTex;
				half dissolveTime = i.custom1.x;
			#else
				half dissolveTex = 0;
				half dissolveTime = 0;
			#endif

				// ================================================================
				//

			#if PASS_SHADOW_CASTER
				return frag(i, vpos, particleColor, particleAlpha, dissolveTex, dissolveTime, 0.0);
			#else
				return frag(i, particleColor, particleAlpha, dissolveTex, dissolveTime, 0.0);
			#endif
			}


		ENDCG

		//====================================================================================================================================
		// Universal Rendering Pipeline

		SubShader
		{
			Pass
			{
				Name "BASE"
				Tags { "LightMode"="UniversalForward" }

				CGPROGRAM

				#pragma vertex vertex_program
				#pragma fragment fragment_program
				
				#pragma target 2.0
				
				// #pragma multi_compile_instancing
				#pragma multi_compile_fog

				#pragma multi_compile CFXR_URP
				
				#pragma shader_feature_local _ _CFXR_SINGLE_CHANNEL
				#pragma shader_feature_local _ _CFXR_RADIAL_UV
				#pragma shader_feature_local _ _CFXR_WORLD_SPACE_RING
				#pragma shader_feature_local _ _CFXR_DISSOLVE
				#pragma shader_feature_local _ _CFXR_HDR_BOOST

				// Using the same keywords as Unity's Standard Particle shader to minimize project-wide keyword usage
				#pragma shader_feature_local _ _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON _CFXR_ADDITIVE


				ENDCG
			}
			
			// Same as above with 'Universal2D' instead and DISABLE_SOFT_PARTICLES keyword
			Pass
			{
				Name "BASE"
				Tags { "LightMode"="Universal2D" }

				CGPROGRAM

				#pragma vertex vertex_program
				#pragma fragment fragment_program
				
				#pragma target 2.0
				
				// #pragma multi_compile_instancing
				#pragma multi_compile_fog

				#pragma multi_compile CFXR_URP
				#pragma multi_compile DISABLE_SOFT_PARTICLES
				
				#pragma shader_feature_local _ _CFXR_SINGLE_CHANNEL
				#pragma shader_feature_local _ _CFXR_RADIAL_UV
				#pragma shader_feature_local _ _CFXR_WORLD_SPACE_RING
				#pragma shader_feature_local _ _CFXR_DISSOLVE
				#pragma shader_feature_local _ _CFXR_HDR_BOOST

				// Using the same keywords as Unity's Standard Particle shader to minimize project-wide keyword usage
				#pragma shader_feature_local _ _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON _CFXR_ADDITIVE


				ENDCG
			}

			//--------------------------------------------------------------------------------------------------------------------------------

			Pass
			{
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				BlendOp Add
				Blend One Zero
				ZWrite On
				Cull Off
			
				CGPROGRAM

				#pragma multi_compile CFXR_URP
				#pragma multi_compile PASS_SHADOW_CASTER

				#pragma vertex vertex_program
				#pragma fragment fragment_program

				#pragma shader_feature_local _ _CFXR_SINGLE_CHANNEL
				#pragma shader_feature_local _ _CFXR_RADIAL_UV
				#pragma shader_feature_local _ _CFXR_WORLD_SPACE_RING
				#pragma shader_feature_local _ _CFXR_DISSOLVE

				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON _CFXR_ADDITIVE

				#pragma multi_compile_shadowcaster
				#pragma shader_feature_local _ _CFXR_DITHERED_SHADOWS_ON _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE

			#if (_CFXR_DITHERED_SHADOWS_ON || _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE) && !defined(SHADER_API_GLES)
				#pragma target 3.0		//needed for VPOS
			#endif

				ENDCG
			}
		}

		//====================================================================================================================================
		// Built-in Rendering Pipeline

		SubShader
		{
			Pass
			{
				Name "BASE"
				Tags { "LightMode"="ForwardBase" }

				CGPROGRAM

				#pragma vertex vertex_program
				#pragma fragment fragment_program
				
				#pragma target 2.0
				
				#pragma multi_compile_particles
				// #pragma multi_compile_instancing
				#pragma multi_compile_fog
				
				#pragma shader_feature_local _ _CFXR_SINGLE_CHANNEL
				#pragma shader_feature_local _ _CFXR_RADIAL_UV
				#pragma shader_feature_local _ _CFXR_WORLD_SPACE_RING
				#pragma shader_feature_local _ _CFXR_DISSOLVE
				#pragma shader_feature_local _ _CFXR_HDR_BOOST

				// Using the same keywords as Unity's Standard Particle shader to minimize project-wide keyword usage
				#pragma shader_feature_local _ _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON _CFXR_ADDITIVE


				ENDCG
			}

			//--------------------------------------------------------------------------------------------------------------------------------

			Pass
			{
				Name "ShadowCaster"
				Tags { "LightMode" = "ShadowCaster" }

				BlendOp Add
				Blend One Zero
				ZWrite On
				Cull Off
			
				CGPROGRAM

				#pragma multi_compile PASS_SHADOW_CASTER

				#pragma vertex vertex_program
				#pragma fragment fragment_program

				#pragma shader_feature_local _ _CFXR_SINGLE_CHANNEL
				#pragma shader_feature_local _ _CFXR_RADIAL_UV
				#pragma shader_feature_local _ _CFXR_WORLD_SPACE_RING
				#pragma shader_feature_local _ _CFXR_DISSOLVE

				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON _CFXR_ADDITIVE

				#pragma multi_compile_shadowcaster
				#pragma shader_feature_local _ _CFXR_DITHERED_SHADOWS_ON _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE

			#if (_CFXR_DITHERED_SHADOWS_ON || _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE) && !defined(SHADER_API_GLES)
				#pragma target 3.0		//needed for VPOS
			#endif

				ENDCG
			}
		}
	}
	
	CustomEditor "CartoonFX.MaterialInspector"
}

