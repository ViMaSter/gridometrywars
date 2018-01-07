using UnityEngine;
using System.Collections;

public class ParticleByVelocity : MonoBehaviour {
    private new ParticleSystem particleSystem;
    private Move moveScript;

    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        moveScript = transform.parent.GetComponent<Move>();
    }

    void Update ()
    {
        particleSystem.startLifetime = moveScript.GetInputVelocity().magnitude;
    }
}
