using UnityEngine;
using System.Collections;

public class JSAPI : MonoBehaviour
{
    private static JSAPI _Instance;
    public static JSAPI Instance
    {
        get
        {
            return _Instance;
        }
    }
    public float GetDeltaTime()
    {
        if (deltaTime)
        {
            return Time.deltaTime;
        }
        else
        {
            return 0.01f;
        }
    }
    public void Awake()
    {
        JSAPI._Instance = this;
    }

    private float frameRate = 30;
    public void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            frameRate += 10 * Time.deltaTime;
            SetTargetFramerate((int)frameRate);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            frameRate -= 10 * Time.deltaTime;
            frameRate = Mathf.Max(1, frameRate);
            SetTargetFramerate((int)frameRate);
        }
        if (Input.GetKey(KeyCode.Y))
        {
            SetDeltaTime(deltaTime ? 0 : 1);
        }
    }

    public void SetTargetFramerate(int frameRate)
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
        Debug.Log(frameRate);
    }

    private bool deltaTime = true;
    public void SetDeltaTime(int newState)
    {
        deltaTime = newState != 0;
    }
}
