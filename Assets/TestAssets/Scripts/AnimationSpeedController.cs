using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    public Animator animator; // アニメーターコンポーネント
    public float bpm; // 曲のBPM
    public string targetStateName; // 再生速度を変更するステートの名前

    void Start()
    {
        if (animator == null || bpm <= 0 || string.IsNullOrEmpty(targetStateName))
        {
            return;
        }

        // 現在のアニメーションのステート情報を取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // ステートの名前を確認
        if (stateInfo.IsName(targetStateName))
        {
            // 現在のアニメーションのクリップを取得
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            
            if (clipInfo.Length == 0)
            {
                return;
            }

            AnimationClip currentClip = clipInfo[0].clip;

            // アニメーションの総フレーム数を取得
            float totalFrames = currentClip.length * currentClip.frameRate;

            // BPMから1ビートの秒数を計算
            float secondsPerBeat = 60f / bpm;

            // アニメーション全体の再生時間（秒）を計算
            float totalAnimationTime = currentClip.length;

            // アニメーションの再生速度を計算
            float playbackSpeed = totalAnimationTime / (secondsPerBeat * (totalFrames / currentClip.frameRate));

            Debug.Log(playbackSpeed);
            // アニメーションの再生速度を設定
            animator.speed = playbackSpeed / 2;
        }
    }
}
