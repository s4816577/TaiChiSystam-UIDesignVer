Shader "Custom/PlanetRingShadow"
{
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_ColorBK("Back Main Color", Color) = (.5,.5,.5,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Emission("Emission (RGB)", 2D) = "white" {}
		_EmissionPw("Emission Power", Range(0,10)) = 1.0
		_TransparentMap("Transparent Map (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
	}

	SubShader{
		Tags{ 
			"Queue" = "AlphaTest+10" 
			// "Queue" = "Transparent"
			"IgnoreProjector" = "True" 
			"RenderType" = "Transparent" 
		}
		blend SrcAlpha OneMinusSrcAlpha

		LOD 200
		Cull back
		ZWrite Off
		// ZTest Always
		CGPROGRAM
		#pragma surface surf Standard exclude_path:prepass fullforwardshadows decal:blend 

		sampler2D _MainTex, _Emission;
		sampler2D _TransparentMap;
		half _Glossiness, _EmissionPw;
		half _Metallic;
		fixed4 _Color;

		struct Input{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o){
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 ec = tex2D(_Emission, IN.uv_MainTex) / 2.0f;
			o.Albedo = c.rgb;
			o.Alpha = tex2D(_TransparentMap, IN.uv_MainTex).a * _Color.a;
			 o.Emission = ec.rgb * c.rgb * _EmissionPw * o.Alpha;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG

	}

	// Fallback "VertexLit"
	FallBack "Diffuse"
	// Fallback off
}
