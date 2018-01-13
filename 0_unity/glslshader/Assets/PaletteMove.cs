using UnityEngine;
using System.Collections;

public class PaletteMove : MonoBehaviour {

    public float speed = 15.0f;
    private float deadZoneBuffer = 1.0f;
    public Vector3 originVelocity;

	void Update ()
    {
        MoveWithSpeed();
        ValidateDeadZone();
	}

    public void SetOriginVelocity(Vector3 velocity)
    {
        originVelocity = velocity;
    }

    void MoveWithSpeed()
    {
        transform.position += GetComponent<Transform>().up * speed * Time.deltaTime;
    }

    void ValidateDeadZone()
    {
        if (
            ((transform.position.x + deadZoneBuffer) < Game.World.Instance.Map.Bounds.min.x) ||
            ((transform.position.y + deadZoneBuffer) < Game.World.Instance.Map.Bounds.min.y) ||
            ((transform.position.x - deadZoneBuffer) > Game.World.Instance.Map.Bounds.max.x) ||
            ((transform.position.y - deadZoneBuffer) > Game.World.Instance.Map.Bounds.max.y)
            )
        {
            Destroy(gameObject);
        }
    }
}
