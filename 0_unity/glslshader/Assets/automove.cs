using UnityEngine;
using System.Collections;

public class automove : MonoBehaviour {

    public float speed = 15.0f;

	void Update () {
        transform.position += GetComponent<Transform>().up * speed * Time.deltaTime;
	}
}
