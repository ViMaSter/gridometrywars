using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

    public Transform paletteObject;
    public UnityLayer paletteTargetLayer;
    public float nextShotAt = 0.2f;

    private float lastShotAt = -1;

    // Update is called once per frame
    void Update ()
    {
	    if (Input.GetAxisRaw("Fire") != 0.0f)
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 thisPos = transform.position;
            Vector2 direction = clickPos - thisPos;
            direction.Normalize();
            RequeustShot(direction);
        }

        if (Input.GetAxisRaw("Fire X") != 0.0f || Input.GetAxisRaw("Fire Y") != 0.0f)
        {
            RequeustShot(new Vector2(Input.GetAxisRaw("Fire X"), Input.GetAxisRaw("Fire Y")));
        }

    }

    void RequeustShot(Vector2 direction)
    {
        if (lastShotAt + nextShotAt <= Time.time)
        {
            Instantiate(paletteObject, transform.position, Quaternion.Euler(0, 0, 180 + Mathf.Rad2Deg * Mathf.Atan2(direction.x, -direction.y)));
            foreach (Pallete palleteScript in paletteObject.GetComponentsInChildren<Pallete>())
            {
                palleteScript.SetLayers(gameObject.layer, paletteTargetLayer);
            }
            paletteObject.GetComponent<PalleteMove>().SetOriginVelocity(GetComponent<Move>().GetIntendedVelocity());

            lastShotAt = Time.time;
        }
    }
}
