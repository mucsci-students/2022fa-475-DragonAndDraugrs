using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public bool level1 = true;
    public bool level2 = false;
    public bool level3 = false;

    public float level1Time = -1.0f;
    public float level2Time = -1.0f;
    public float level3Time = -1.0f;

    public GameObject playerInstance;

    private void Awake()
    {
        int num = FindObjectsOfType<LevelSelectManager>().Length;
        if (num != 1) return;

        DontDestroyOnLoad(this);
    }
}
