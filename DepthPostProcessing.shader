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
				float2 depth : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_DEPTH(o.depth);
				return o;
			}

			float3 nearColor;
			float3 farColor;

			float4 frag (v2f i) : SV_Target
			{
				// UNITY_OUTPUT_DEPTH(i.depth);
				// //Todo: Output depth value
				float depth = -(UnityObjectToViewPos( v.vertex ).z * _ProjectionParams.w);
				float sd = ( depth - _minDepth ) / ( _maxDepth - _minDepth );
				return float4(lerp(nearColor, farColor, sd ), 1);
			}
			ENDCG
		}
	}
}
