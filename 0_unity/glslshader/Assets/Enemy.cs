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
    public Transform residuePrefab;

    private Transform target;
    private float movementForce = 5.0f;



    private void Start()
    {
        movementForce = Random.value * 200 + 200;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        // add movement to player
        Vector3 normalizedDir = target.position - transform.position;
        normalizedDir.Normalize();
        Vector2 targetDir = new Vector2(normalizedDir.x, normalizedDir.y);
        GetComponent<Rigidbody2D>().AddForce(targetDir * movementForce * Time.deltaTime);

        // update rotation based on current velocity
        Vector2 v = GetComponent<Rigidbody2D>().velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Palletes")) == LayerMask.GetMask("Palletes"))
        {
            Debug.Log("Pallete collision!");
            OnDeath();
        }

        if (((1 << collision.gameObject.layer) & LayerMask.GetMask("Player")) == LayerMask.GetMask("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnDeath()
    {
        Instantiate(residuePrefab, gameObject.GetComponent<Transform>().position + new Vector3(0, 0, -1), Quaternion.identity);
        Destroy(gameObject);
    }
}
