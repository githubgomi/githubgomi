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

    // -----変数宣言
    public Vector3 _playPos = new Vector3(99999, 99999, 99999);

    // エフェクトデータをインスペクターで設定
    [SerializeField]
    pair<string, GameObject>[] effectData;

    Dictionary<string, GameObject> effect = new Dictionary<string, GameObject>(EFFECT_DATA);

    float lifeTime = 1.0f;

    private pair<string, GameObject> currentEffect = new pair<string, GameObject>("none",null); // 現在生成されているジャスト回避エフェクトの参照を保持する変数


    // Start is called before the first frame update
    void Start()
    {
       // 設定されたデータをmapに移動
       for(int i = 0; i < effectData.Length; i++)
       {
            if (effectData[i].Value == null)
                continue;

            effect.Add(effectData[i].Key, effectData[i].Value);
       }

        // エディタからデータが設定されていなかった場合,
        // アタッチされているオブジェクトのポジションを取得
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
    /// エフェクトを生成
    /// </summary>
    /// <param name="effectname">使用したいエフェクトの名前</param>
    /// <param name="addPos">加算したい値 default = 0,0,0</param>
    public void InstantiateEffect(in string effectname, in Vector3? addPos = null)
    {
        _playPos = transform.position;
        // 座標にエフェクト生成
        _playPos += addPos ?? Vector3.zero;

        if (currentEffect.Key != effectname)
        {
            currentEffect.Key = effectname;
            currentEffect.Value = Instantiate(effect[effectname], _playPos, Quaternion.identity);

            StartCoroutine(DestroyObject(currentEffect.Value, lifeTime));
        }

    
    }

    /// <summary>
    /// オブジェクトを指定された時間で削除
    /// </summary>
    /// <param name="obj">削除したいオブジェクト</param>
    /// <param name="deley">生存時間</param>
    private IEnumerator DestroyObject(GameObject obj, float deley)
    {
        yield return new WaitForSeconds(deley);
        Destroy(obj);
    }
}
