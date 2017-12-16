using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public float move = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, move, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -move, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-move, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(move, 0, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<Camera>().orthographicSize += move * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<Camera>().orthographicSize -= move * Time.deltaTime;
        }

    }
}
