using UnityEngine;
using System.Collections;

public class ExplosionForce : MonoBehaviour {
	// Use this for initialization
	void Start () {
        StartCoroutine(KillAfterFrame());
	}

    IEnumerator KillAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        GameObject.Destroy(gameObject);
    }

}
