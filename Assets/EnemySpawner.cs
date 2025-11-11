using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab del enemigo
    public float spawnInterval = 2f; // Cada cuántos segundos aparece uno

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        // Instancia un enemigo en la posición del Spawner
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}
