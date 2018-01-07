using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public float move = 5.0f;
    private Vector3 currentVelocity;

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

        transform.position += currentVelocity * move * Time.deltaTime;

        if (!Game.World.Instance.Map.Bounds.ContainBounds(transform.GetComponent<Collider2D>().bounds))
        {
            Vector3 leakingDirection = Game.World.Instance.Map.Bounds.LeakingDirection(transform.GetComponent<Collider2D>().bounds);
            transform.position += leakingDirection;

            if (leakingDirection.x != 0.0f)
            {
                currentVelocity.x = 0;
            }

            if (leakingDirection.y != 0.0f)
            {
                currentVelocity.y = 0;
            }
        }

        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * Mathf.Atan2(currentVelocity.y, currentVelocity.x));
    }
}
