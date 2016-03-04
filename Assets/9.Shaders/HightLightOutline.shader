Shader "Custom/TestShader" {
		Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { Texgen CubeNormal }
		_OccludeColor ("HighLight Color",Color) = (1,0,0,0.5)
		
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline width", Range (0.0, 0.03)) = .005

	}
	
   	CGINCLUDE
	#include "UnityCG.cginc"
	 
	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 normal : NORMAL;
	};
	 
	struct v2f {
		float4 pos : POSITION;
		float2 texcoord : TEXCOORD0;
		float3 cubenormal : TEXCOORD1;

		float4 color : COLOR;
	};
	 
	uniform float _Outline;
	uniform float4 _OutlineColor;
	 
	v2f vert(appdata v) {
		// just make a copy of incoming vertex data but scaled according to normal direction
		v2f o;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
	 
		float3 norm   = mul ((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);
	 
		o.pos.xy += offset * o.pos.z * _Outline;
		o.color = _OutlineColor;
		return o;
	}
	ENDCG     
  

	//Toony shader
	SubShader {
		Tags { 
			"RenderType"="Opaque"
			"Queue"="Geometry+5"
		}
        //HighLight pass, used when player is behind other objects
        Pass {
            ZWrite Off
			Blend Zero One
            ZTest Greater
			Cull Back

			SetTexture [_OutlineColor] {
				ConstantColor (0,0,0,0)
				Combine constant
			}
        }
        Pass{
            ZWrite Off
			Blend Zero One
            ZTest Greater
			Cull Front
			Blend One OneMinusDstColor
			

			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			 
			half4 frag(v2f i) :COLOR {
				return i.color;
			}
			ENDCG
        }
        //Normal toony pass
		Pass {
			Name "BASE"
            Tags {"LightMode" = "Vertex"}

			Cull Off
            ZWrite On
            SeparateSpecular On

			CGPROGRAM
			#pragma vertex vertexFunction
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;



			v2f vertexFunction (appdata v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
				return o;
			}

			float4 frag (v2f i) : COLOR
			{
				float4 col = _Color * tex2D(_MainTex, i.texcoord);
				clip(col.a);
				float4 cube = texCUBE(_ToonShade, i.cubenormal);
				return float4(2.0f * cube.rgb * col.rgb, col.a);
			}
			ENDCG			
		}
	} 

	
	Fallback "VertexLit"
}
