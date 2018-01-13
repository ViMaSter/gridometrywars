using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public Vector2 borderVertical;
    public Vector2 borderHorizontal;

    public Transform enemyPrefab;
    private int bulk = 5;
    public float interval = 2.0f;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0.0f, interval);
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < bulk; i++)
        {
            Vector2 position = new Vector2(
                Random.Range(Game.World.Instance.Map.Bounds.min.x, Game.World.Instance.Map.Bounds.max.x),
                Random.Range(Game.World.Instance.Map.Bounds.min.y, Game.World.Instance.Map.Bounds.max.y)
            );

            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }
}
