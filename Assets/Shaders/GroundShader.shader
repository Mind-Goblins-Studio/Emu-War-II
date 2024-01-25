Shader "Custom/GroundShader"
{
    Properties
    {
        // textures
        _GroundTex ("Ground Texture", 2D) = "white" {}
        _PathTex ("Path Texture", 2D) = "white" {}
        _MaskTex ("Path Mask", 2D) = "white" {}
        
        // texture tiling values
        _GroundTexTiling ("Ground Texture Tiling", Vector) = (1, 1, 0, 0)
        _PathTexTiling ("Path Texture Tiling", Vector) = (1, 1, 0, 0)
        
        // texture hue values
        _GroundColour ("Ground Colour", Color) = (1, 1, 1, 1)
        _PathColour ("Path Colour", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Pass
        {
            // cull back to save on rendering
            Cull Back
            
            CGPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            // properties
            sampler2D _GroundTex;
            sampler2D _PathTex;
            sampler2D _MaskTex;
            float4 _GroundTexTiling;
            float4 _PathTexTiling;
            float4 _GroundColour;
            float4 _PathColour;

            // in vertex struct
            struct vertIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // out vertex struct
            struct vertOut
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            // vert function
            vertOut vert (vertIn v)
            {
                // convert vertex position & apply uv
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // frag function
            fixed4 frag (vertOut i) : SV_Target
            {
                // apply textures & tiling, overlay colour hue adjustment
                fixed4 groundCol = tex2D(_GroundTex, i.uv * _GroundTexTiling.xy + _GroundTexTiling.zw) * _GroundColour;
                fixed4 pathCol = tex2D(_PathTex, i.uv * _PathTexTiling.xy + _PathTexTiling.zw) * _PathColour;
                half mask = tex2D(_MaskTex, i.uv).r;

                // use mask to smooth transition between ground and path texture accordingly
                fixed4 col = lerp(groundCol, pathCol, mask);
                return col;
            }
            ENDCG
        }
    }
}