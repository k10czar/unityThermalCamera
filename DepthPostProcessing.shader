Shader "Hidden/DepthPostProcessing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}

	CGINCLUDE
	// global input
	float _minDepth;
	float _maxDepth;
	float _power;
	float4 _pulseColor;
	float _pulseMaxDistance;
	float _pulseLength;
	float _pulseFrequency;
	float _pulsePower;
	ENDCG

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenSpace : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenSpace = ComputeScreenPos( o.vertex );
				return o;
			}

			float3 nearColor;
			float3 farColor;
            sampler2D _CameraDepthTexture;

			float4 frag (v2f i) : SV_Target
			{
				float2 screenSpaceUV = i.screenSpace.xy/ i.screenSpace.w;
				float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, screenSpaceUV ));
				float sd = ( depth - _minDepth ) / ( _maxDepth - _minDepth );

				float pulse = _pulseMaxDistance * pow( ( _Time.y * _pulseFrequency ) % 1.0, _pulsePower );
				float pulseFunc = saturate( -( abs( depth - pulse ) - _pulseLength ) / _pulseLength );

				sd = saturate( sd );
				sd = pow( sd, _power );
				float3 baseColor = lerp(nearColor, farColor, sd );
				float3 combined = lerp(baseColor, _pulseColor.rgb, pulseFunc * _pulseColor.a );

				return float4( combined, 1);
			}
			ENDCG
		}
	}
}
