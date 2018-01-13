using UnityEngine;
using System.Collections;

public class Pallete : MonoBehaviour
{
    public UnityLayer originLayer;
    public UnityLayer targetLayer;
    public void SetLayers(UnityLayer origin, UnityLayer target)
    {
        originLayer = origin;
        targetLayer = target;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer & targetLayer) == targetLayer)
        {
            collision.gameObject.SendMessage("OnPaletteCollision");
        }

        if ((collision.gameObject.layer & originLayer) == originLayer)
        {
            return;
        }

        Destroy(gameObject);
    }
}
