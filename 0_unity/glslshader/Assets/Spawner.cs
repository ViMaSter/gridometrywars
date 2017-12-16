using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Vector2 range;
    public Transform Obj;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnObject", 0.0f, Random.Range(1.0f, 2.0f));
    }

    // Update is called once per frame
    void SpawnObject()
    {
        Instantiate(Obj, new Vector3(Random.Range(range.x, range.y), 20.0f, 0.0f), Quaternion.identity);
    }
}
