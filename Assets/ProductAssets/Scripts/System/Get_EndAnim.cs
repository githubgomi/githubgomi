using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_EndAnim : MonoBehaviour
{
    private Animator animator;
    private bool isAnimationFinished = false;

    [SerializeField] GameObject AtkCounterUI;
    [SerializeField] GameObject GameManagerObj;

    private GameManager gameManager;

    void Start()
    {
        animator = GetComponent<Animator>();
        gameManager = GameManagerObj.GetComponent<GameManager>();
    }

    void Update()
    {
        // 再生中のアニメーションの状態を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // アニメーションが終了したかどうかを確認
        if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0))
        {
            if (!isAnimationFinished)
            {
                isAnimationFinished = true;
                ActiveCounterUI();
            }
        }
        else
        {
            // アニメーションが終了していない場合、フラグをリセット
            if (isAnimationFinished)
            {
                isAnimationFinished = false;
            }
        }
    }

    void ActiveCounterUI()
    {
        if (!AtkCounterUI.activeSelf && gameManager.isGameStarted)
        {
            AtkCounterUI.SetActive(true);
        }
    }
}
