using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class pair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;


    public pair(TKey first, TValue second)
    {
        this.Key = first;
        this.Value = second;
    }

}

public class EffectPlay : MonoBehaviour
{
    const int EFFECT_DATA = 5;

    // -----�ϐ��錾
    public Vector3 _playPos = new Vector3(99999, 99999, 99999);

    // �G�t�F�N�g�f�[�^���C���X�y�N�^�[�Őݒ�
    [SerializeField]
    pair<string, GameObject>[] effectData;

    Dictionary<string, GameObject> effect = new Dictionary<string, GameObject>(EFFECT_DATA);

    float lifeTime = 1.3f;

    private pair<string, GameObject> currentEffect = new pair<string, GameObject>("none",null); // ���ݐ�������Ă���W���X�g����G�t�F�N�g�̎Q�Ƃ�ێ�����ϐ�


    // Start is called before the first frame update
    void Start()
    {
       // �ݒ肳�ꂽ�f�[�^��map�Ɉړ�
       for(int i = 0; i < effectData.Length; i++)
       {
            if (effectData[i].Value == null)
                continue;

            effect.Add(effectData[i].Key, effectData[i].Value);
       }

        // �G�f�B�^����f�[�^���ݒ肳��Ă��Ȃ������ꍇ,
        // �A�^�b�`����Ă���I�u�W�F�N�g�̃|�W�V�������擾
        if (_playPos == new Vector3(99999, 99999, 99999))
        {
            _playPos = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// �G�t�F�N�g�𐶐�
    /// </summary>
    /// <param name="effectname">�g�p�������G�t�F�N�g�̖��O</param>
    /// <param name="addPos">���Z�������l default = 0,0,0</param>
    public void InstantiateEffect(in string effectname, in Vector3? addPos = null)
    {
        _playPos = transform.position;
        // ���W�ɃG�t�F�N�g����
        _playPos += addPos ?? Vector3.zero;

        if (currentEffect.Key != effectname)
        {
            currentEffect.Key = effectname;
            currentEffect.Value = Instantiate(effect[effectname], _playPos, Quaternion.identity);

            StartCoroutine(DestroyObject(currentEffect.Value, lifeTime));
        }

    
    }

    /// <summary>
    /// �I�u�W�F�N�g���w�肳�ꂽ���Ԃō폜
    /// </summary>
    /// <param name="obj">�폜�������I�u�W�F�N�g</param>
    /// <param name="deley">��������</param>
    private IEnumerator DestroyObject(GameObject obj, float deley)
    {
        yield return new WaitForSeconds(deley);
        Destroy(obj);
    }
}
