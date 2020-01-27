// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Mobile Xray Shader created by Bruno Rime 2012
// Free for personal and comercial use

Shader "XRay" {
	Properties {
		_Color("_Color", Color) = (0,1,0,1)
	    //_Inside("_Inside", Range(0,1) ) = 0
	   // _Rim("_Rim", Range(0,2) ) = 1.2
		_XColor("_XColor", Color) = (0,1,0,1)

		

	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		LOD 80
		
	Pass {
		Name "Darken"
		//Cull off
		Zwrite off
		Blend dstcolor zero
		ZTEST GREATER
				
		CGPROGRAM
		
		#pragma vertex vert_surf
		#pragma fragment frag_surf
		#pragma fragmentoption ARB_precision_hint_fastest
		//#pragma multi_compile_fwdbase

		#include "HLSLSupport.cginc"
		#include "UnityCG.cginc"


		struct v2f_surf {
			  half4 pos 	: SV_POSITION;
			  fixed4 finalColor : COLOR;
		};
		
		uniform half4 _Color;
		//uniform half _Rim;
		//uniform half _Inside;
		v2f_surf vert_surf (appdata_base v) {
		v2f_surf o;
			
			o.pos = UnityObjectToClipPos (v.vertex);
			half3 uv = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal );
			uv = normalize(uv);
			o.finalColor = _Color;
			return o;
		}
		
		fixed4 frag_surf (v2f_surf IN) : COLOR {

			return IN.finalColor;
		}
 
	ENDCG
	}
	
	Pass {
		Name "Lighten"
		Cull off
		Zwrite off
		ZTEST GREATER
		Blend oneminusdstcolor one
				
		CGPROGRAM
		
		#pragma vertex vert_surf
		#pragma fragment frag_surf
		#pragma fragmentoption ARB_precision_hint_fastest
		//#pragma multi_compile_fwdbase

		#include "HLSLSupport.cginc"
		#include "UnityCG.cginc"


		struct v2f_surf {
			  half4 pos 	: SV_POSITION;
			  fixed4 finalColor : COLOR;
		};
		
		uniform half4 _Color;
		//uniform half _Rim;
		//uniform half _Inside;

		v2f_surf vert_surf (appdata_base v) {
		v2f_surf o;
			
			o.pos = UnityObjectToClipPos (v.vertex);
			half3 uv = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal );
			uv = normalize(uv);
			o.finalColor = _Color;
		}
		
		fixed4 frag_surf (v2f_surf IN) : COLOR {

			return IN.finalColor;
		}
 
	ENDCG
	}
	
}

FallBack "Mobile/VertexLit"
}