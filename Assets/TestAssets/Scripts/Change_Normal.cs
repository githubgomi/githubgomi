using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Normal : MonoBehaviour
{
    [SerializeField] float timeScale = 0.5f;
    // Start is called before the first frame update
    public void SwitchTimeScale()
    {
        Time.timeScale = timeScale;
    }
}
