Shader "Custom/FireIGuess"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseSpeed ("Noise Vertical Speed", float) = 1
		_Bottom ("Bottom Color", Color) = (1,1,1,1)
		_Top ("Top Color", Color) = (0,0,0,1)
		_Saturate("Saturate Noise", Range(0,1)) = 30
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 100
		ZWrite Off
		Blend One One

		Pass
		{
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _NoiseSpeed;
			fixed4 _Bottom;
			fixed4 _Top;
			float _Saturate;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				
				fixed4 noise = tex2D(_MainTex, float2(i.uv.x, i.uv.y + _Time[0] * _NoiseSpeed));
				fixed gradient = lerp(_Top, _Bottom, i.uv.y);
				float4 flame = saturate(noise.a * _Saturate);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				float4 result = (flame + noise) * gradient;
				return result;
			}
			ENDCG
		}
	}
}
