Shader "Cadpeople/Unlit/OutlinedObject"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineAmount("Outline Amount", Range(0,0.2)) = 0.05
		_IsLit("Is Lit", Range(0,1)) = 1
		//_IsConstScreenSize("Is Constanst Screen Size", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Transparent+1" }
		LOD 100

		Blend SrcAlpha OneMinusSrcAlpha


		Cull Front
		
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
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _OutlineAmount;
				float4 _OutlineColor;
				float _IsConstScreenSize = 0;

				v2f vert(appdata v)
				{
					v2f o;

					float dist = distance(_WorldSpaceCameraPos, UNITY_MATRIX_M._41_42_43);

					float4 vert = v.vertex;
					if (_IsConstScreenSize == 0)
					{
						vert = v.vertex + ( float4(v.normal,1) * _OutlineAmount  );
						o.vertex = UnityObjectToClipPos(vert);

					}
					else
					{
						float3 normal = mul(UNITY_MATRIX_M, v.normal);
						o.vertex = mul( UNITY_MATRIX_M, vert);
						o.vertex = o.vertex + (float4(normal, 1) * _OutlineAmount);// *(_IsConstScreenSize * dist));
						o.vertex = mul( UNITY_MATRIX_VP, o.vertex);
					}



					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = _OutlineColor;
					
					return col;
				}
				ENDCG
			}


			Blend Off
			Cull Back

		Pass
		{
			CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members shade)
			#pragma vertex vert
			#pragma fragment frag


			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float shade : BLENDWEIGHT0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float _IsLit;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);

				//TODO  Calc some simple light based on the the normals and light direction in World Space
				float3 worldNormal = mul(v.normal, UNITY_MATRIX_M);

				

				o.shade = dot( _WorldSpaceLightPos0.xyz, worldNormal) + unity_AmbientSky;
				if (_IsLit == 0) o.shade = 1;


				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) *_Color * i.shade;

				return col;
			}
			ENDCG
		}




	}
}
