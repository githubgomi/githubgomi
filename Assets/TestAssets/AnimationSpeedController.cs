using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    public Animator animator; // アニメーターコンポーネント
    public float bpm; // 曲のBPM

    void Start()
    {
        if (animator == null)
        {
            Debug.LogError("Animatorがナイ！");
            return;
        }

        if (bpm <= 0)
        {
            return;
        }

        // 現在のアニメーションのクリップを取得
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        if (clipInfo.Length == 0)
        {
            Debug.LogError("再生中のアニメーションがナイ！");
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

        // アニメーションの再生速度を設定
        animator.speed = playbackSpeed / 2;
    }
}
