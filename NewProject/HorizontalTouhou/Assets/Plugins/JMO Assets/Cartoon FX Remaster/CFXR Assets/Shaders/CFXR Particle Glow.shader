//--------------------------------------------------------------------------------------------------------------------------------
// Cartoon FX
// (c) 2012-2020 Jean Moreno
//--------------------------------------------------------------------------------------------------------------------------------

Shader "Cartoon FX/Remaster/Particle Procedural Glow"
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

	//# Procedural Circle
	//#

		[KeywordEnum(P0, P2, P4, P8)] _CFXR_GLOW_POW ("Apply Power of", Float) = 0
		_GlowMin ("Circle Min", Float) = 0
		_GlowMax ("Circle Max", Float) = 1
	//#
		_MaxValue ("Max Value", Float) = 10

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
			"PreviewType"="Plane"
		}
		Blend [_SrcBlend] [_DstBlend], One One
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

			half _GlowMin;
			half _GlowMax;
			half _MaxValue;

			half _InvertDissolveTex;
			half _DissolveSmooth;

			half _HdrMultiply;

			half _Cutoff;

			half _SoftParticlesFadeDistanceNear;
			half _SoftParticlesFadeDistanceFar;
			half _EdgeFadePow;

		#if !defined(SHADER_API_GLES)
			float _ShadowStrength;
			float4 _DitherCustom_TexelSize;
		#endif

			CBUFFER_END

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
				float4 texcoord		: TEXCOORD0;	//xy = uv, zw = random
				float4 texcoord1	: TEXCOORD1;	//additional particle data: x = dissolve
		#if PASS_SHADOW_CASTER
				float3 normal : NORMAL;
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
		#if !defined(GLOBAL_DISABLE_SOFT_PARTICLES) && ((defined(SOFTPARTICLES_ON) || defined(CFXR_URP) || defined(SOFT_PARTICLES_ORTHOGRAPHIC)) && defined(_FADING_ON))
				float4 projPos			: TEXCOORD2;
		#endif
				UNITY_FOG_COORDS(3)		//note: does nothing if fog is not enabled
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			// vertex to fragment (shadow caster)
			struct v2f_shadowCaster
			{
				V2F_SHADOW_CASTER_NOPOS
				half4 color				: COLOR;
				float4 uv_random		: TEXCOORD1;	//uv + particle data
				float4 custom1			: TEXCOORD2;	//additional particle data
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
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

		#if !PASS_SHADOW_CASTER
				o.pos = UnityObjectToClipPos(v.vertex);
		#endif

				o.color = GetParticleColor(v.color);
				o.custom1 = v.texcoord1;
				GetParticleTexcoords(o.uv_random.xy, o.uv_random.zw, o.custom1.y, v.texcoord, v.texcoord1.y);
				//o.uv_random = v.texcoord;

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
				// procedural glow
				float2 uvm = i.uv_random.xy - 0.5;
				half glow = saturate(1 - ((dot(uvm, uvm) * 4)));
			#if _CFXR_GLOW_POW_P2
				glow = pow(glow, 2);
			#elif _CFXR_GLOW_POW_P4
				glow = pow(glow, 4);
			#elif _CFXR_GLOW_POW_P8
				glow = pow(glow, 8);
			#endif
				glow = clamp(lerp(_GlowMin, _GlowMax, glow), 0, _MaxValue) * saturate(glow * 30);
				half4 mainTex = half4(1, 1, 1, glow);
				//--------------------------------
					
			#if _CFXR_HDR_BOOST
				#ifdef UNITY_COLORSPACE_GAMMA
					_HdrMultiply = LinearToGammaSpaceApprox(_HdrMultiply);
				#endif
				mainTex.rgb *= _HdrMultiply * GLOBAL_HDR_MULTIPLIER;
			#endif
					
				half3 particleColor = mainTex.rgb * i.color.rgb;
				half particleAlpha = mainTex.a * i.color.a;

				// ================================================================
				// Dissolve

			#if _CFXR_DISSOLVE
				half dissolveTex = tex2D(_DissolveTex, i.uv_random.xy).r;
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
				// #pragma instancing_options procedural:ParticleInstancingSetup
				#pragma multi_compile_fog

				#pragma multi_compile CFXR_URP
				
				#pragma shader_feature_local _ _CFXR_GLOW_POW_P2 _CFXR_GLOW_POW_P4 _CFXR_GLOW_POW_P8
				#pragma shader_feature_local _ _CFXR_HDR_BOOST
				#pragma shader_feature_local _ _CFXR_DISSOLVE

				#pragma shader_feature_local _FADING_ON
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
				// #pragma instancing_options procedural:ParticleInstancingSetup
				#pragma multi_compile_fog

				#pragma multi_compile CFXR_URP
				#pragma multi_compile DISABLE_SOFT_PARTICLES
				
				#pragma shader_feature_local _ _CFXR_GLOW_POW_P2 _CFXR_GLOW_POW_P4 _CFXR_GLOW_POW_P8
				#pragma shader_feature_local _ _CFXR_HDR_BOOST
				#pragma shader_feature_local _ _CFXR_DISSOLVE

				#pragma shader_feature_local _FADING_ON
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

				#pragma shader_feature_local _ _CFXR_GLOW_POW_P2 _CFXR_GLOW_POW_P4 _CFXR_GLOW_POW_P8
				#pragma shader_feature_local _ _CFXR_DISSOLVE

				#pragma shader_feature_local _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

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
				
				//vertInstancingSetup writes to global, not allowed with DXC
				// #pragma never_use_dxc
				// #pragma target 2.5
				// #pragma multi_compile_instancing
				// #pragma instancing_options procedural:vertInstancingSetup

				#pragma multi_compile_particles
				#pragma multi_compile_fog
				
				#pragma shader_feature_local _ _CFXR_GLOW_POW_P2 _CFXR_GLOW_POW_P4 _CFXR_GLOW_POW_P8
				#pragma shader_feature_local _ _CFXR_HDR_BOOST
				#pragma shader_feature_local _ _CFXR_DISSOLVE

				#pragma shader_feature_local _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON
				#pragma shader_feature_local _ _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON _CFXR_ADDITIVE

				#include "UnityStandardParticleInstancing.cginc"

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

				//vertInstancingSetup writes to global, not allowed with DXC
				// #pragma never_use_dxc
				// #pragma target 2.5
				// #pragma multi_compile_instancing
				// #pragma instancing_options procedural:vertInstancingSetup

				#pragma shader_feature_local _ _CFXR_GLOW_POW_P2 _CFXR_GLOW_POW_P4 _CFXR_GLOW_POW_P8
				#pragma shader_feature_local _ _CFXR_DISSOLVE

				#pragma shader_feature_local _FADING_ON
				#pragma shader_feature_local _ _ALPHATEST_ON _ALPHABLEND_ON _ALPHAPREMULTIPLY_ON _ALPHAMODULATE_ON

				#pragma multi_compile_shadowcaster
				#pragma shader_feature_local _ _CFXR_DITHERED_SHADOWS_ON _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE

			#if (_CFXR_DITHERED_SHADOWS_ON || _CFXR_DITHERED_SHADOWS_CUSTOMTEXTURE) && !defined(SHADER_API_GLES)
				#pragma target 3.0		//needed for VPOS
			#endif

				#include "UnityStandardParticleInstancing.cginc"

				ENDCG
			}
		}
	}
	
	CustomEditor "CartoonFX.MaterialInspector"
}

