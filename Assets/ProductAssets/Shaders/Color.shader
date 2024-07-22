// ShaderBase

Shader "Unlit/Color"
{
    // エディタ公開情報
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",color) = (1,1,1,1)
    }

    
    SubShader
    {
        // レンダリング方法
        Tags { "RenderType"="Transparent"  "Queue" = "Transparent"} // alpha対応
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        // レンダリング処理
        Pass
        {
            CGPROGRAM
            #pragma vertex vtxShader    // 関数vtxShaderを頂点シェーダーとして利用
            #pragma fragment pixel      // 関数pixelをピクセルシェーダー(フラグメント)として利用
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct VS_IN
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct PS
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            // プロパティ情報のインスタンス化
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            PS vtxShader (VS_IN vin)
            {
                PS pout;
                pout.vertex = UnityObjectToClipPos(vin.vertex);
                pout.uv = TRANSFORM_TEX(vin.uv, _MainTex);
                UNITY_TRANSFER_FOG(pout,pout.vertex);
                return pout;
            }

            fixed4 pixel (PS pin) : SV_Target
            {
                // sample the texture
                 fixed4 col;
                 col= _Color;
                 

                return col;
            }
            ENDCG
        }
    }
}
