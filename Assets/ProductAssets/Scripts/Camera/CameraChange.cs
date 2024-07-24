using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

using UnityEngine.Playables;
public class CameraChange : MonoBehaviour
{
    // ------ 定数定義 ------
    const float DEFAULT_TIMESCALE = 1.0f;
    const int PRIORITY_CAM = 1;
    const int NOT_PRIORITY = 0;

    public enum E_CAMERA_KIND
    {
        TYPE_DEFAULT,
        TYPE_S_LEFT,
        TYPE_B_LEFT,
        TYPE_S_RIGHT,
        TYPE_B_RIGHT,
        TYPE_S_UP,
        TYPE_B_UP,
        TYPE_S_DOWN,
        TYPE_B_DOWN,

        TYPE_MAX,
    };



    [SerializeField] private CinemachineVirtualCamera vDefaultCam;

    [SerializeField] GameObject AtkCounterUI;

    [SerializeField] private float delayTime;

    Camera cam;             //　メインカメラ格納用
    bool isMove = false;    // 移動フラグ
    Vector3 pos;            // カメラの現在座標
    Vector3 old_pos;        //　カメラの過去座標




    // yyyyy
    [SerializeField] private CinemachineVirtualCamera[] vCameras = new CinemachineVirtualCamera[(int)E_CAMERA_KIND.TYPE_MAX];

    E_CAMERA_KIND NowCamera;

    // Start is called before the first frame update
    void Start()
    {

        // カメラの取得
        cam = Camera.main;
        pos = cam.transform.position;

        NowCamera = E_CAMERA_KIND.TYPE_DEFAULT;

        int index = -1;
        foreach (var i in vCameras)
        {
            index++;

            vCameras[index].Priority = 0;
        }
        vCameras[0].Priority = 1;

    }

    // Update is called once per frame
    void Update()
    {
        // 過去座標の更新
        old_pos = pos;

        // 現在座標の取得
        pos = cam.transform.position;
    }

    void ChangeMainCamera()
    {
        vCameras[(int)NowCamera].Priority = NOT_PRIORITY;
        vCameras[(int)E_CAMERA_KIND.TYPE_DEFAULT].Priority = PRIORITY_CAM;
    }

    public void ChangeCam(LoadChartData.NoteKind kind)
    {
        vCameras[0].Priority = NOT_PRIORITY;
        switch (kind)
        {
            case LoadChartData.NoteKind.E_UP_L:
                SetCamera(E_CAMERA_KIND.TYPE_B_UP);
                break;

            case LoadChartData.NoteKind.E_UP_S:
                SetCamera(E_CAMERA_KIND.TYPE_S_UP);
                break;

            case LoadChartData.NoteKind.E_DOWN_S:
                SetCamera(E_CAMERA_KIND.TYPE_S_DOWN);
                break;

            case LoadChartData.NoteKind.E_DOWN_L:
                SetCamera(E_CAMERA_KIND.TYPE_B_DOWN);
                break;

            case LoadChartData.NoteKind.E_LEFT_S:
                SetCamera(E_CAMERA_KIND.TYPE_S_LEFT);
                break;

            case LoadChartData.NoteKind.E_LEFT_L:
                SetCamera(E_CAMERA_KIND.TYPE_B_LEFT);
                break;

            case LoadChartData.NoteKind.E_RIGHT_S:
                SetCamera(E_CAMERA_KIND.TYPE_S_RIGHT);
                break;

            case LoadChartData.NoteKind.E_RIGHT_L:
                SetCamera(E_CAMERA_KIND.TYPE_B_RIGHT);
                break;
        }

        Invoke(nameof(ChangeMainCamera), delayTime);

    }

    void SetCamera(E_CAMERA_KIND cameraKind)
    {
        AtkCounterUI.SetActive(false);

        vCameras[(int)cameraKind].Priority = PRIORITY_CAM;
        vCameras[(int)cameraKind].gameObject.GetComponent<PlayableDirector>().Play();

        vCameras[(int)cameraKind].m_Lens.FieldOfView = 60.0f;
        NowCamera = cameraKind;
    }

}
