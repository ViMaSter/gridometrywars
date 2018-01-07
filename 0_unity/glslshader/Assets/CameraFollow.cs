using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    public Transform target;
    private Move moveScript;
    public float targetLerpIntensity = 0.3f;
    private Vector3 currentTargetPosition = Vector3.zero;
    public float velocityLerpIntensity = 0.05f;
    private Vector3 currentVelocityOffset = Vector3.zero;

    public Camera ownCamera;

    void Awake()
    {
        moveScript = target.GetComponent<Move>();
    }

    void UpdatePosition()
    {
        currentTargetPosition = Vector3.Slerp(currentTargetPosition, target.position, targetLerpIntensity);
        // currentVelocityOffset = Vector3.Slerp(currentVelocityOffset, moveScript.GetNormalizedVelocity(), velocityLerpIntensity);
        transform.position = currentTargetPosition + currentVelocityOffset - new Vector3(0, 0, 5);
    }

    void DebugZoom()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ownCamera.orthographicSize += 5.0f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            ownCamera.orthographicSize -= 5.0f * Time.deltaTime;
        }
    }

	void Update()
    {
        DebugZoom();
    }

    void LateUpdate()
    {
        UpdatePosition();
    }
}
