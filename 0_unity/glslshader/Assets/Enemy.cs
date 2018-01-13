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

    private enum State
    {
        NONE,
        Spawning,
        Alive
    }

    State currentState = State.NONE;

    [Header("Spawning animation")]
    public float fadeTime = 0.5f;

    [Header("Death properties")]
    public Transform residuePrefab;

    private Transform target;
    private float movementForce = 5.0f;

    private void Start()
    {
        movementForce = Random.value * 200 + 200;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SwitchState(State.Spawning);
    }

    private void SwitchState(State newState)
    {
        switch (newState)
        {
            case State.Spawning:
                StartCoroutine(Spawning());
                break;
        }
        currentState = newState;
    }

    private IEnumerator Spawning()
    {
        yield return new WaitForEndOfFrame();
        SwitchState(State.Alive);
        float startTime = Time.time;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        while ((Time.time - startTime) < fadeTime)
        {
            float size = com.spacepuppy.Tween.ConcreteEaseMethods.BackEaseOutFull(Time.time - startTime, 0.0f, 1.0f, fadeTime, 5.0f);
            if (size > 0.01)
            {
                collider.enabled = true;
            }
            transform.localScale = Vector3.one * size;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = Vector3.one;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Alive:
                Alive();
                break;
        }
    }

    private void Alive()
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

    void OnPaletteCollision()
    {
        Instantiate(residuePrefab, gameObject.GetComponent<Transform>().position + new Vector3(0, 0, -1), Quaternion.identity);
        Destroy(gameObject);
    }
}
