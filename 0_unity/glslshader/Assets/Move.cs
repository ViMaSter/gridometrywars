using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public float move = 5.0f;
    private Vector3 currentVelocity;
    private Vector3 intendedVelocity;

    public Vector3 GetIntendedVelocity()
    {
        return intendedVelocity;
    }

    public Vector3 GetNormalizedVelocity()
    {
        return currentVelocity.normalized;
    }

    public Vector3 GetInputVelocity()
    {
        Vector3 currentVelocity = Vector3.zero;
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            currentVelocity += new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        }
        return currentVelocity;
    }

    // Update is called once per frame
    void Update ()
    {
        currentVelocity = GetInputVelocity();
        intendedVelocity = currentVelocity * move;

        Bounds predictedBounds = transform.GetComponent<Collider2D>().bounds;
        predictedBounds.center += intendedVelocity;
        if (!Game.World.Instance.Map.Bounds.ContainBounds(predictedBounds))
        {
            Vector3 leakingDirection = Game.World.Instance.Map.Bounds.LeakingDirection(transform.GetComponent<Collider2D>().bounds);
            intendedVelocity += leakingDirection;
        }

        transform.position += intendedVelocity * JSAPI.Instance.GetDeltaTime();
        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(currentVelocity.y, currentVelocity.x));
    }
}
