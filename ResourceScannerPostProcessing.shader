Shader "Hidden/ResourceScannerPostProcessing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Colour", Color) = (1,1,1,1)
	}

	CGINCLUDE
	// global input
	float _minDepth;
	float _maxDepth;
	float _power;
	float4 _resourceColor;
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenSpace : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenSpace = ComputeScreenPos( o.vertex );
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			float3 nearColor;
			float3 farColor;
			float4 _Color;
            sampler2D _CameraDepthTexture;

			float4 frag (v2f i) : SV_Target
			{
				float2 screenSpaceUV = i.screenSpace.xy/ i.screenSpace.w;
				float depth = Linear01Depth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, screenSpaceUV ));
				float sd = ( depth - _minDepth ) / ( _maxDepth - _minDepth );
				sd = saturate( sd );
				sd = pow( sd, _power );
				float3 depthColor = lerp(nearColor, farColor, sd );
				
				float4 src = tex2D(_MainTex, i.uv);
				//return src.a;
				// return float4(lerp(nearColor, farColor, sd ), 1);
				return float4(lerp(_resourceColor, depthColor, 1-src.a ), 1);
			}
			ENDCG
		}
	}
}
