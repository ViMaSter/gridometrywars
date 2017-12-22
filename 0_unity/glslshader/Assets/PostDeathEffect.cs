using UnityEngine;
using System.Collections;

public class PostDeathEffect : MonoBehaviour
{
    [Header("Shockwave properties")]
    public float radius = 2.0f;
    public float intensity = 25.0f;

    [Header("Light")]
    public com.spacepuppy.Tween.EaseStyle lightEasing;
    public float fadeTime = 3.0f;

    void Start () {
        DoShockwave();
        StartCoroutine(KillAfter(DoLightFade()));
	}

    IEnumerator KillAfter(IEnumerator enumerator)
    {
        Debug.Log("KillAfter Start");
        yield return StartCoroutine(enumerator);
        GameObject.Destroy(gameObject);
        Debug.Log("KillAfter End");
    }

    IEnumerator DoLightFade()
    {
        Light lightComponent = GetComponent<Light>();
        float initialIntensity = lightComponent.intensity;
        float startTime = Time.time;
        Debug.Log("Ease Start Intensity: " + initialIntensity);
        while (lightComponent.intensity > 0)
        {
            lightComponent.intensity = com.spacepuppy.Tween.EaseMethods.GetEase(lightEasing)(Time.time - startTime, initialIntensity, -initialIntensity - 0.01f, fadeTime);
            Debug.Log("Fade: " + lightComponent.intensity);
            yield return new WaitForEndOfFrame();
        }
    }

    void DoShockwave()
    {
        Vector3 currentPosition = GetComponent<Transform>().position;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(currentPosition, radius, LayerMask.GetMask("BG"));
        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitColliders[i].GetComponent<Rigidbody2D>().AddExplosionForce(intensity, currentPosition, radius);
        }
    }
}
