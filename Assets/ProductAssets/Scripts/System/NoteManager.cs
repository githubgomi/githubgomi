using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;


public class NoteManager : MonoBehaviour
{
    /// <summary>
    /// �m�[�c�̎��
    /// </summary>
    private enum NoteKind
    {
        Line = 0,   // �\����
        Attack,     // �U��
        MAX
    }

    [SerializeField]
    static GameObject[] prefab = new GameObject[(int)NoteKind.MAX]; // �m�[�c�̃v���n�u
    [SerializeField] Transform pos;                                 // �v���C���[���A�^�b�`
    static Transform copyPos;                                       // Player�̍��W

    static Vector3 startPos = new Vector3(0.0f, -5.0f, 0.0f);

    static GameObject noteInstance;    // ���ۂɐ������ꂽ�C���X�^���X
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
        copyPos = pos;   // Player�̍��W�擾

#if UNITY_EDITOR
        // �\�����I�u�W�F�N�g�ǂݍ���
        if (!(prefab[(int)NoteKind.Line] = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/ProductAssets/Prefabs/AttackLine.prefab")))
            Debug.Log("AttackLine.prefab Loaderror");

        // �U���m�[�c�̃I�u�W�F�N�g�ǂݍ���
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
    /// �m�[�c�I�u�W�F�N�g����
    /// </summary>
    /// <param name="type"></param>
    static public void CreateNote(LoadChartData.NoteKind type, bool predict,int idx)
    {
        // �m�[�c����������Ă���
        if(noteInstance != null)
            return;

#if UNITY_EDITOR
        // TO_DO �G�l�~�[�U������

        // �G�l�~�[�U�����ł���܂ł̉��u��
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

                // �������ς��Ă�
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
