using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public float move = 5.0f;
    private Vector3 currentVelocity;

	public Vector3 GetNormalizedVelocity()
    {
        return currentVelocity.normalized;
    }
	
	// Update is called once per frame
	void Update ()
    {
        currentVelocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        { 
            currentVelocity += new Vector3(0, move, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentVelocity += new Vector3(0, -move, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            currentVelocity += new Vector3(-move, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            currentVelocity += new Vector3(move, 0, 0);
        }

        transform.position += currentVelocity * Time.deltaTime;

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

    }
}
