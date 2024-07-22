using UnityEngine;
using UnityEngine.UI;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
public class S_Bloom : MonoBehaviour
{
    [SerializeField]
    private Image _image;

    [SerializeField]
    private Image _glowImage;

    /// <summary> �����F </summary>
    [SerializeField]
    private Color _glowColor = Color.white;

    [SerializeField]
    private Material material;

    /// <summary> ��悹�摜�̂ڂ������� </summary>
    [SerializeField]
    private float _blurSig = 5f;

    // �ڂ����摜�̍Đ�������p
    private float _preBlurSig;
    private Sprite _preOrifinSprite;

    /// <summary>
    /// �N����
    /// </summary>
    private void Awake()
    {
        UpdateGlow();
    }

    /// <summary>
    /// Inspector��̒l���ύX���ꂽ�Ƃ��ɌĂяo��
    /// </summary>
    private void OnValidate()
    {
        UpdateGlow();
    }

    /// <summary>
    /// ������
    /// </summary>
    private void Initialize()
    {
        _image = GetComponent<Image>();

        // ��悹���锭���\���p��Image�̐���
        _glowImage = new GameObject("Glow", typeof(Image)).GetComponent<Image>();
        // ���Z�����}�e���A����ݒ�
        _glowImage.material = material;
        _glowImage.transform.SetParent(_image.transform, false);
        _glowImage.gameObject.layer = _image.gameObject.layer;
        _glowImage.rectTransform.sizeDelta = _image.rectTransform.sizeDelta;

        _preBlurSig = _blurSig;
        _preOrifinSprite = _image.sprite;
    }

    /// <summary>
    /// �����\���p�̂ڂ����摜�̍X�V
    /// </summary>
    private void UpdateGlow()
    {
        if (_image == null)
        {
            // �x�[�X��Image���擾���Ă��Ȃ������珉����
            Initialize();
        }

        if (_image.sprite == null)
        {
            // �x�[�X��Image�̉摜���ݒ肳��Ă��Ȃ������牽�����Ȃ�
            _glowImage.sprite = null;
            return;
        }

        _glowImage.color = _glowColor;

        if (_glowImage.sprite != null && _preBlurSig == _blurSig && _preOrifinSprite == _image.sprite)
        {
            // �ڂ��������ƃx�[�XImage�ɕύX���Ȃ���΂ڂ����摜�̍Đ��������Ȃ�
            return;
        }

        Sprite preGlowSprite = _glowImage.sprite;

        Texture2D blurTex = CreateBlurTexture(_image.sprite.texture, _blurSig);
        Sprite blurSprite = Sprite.Create(blurTex, _image.sprite.rect, _image.rectTransform.pivot);
        _glowImage.sprite = blurSprite;
        _glowImage.rectTransform.sizeDelta = _image.rectTransform.sizeDelta;

#if UNITY_EDITOR
        // �G�f�B�^�[�ł̂ݕ�����₷���悤�ɃX�v���C�g��������
        blurSprite.name = _image.sprite.name + " blur";
#endif

        // �g��Ȃ��Ȃ����X�v���C�g��j��
        DestroySprite(preGlowSprite);

        _preBlurSig = _blurSig;
        _preOrifinSprite = _image.sprite;
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        _glowImage.rectTransform.sizeDelta = _image.rectTransform.sizeDelta;
        _glowImage.color = _glowColor;
    }

    /// <summary>
    /// �폜���ɌĂяo��
    /// </summary>
    private void OnDestroy()
    {
        DestroySprite(_glowImage.sprite);
    }

    /// <summary>
    /// �X�v���C�g�̔j��
    /// </summary>
    private void DestroySprite(Sprite sprite)
    {
        if (sprite == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(sprite.texture);
            Destroy(sprite);
        }
        else
        {
            // �G�f�B�^�[���[�h�ł͑����j��
            DestroyImmediate(sprite.texture);
            DestroyImmediate(sprite);
        }
    }

    /// <summary>
    /// �ڂ����摜�𐶐�
    /// https://qiita.com/divideby_zero/items/4c02177a56f7d500d4c0
    /// </summary>
    /// <param name="sig">�ڂ�������</param>
    private static Texture2D CreateBlurTexture(Texture2D tex, float sig)
    {
        sig = Mathf.Max(sig, 0f);
        int W = tex.width;
        int H = tex.height;
        int Wm = (int)(Mathf.Ceil(3.0f * sig) * 2 + 1);
        int Rm = (Wm - 1) / 2;

        //�t�B���^
        float[] msk = new float[Wm];

        sig = 2 * sig * sig;
        float div = Mathf.Sqrt(sig * Mathf.PI);

        //�t�B���^�̍쐬
        for (int x = 0; x < Wm; x++)
        {
            int p = (x - Rm) * (x - Rm);
            msk[x] = Mathf.Exp(-p / sig) / div;
        }

        var src = tex.GetPixels(0).Select(x => x.a).ToArray();
        var tmp = new float[src.Length];
        var dst = new Color[src.Length];

        //��������
        for (int x = 0; x < W; x++)
        {
            for (int y = 0; y < H; y++)
            {
                float sum = 0;
                for (int i = 0; i < Wm; i++)
                {
                    int p = y + i - Rm;
                    if (p < 0 || p >= H) continue;
                    sum += msk[i] * src[x + p * W];
                }
                tmp[x + y * W] = sum;
            }
        }
        //��������
        for (int x = 0; x < W; x++)
        {
            for (int y = 0; y < H; y++)
            {
                float sum = 0;
                for (int i = 0; i < Wm; i++)
                {
                    int p = x + i - Rm;
                    if (p < 0 || p >= W) continue;
                    sum += msk[i] * tmp[p + y * W];
                }
                dst[x + y * W] = new Color(1, 1, 1, sum);
            }
        }

        var createTexture = new Texture2D(W, H);
        createTexture.SetPixels(dst);
        createTexture.Apply();

        return createTexture;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(S_Bloom))]
    public class ImageFakeBloomEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            S_Bloom target = this.target as S_Bloom;
            if (target._glowImage == null || target._glowImage.sprite == null)
            {
                return;
            }

            // �����\���p�ɐ������ꂽ�摜���v���W�F�N�g�ɕۑ�����{�^��
            if (GUILayout.Button("�����摜��ۑ�"))
            {
                SaveBlurSprite(target._glowImage.sprite);
            }
        }

        /// <summary>
        /// �n���ꂽSprite���v���W�F�N�g�ɕۑ�
        /// </summary>
        private void SaveBlurSprite(Sprite sprite)
        {
            string path = EditorUtility.SaveFilePanelInProject("�����摜��ۑ�", sprite.name, "png", "�ۑ�����摜������͂��Ă�������");
            if (path.Length == 0)
            {
                // �ۑ��L�����Z��
                return;
            }

            byte[] pngData = sprite.texture.EncodeToPNG();
            if (pngData == null)
            {
                Debug.LogError("�摜�f�[�^�̎擾�Ɏ��s���܂���");
                return;
            }

            File.WriteAllBytes(path, pngData);
            AssetDatabase.Refresh();

            // �e�N�X�`���^�C�v��Sprite�ɕύX
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
            if (textureImporter != null)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                textureImporter.SaveAndReimport();
            }
        }
    }
#endif
}
