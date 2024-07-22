using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EAtkAnimSpeed : MonoBehaviour
{
    /// <summary>
    /// アニメーション速度
    /// </summary>
    public static float Speed { get; private set; } = 1.0f;

    /// <summary>
    /// アニメーションコンポーネント
    /// </summary>
    //[SerializeField] Animator serialize_animator;
    static Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>(); // static変数に取得したanimatorをセット
    }

    // Update is called once per frame
    void Update()
    {
        Speed = LoadChartData.Data.Bpm / 120; // １ビートあたりの秒数
        animator.SetFloat("Speed", Speed);
    }

    public static void SetSpeedToBPM(in float bpm)
    {
        Speed = bpm / 120; // １ビートあたりの秒数
        animator.SetFloat("Speed", Speed);
    }
}
