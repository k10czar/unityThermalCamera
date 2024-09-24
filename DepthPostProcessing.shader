Shader "Blit/DepthPostProcessing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	HLSLINCLUDE
	// global input
	float _minDepth;
	float _maxDepth;
	float _power;
	float4 _pulseColor;
	float _pulseMaxDistance;
	float _pulseLength;
	float _pulseFrequency;
	float _pulsePower;
	ENDHLSL

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

        	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

			struct Attributes
			{
				float4 positionOS : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionCS : SV_POSITION;
				float4 screenSpace : TEXCOORD0;
			};

			Varyings vert (Attributes IN)
			{
				Varyings OUT;
				VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
				OUT.positionCS = positionInputs.positionCS;
				OUT.screenSpace = positionInputs.positionNDC;
				return OUT;
			}

			float3 nearColor;
			float3 farColor;

			float4 frag (Varyings i) : SV_Target
			{
				float2 screenSpaceUV = i.screenSpace.xy/ i.screenSpace.w;
				float rawDepth = SampleSceneDepth( screenSpaceUV );
				// float depth = Linear01Depth(rawDepth);
				float depth = rawDepth;
				float sd = ( depth - _minDepth ) / ( _maxDepth - _minDepth );

				float pulse = _pulseMaxDistance * pow( ( _Time.y * _pulseFrequency ) % 1.0, _pulsePower );
				float pulseFunc = saturate( -( abs( depth - pulse ) - _pulseLength ) / _pulseLength );

				sd = saturate( sd );
				sd = pow( sd, _power );
				float3 baseColor = lerp(nearColor, farColor, sd );
				float3 combined = lerp(baseColor, _pulseColor.rgb, pulseFunc * _pulseColor.a );

				return float4( combined, 1);
			}
			ENDHLSL
		}
	}
}
