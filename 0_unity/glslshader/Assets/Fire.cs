using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

    public Transform palette;
    private float lastShotAt = -1;
    public float nextShotAt = 0.2f;

    // Update is called once per frame
    void Update ()
    {
	    if (Input.GetMouseButton(0))
        {
            if (lastShotAt + nextShotAt <= Time.time)
            {
                Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                click.z = GetComponent<Transform>().position.z;
                Vector3 pos = click - GetComponent<Transform>().position;
                pos.Normalize();
                Instantiate(palette, GetComponent<Transform>().position, Quaternion.LookRotation(-Vector3.forward, pos));

                lastShotAt = Time.time;
            }
        }
	}
}
