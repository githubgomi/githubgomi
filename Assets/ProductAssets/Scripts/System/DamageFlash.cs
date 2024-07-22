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
    float flash = 0.1f;     // 点滅回数

    [SerializeField]
    int flashNum = 5;       // 点滅回数

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
        for(int i = 0; i < flashNum; ++i)
        {
            material.color = new Color(color.r, color.g, color.b,0.0f);

            yield return new WaitForSeconds(flash);

            material.color = color;

            yield return new WaitForSeconds(flash);
        }
    }
}
