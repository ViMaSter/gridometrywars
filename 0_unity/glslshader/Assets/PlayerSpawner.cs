using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {

    public Transform player;

	void Start ()
    {
        Instantiate(player, transform.position, Quaternion.identity);
        Destroy(gameObject);
	}
}
