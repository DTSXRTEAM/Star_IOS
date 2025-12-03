Shader "Custom/Blink" {
	Properties{
		_MainTex("Particle Texture", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1)
	}

		SubShader{
		 Tags {"RenderType" = "Fade" "Queue" = "Transparent"}
		Blend SrcAlpha One

		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		float3 _Color;

		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			half4 c = tex2D(_MainTex, IN.uv_MainTex);

			c *= ( abs(sin(_Time.w/2)));
			
			o.Albedo = c.rgb*_Color;
			o.Alpha = c.a / 2;
		}
		ENDCG
			}
				FallBack "Diffuse"

}
