using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private GridManager gridManager;
    public GameObject player;
    public MyPair<GameObject, int>[] enemies;
    void Start()
    {
        gridManager = GridManager.Instance;
        SpawnPlayer();
        SpawnEnemies();
    }

    void SpawnPlayer()
    {
        int height = gridManager.height;
        Vector3Int tilePos = gridManager.FindCellPositionByGridData(1, height - 2);
        Vector3 playerPosition = gridManager.FindCellWorldPosition(tilePos);
        Instantiate(player, playerPosition, Quaternion.identity);
    }
    void SpawnEnemies()
    {
        List<Vector3> spawnableLocations = gridManager.spawnableLocations;
        foreach (var enemy in enemies)
        {
            for (int i = 0; i < enemy.Value; i++)
            {
                int randomIndex = Random.Range(0,spawnableLocations.Count);
                Instantiate(enemy.Key, spawnableLocations[randomIndex], Quaternion.identity);
            }
        }
    }
}
