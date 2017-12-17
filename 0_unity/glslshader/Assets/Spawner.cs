using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Vector2 borderVertical;
    public Vector2 borderHorizontal;

    public Transform enemyPrefab;
    private int bulk = 1;
    public float interval = 2.0f;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0.0f, interval);
    }

    void SpawnEnemy()
    {
        bulk += 2;

        for (int i = 0; i < bulk; i++)
        {
            Vector2 position = new Vector2(
                Random.Range(borderHorizontal.x, borderHorizontal.y),
                Random.Range(borderVertical.x, borderVertical.y)
            );

            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }
}
