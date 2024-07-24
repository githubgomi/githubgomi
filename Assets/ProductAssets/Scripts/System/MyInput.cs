using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyInput : MonoBehaviour
{
    /// <summary>
    /// キー種類
    /// </summary>
    public enum Key
    {
        Up,
        Down,
        Left,
        Right,
        BigOption,

        Max
    }

    /// <summary>
    /// 数値の入力を受け付けるまでのデッドゾーン
    /// </summary>
    [SerializeField] float deadZone = 0.7f;

    //--- private
    private static bool[] oldInput = new bool[(int)Key.Max];
    private static bool[] curtInput = new bool[(int)Key.Max];

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 前回の入力を保持
        for(int i = 0; i < (int)Key.Max; ++i) { oldInput[i] = curtInput[i]; }

        // 初期化
        for(int i = 0; i < (int)Key.Max; ++i) { curtInput[i] = false; }
        
        // 判定
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // スティック + 十字キー + 十字ボタン
        if (vertical > deadZone)        { curtInput[(int)Key.Up] = true; }
        else if (vertical < -deadZone)  { curtInput[(int)Key.Down] = true; }
        else if (horizontal < -deadZone){ curtInput[(int)Key.Left] = true; }
        else if (horizontal > deadZone) { curtInput[(int)Key.Right] = true; }

        // Aボタン(xbox) + 左シフト
        if (Input.GetKey("joystick button 0") || Input.GetKey(KeyCode.LeftShift))
        //if (Input.GetKey("BigOption"))
        {
            curtInput[(int)Key.BigOption] = true;
        }
    }

    /// <summary>
    /// 入力されているか判定
    /// </summary>
    /// <param name="kind">種類</param>
    /// <returns>入力されている？</returns>
    public static bool GetKey(in Key kind)
    {
        return curtInput[(int)kind];
    }

    /// <summary>
    /// 押された瞬間かを判定
    /// </summary>
    /// <param name="kind">種類</param>
    /// <returns>押された瞬間？</returns>
    public static bool GetKeyDown(in Key kind)
    {
        return (oldInput[(int)kind] ^ curtInput[(int)kind]) & curtInput[(int)kind];
    }

    /// <summary>
    /// 離れた瞬間かを判定
    /// </summary>
    /// <param name="kind">種類</param>
    /// <returns>離れた瞬間？</returns>
    public static bool GetKeyRelease(in Key kind)
    {
        return (oldInput[(int)kind] ^ curtInput[(int)kind]) & oldInput[(int)kind];
    }


    public static bool GetKeyDown(in LoadChartData.NoteKind kind)
    {
        switch (kind)
        {
            // 小回避
            case LoadChartData.NoteKind.E_UP_S:
            case LoadChartData.NoteKind.E_DOWN_S:
            case LoadChartData.NoteKind.E_LEFT_S:
            case LoadChartData.NoteKind.E_RIGHT_S:
                return IsKeyDownSmall((Key)kind);

            // 大回避
            case LoadChartData.NoteKind.E_UP_L:
            case LoadChartData.NoteKind.E_DOWN_L:
            case LoadChartData.NoteKind.E_LEFT_L:
            case LoadChartData.NoteKind.E_RIGHT_L:
                return IsKeyDownBig((Key)((int)kind - 4));

            default: return false;
        }
    }

    //--- サブルーチン
    static bool IsKeyDownBig(in Key key)
    {
        // どちらか片方のキーが押されている状況から
        // 両方のキーが入力されたフレームを判断
        bool old = oldInput[(int)Key.BigOption] & oldInput[(int)key];
        bool curt = curtInput[(int)Key.BigOption] & curtInput[(int)key];
    
        return (old ^ curt) & curt;
    }

    static bool IsKeyDownSmall(in Key key)
    {
        return (GetKeyDown(key) ^ curtInput[(int)Key.BigOption]) & GetKeyDown(key);
    }
}
