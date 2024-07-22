using UnityEngine;

/// <summary>
/// �U���\���̓����̐���N���X
/// </summary>
public class AttackLine : MonoBehaviour
{
    Vector3 startPos;   // �����n�߂�ʒu
    Vector3 endPos;     // �ړI�n
    const float moveDistance = 3.0f; // �ړ�����
    Vector3 velocity;

    [SerializeField]
    float moveTime;     // �ړ��ɂ����鎞��


    LoadChartData.NoteKind type;

    
    // Start is called before the first frame update
    void Start()
    {
        //type = E_NOTE_TYPE.E_LEFT;

        switch (type)
        {// �I���n�_���v�Z
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

        // �ړI���Ԃ܂ł�B�����邽�߂̃X�s�[�h
        float speed = moveDistance / moveTime;

        // ���x�x�N�g���̌v�Z
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
        // ���Ԓʂ�ɐi�߂�
        if(moveTime > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPos, velocity.magnitude * Time.deltaTime);
        }

        moveTime -= Time.deltaTime;
    }
    /// <summary>
    /// �m�[�c�̏����Z�b�g���܂�
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="type"></param>
    public void SetNote(Vector3 startPos, LoadChartData.NoteKind type)
    {
        this.startPos = startPos;
        this.type = type;
    }
}
