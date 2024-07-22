// ShaderBase

Shader "Unlit/Color"
{
    // �G�f�B�^���J���
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",color) = (1,1,1,1)
    }

    
    SubShader
    {
        // �����_�����O���@
        Tags { "RenderType"="Transparent"  "Queue" = "Transparent"} // alpha�Ή�
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        // �����_�����O����
        Pass
        {
            CGPROGRAM
            #pragma vertex vtxShader    // �֐�vtxShader�𒸓_�V�F�[�_�[�Ƃ��ė��p
            #pragma fragment pixel      // �֐�pixel���s�N�Z���V�F�[�_�[(�t���O�����g)�Ƃ��ė��p
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

            // �v���p�e�B���̃C���X�^���X��
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
