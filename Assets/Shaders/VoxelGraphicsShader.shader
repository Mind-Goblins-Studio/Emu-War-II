// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/VoxelGraphicsShader" {
	
	Properties {
		_OutlineColor ("Outline Color", Color) = (0,0,0,1)
		_Outline ("Outline Thickness", float) = 0.025
		
		_MainTex ("Texture", 2D) = "white" { }
	}
	
	SubShader
	{
		// PASS 1: Base Object (Texture Shading)
		Pass 
		{
			// cull back to save on rendering
			Cull Back
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			// Properties
			uniform sampler2D _MainTex;

			// In Vertex Struct
			struct vertIn {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			// Out Vertex Struct
			struct vertOut {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			// Vert Function
			vertOut vert(vertIn v)
			{
				// apply texture with uv
				vertOut o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			// Frag Function
			fixed4 frag(vertOut v) : SV_Target
			{
				// colour with texture
				fixed4 col = tex2D(_MainTex, v.uv);
				return col;
			}

			ENDCG
		}
		
		// PASS 2: Outline of Object (Outline Shading)
		Pass
		{
			// cull front in order to only get outlines
			Cull Front
			
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
						
			#include "UnityCG.cginc"

			// Properties
			uniform float _Outline;
			uniform float4 _OutlineColor;

			// In Vertex Struct
			struct vertIn {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			// Out Vertex Struct
			struct vertOut {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
			};

			// Vert Function
			vertOut vert(vertIn v)
			{
				// using normal to offset to outline vertex
				vertOut o;
				v.vertex = v.vertex + (float4(v.normal, 0) * _Outline);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			// Frag Function
			fixed4 frag(vertOut o) : SV_Target
			{
				// colour with outline colour
				fixed4 col = _OutlineColor;
				return col;
			}

			ENDCG
		}
	}
}
