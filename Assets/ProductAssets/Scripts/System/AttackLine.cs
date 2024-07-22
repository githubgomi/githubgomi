using UnityEngine;

/// <summary>
/// 攻撃予測の動きの制御クラス
/// </summary>
public class AttackLine : MonoBehaviour
{
    Vector3 startPos;   // 動き始める位置
    Vector3 endPos;     // 目的地
    const float moveDistance = 3.0f; // 移動距離
    Vector3 velocity;

    [SerializeField]
    float moveTime;     // 移動にかける時間


    LoadChartData.NoteKind type;

    
    // Start is called before the first frame update
    void Start()
    {
        //type = E_NOTE_TYPE.E_LEFT;

        switch (type)
        {// 終わり地点を計算
            case LoadChartData.NoteKind.E_UP_S:
                endPos = startPos + -Vector3.up * moveDistance;
                break;

            case LoadChartData.NoteKind.E_DOWN_S:
                endPos = startPos + Vector3.up * moveDistance;
                break;

            case LoadChartData.NoteKind.E_LEFT_S:
                endPos = startPos + Vector3.right * moveDistance;
                break;

            case LoadChartData.NoteKind.E_RIGHT_S:
                endPos = startPos + -Vector3.right * moveDistance;
                break;

        }

        // 目的時間までを達成するためのスピード
        float speed = moveDistance / moveTime;

        // 速度ベクトルの計算
        velocity = (endPos - startPos).normalized * speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTime < 0.0f)
            Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        // 時間通りに進める
        if(moveTime > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, velocity.magnitude * Time.deltaTime);
        }

        moveTime -= Time.deltaTime;
    }
    /// <summary>
    /// ノーツの情報をセットします
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="type"></param>
    public void SetNote(Vector3 startPos, LoadChartData.NoteKind type)
    {
        this.startPos = startPos;
        this.type = type;
    }
}
