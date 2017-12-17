using UnityEngine;
using System.Collections;

public static class Rigidbody2DExtension
{
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * (wearoff <= 0f ? 0f : explosionForce) * wearoff);
    }
}

public class Enemy : MonoBehaviour {

    [Header("Death properties")]
    public float radius = 4.0f;
    public float intensity = 2.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision!");
        OnDeath();
    }

    void OnDeath()
    {
        KnockBack();
	}

    void KnockBack()
    {
        Vector3 currentPosition = GetComponent<Transform>().position;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentPosition, radius, LayerMask.GetMask("BG"));
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].GetComponent<Rigidbody2D>().AddExplosionForce(intensity, currentPosition, radius);
        }
        Destroy(gameObject);
    }
}
