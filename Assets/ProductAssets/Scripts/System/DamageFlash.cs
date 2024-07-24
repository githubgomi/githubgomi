using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField]
    Renderer renderer;

    Material material;
    Material flashMaterial;

    Color color;
    public Color Color
    {
        get { return color; }
        set { color = value; }
    }

    [SerializeField]
    [Tooltip("点滅間隔")]
    float flash = 0.1f;     // 点滅回数

    [SerializeField]
    [Tooltip("点滅回数")]
    int flashNum = 5;       // 点滅回数

    [SerializeField]
    [Tooltip("元に戻るまでの時間")]
    float flashDuration = -999; // 透明から元の色に戻るまでの時間

    // Start is called before the first frame update
    void Start()
    {
        if (renderer != null)
        {
            material = renderer.material;
            color = material.color;

            // マテリアルのレンダリングモードを変更
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        }

        if(flashDuration <= 0)
        flashDuration = flash;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        float elapsedTime;
        float flashDuration = flash; // 透明から元の色に戻るまでの時間
        Color originalColor = material.color;
        Color transparentColor = new Color(color.r, color.g, color.b, 0.2f);

        for (int i = 0; i < flashNum; ++i)
        {
            // 透明に変化する
            material.color = transparentColor;
            yield return new WaitForSeconds(flashDuration);

            // 元の色に戻る
            elapsedTime = 0f;
            while (elapsedTime < flashDuration)
            {
                material.color = Color.Lerp(transparentColor, originalColor, elapsedTime / flashDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            material.color = originalColor;

            yield return new WaitForSeconds(flashDuration);
        }
    }

}
