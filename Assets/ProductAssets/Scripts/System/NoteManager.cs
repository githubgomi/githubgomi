using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;


public class NoteManager : MonoBehaviour
{
    /// <summary>
    /// ノーツの種類
    /// </summary>
    private enum NoteKind
    {
        Line = 0,   // 予測線
        Attack,     // 攻撃
        MAX
    }

    [SerializeField]
    static GameObject[] prefab = new GameObject[(int)NoteKind.MAX]; // ノーツのプレハブ
    [SerializeField] Transform pos;                                 // プレイヤーをアタッチ
    static Transform copyPos;                                       // Playerの座標

    static Vector3 startPos = new Vector3(0.0f, -5.0f, 0.0f);

    static GameObject noteInstance;    // 実際に生成されたインスタンス
    static Note noteScript;

    [SerializeField]
     Color smallLineColor = Color.red;
    [SerializeField]
     Color bigLineColor = Color.blue;


    static Color color1;
    static Color color2;

    // Start is called before the first frame update
    void Start()
    {
        copyPos = pos;   // Playerの座標取得

#if UNITY_EDITOR
        // 予測線オブジェクト読み込み
        if (!(prefab[(int)NoteKind.Line] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ProductAssets/Prefabs/AttackLine.prefab")))
            Debug.Log("AttackLine.prefab Loaderror");

        // 攻撃ノーツのオブジェクト読み込み
        if (!(prefab[(int)NoteKind.Attack] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ProductAssets/Prefabs/Atk.prefab")))
            Debug.Log("Atk.prefab Loaderror");
#else
        if (!(prefab[(int)NoteKind.Line] = Resources.Load<GameObject>("AttackLine")))
            Debug.Log("AttackLine.prefab LoadError");

        if (!(prefab[(int)NoteKind.Attack] = Resources.Load<GameObject>("Atk")))
            Debug.Log("Atk.prefab LoadError");


#endif

        color1 = smallLineColor;
        color2 = bigLineColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (noteInstance == null) return;

        if (!noteScript.IsEnable)
        {
            Destroy(noteInstance);
            noteInstance = null;
        }      
    }

    /// <summary>
    /// ノーツオブジェクト生成
    /// </summary>
    /// <param name="type"></param>
    static public void CreateNote(LoadChartData.NoteKind type, bool predict,int idx)
    {
        // ノーツが生成されている
        if(noteInstance != null)
            return;

#if UNITY_EDITOR
        // TO_DO エネミー攻撃実装

        // エネミー攻撃ができるまでの仮置き
        GameObject temp = Instantiate(prefab[(int)NoteKind.Line]);

        if ((int)LoadChartData.Data.Notes[idx].Kind <= 3)
        {
            Renderer renderer = temp.GetComponent<Renderer>();
            Material material = new(renderer.sharedMaterials[0]);


            material.SetColor("_Color",color1);

            renderer.material = material;
        }

        else
        {
                Renderer renderer = temp.GetComponent<Renderer>();
                Material material = new Material(renderer.sharedMaterials[0]);

                // 第二引数変えてね
                material.SetColor("_Color", color2);

                renderer.material = material;
        }

            noteInstance =
            Instantiate(temp, startPos, Quaternion.identity);
#else
          noteInstance = 
            Instantiate(temp startPos, Quaternion.identity);
#endif

        noteScript = noteInstance.GetComponent<Note>();

        noteScript.SetNote(type,copyPos);


    }
}
