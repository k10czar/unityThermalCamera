Shader "Hidden/ThermalVisionTextureRampPostProcessing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			//#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float Luminance(float3 color)
			{
				return dot(color, float3(0.299f, 0.587f, 0.114f));
			}

			sampler2D _MainTex;

			float _PalleteY; // 0: don't use texture; otherwise means texture index (1 or 2)
			sampler2D _PaletteTex;

			float4 frag (v2f i) : SV_Target
			{
				float4 src = tex2D(_MainTex, i.uv);
				float lum = Luminance(src);
				return tex2D(_PaletteTex, float2(lum, _PalleteY));
			}
			ENDCG
		}
	}
}
