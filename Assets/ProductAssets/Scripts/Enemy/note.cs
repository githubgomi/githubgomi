using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Note : MonoBehaviour
{
    [SerializeField] float moveDistancce = 5.0f;// 移動距離
    float moveTime = RhythmDetect.GOOD_RANGE;     // 移動にかかる時間

    private Vector3 startPos;   // 生成される座標
    private Vector3 movePos;    // 移動方向
    private float speed;        // 移動スピード

    bool isEnable = true;       // 使用されているか

    public bool IsEnable{
        get { return isEnable; }
        private set {; }
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = moveDistancce / (moveTime * 60.0f);
    }

    // Update is called once per frame
    void Update()
    {
        // オブジェクトの削除
        if (moveTime * 60.0f < 0.0f)
            isEnable = false;   // マネージャーでインスタンスの削除をしたいため削除フラグで管理(要検討)
    }

    private void FixedUpdate()
    {
        // 座標更新
        startPos = startPos + movePos * speed;
        transform.SetPositionAndRotation(startPos,Quaternion.identity);

        moveTime -= Time.deltaTime;
    }
    
    /// <summary>
    /// ノーツの座標を設定
    /// </summary>
    /// <param name="type"></param>
    /// <param name="pos"></param>
    public void SetNote(LoadChartData.NoteKind type, Transform pos)
    {
        // startPos = 基本座標 +- 方向ベクトル * (移動距離 * 0.5f)
        switch(type)
        {
            case LoadChartData.NoteKind.E_UP_S:
            case LoadChartData.NoteKind.E_UP_L: // Lは後々変更する
                startPos = pos.position + pos.up * (moveDistancce * 0.5f);
                movePos = -pos.up;
                break;
            case LoadChartData.NoteKind.E_DOWN_S:
            case LoadChartData.NoteKind.E_DOWN_L:
                startPos = pos.position + -pos.up * (moveDistancce * 0.5f);
                movePos = pos.up;
                break;
            case LoadChartData.NoteKind.E_LEFT_S:
            case LoadChartData.NoteKind.E_LEFT_L:
                startPos = pos.position + -pos.right * (moveDistancce * 0.5f);
                movePos = pos.right;
                break;
            case LoadChartData.NoteKind.E_RIGHT_S:
            case LoadChartData.NoteKind.E_RIGHT_L:
                startPos = pos.position + pos.right * (moveDistancce * 0.5f);
                movePos = -pos.right;
                break;
            default:
                break;
        }       
    }
}
