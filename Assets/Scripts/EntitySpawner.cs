using System.Collections.Generic;
using System;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    private GridManager gridManager;
    public GameObject playerObject;
    private GameObject player;
    public MyPair<GameObject, int>[] enemies;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private List<Vector3> crazySpawnableLocations = new List<Vector3>();
    void Start()
    {
        gridManager = GridManager.Instance;
        SpawnplayerObject();
        SpawnEnemies();
    }
    void OnEnable()
    {
        GameManager.OnTimeUp += MakeEnemiesCrazy; 
    }
    void OnDisable()
    {
        GameManager.OnTimeUp -= MakeEnemiesCrazy; 
    }
    void SpawnplayerObject()
    {
        int height = gridManager.height;
        Vector3Int tilePos = gridManager.FindCellPositionByGridData(1, height - 2);
        Vector3 playerObjectPosition = gridManager.FindCellWorldPosition(tilePos);
        player = Instantiate(playerObject, playerObjectPosition, Quaternion.identity);
    }
    void SpawnEnemies()
    {
        List<Vector3> spawnableLocations = gridManager.spawnableLocations;
        foreach (var enemy in enemies)
        {
            for (int i = 0; i < enemy.Value; i++)
            {
                if (spawnableLocations.Count == 0) return;
                int randomIndex = UnityEngine.Random.Range(0,spawnableLocations.Count);
                GameObject enemyObject = enemy.Key;
                GameObject spawnedEnemy = Instantiate(enemyObject, spawnableLocations[randomIndex], Quaternion.identity);
                spawnableLocations.RemoveAt(randomIndex);
                spawnedEnemies.Add(spawnedEnemy);
            }
        }
    }

    void MakeEnemiesCrazy()
    {
        SetCrazyPositions();
        foreach (var enemy in spawnedEnemies)
        {
            if (crazySpawnableLocations.Count == 0) return;
            int randomIndex = UnityEngine.Random.Range(0,crazySpawnableLocations.Count);
            enemy.transform.position = crazySpawnableLocations[randomIndex];
            if (!enemy.activeInHierarchy)
            {
                enemy.SetActive(true);
            }
            crazySpawnableLocations.RemoveAt(randomIndex);
        }
    }

    private void SetCrazyPositions()
    {
        Vector2Int playerObjectGridData = gridManager.FindGridDataIndex(player.transform.position);
        int xMin = Math.Max(playerObjectGridData.x - 5, 1);
        int xMax = Math.Min(playerObjectGridData.x + 5, gridManager.width - 1);
        int yMin = Math.Max(playerObjectGridData.y - 5, 1);
        int yMax = Math.Min(playerObjectGridData.y + 5, gridManager.height - 1);
        for (int i = xMin; i <= xMax; i++)
        {
            if (i == playerObjectGridData.x) continue;
            for (int j = yMin; j <= yMax; j++)
            {
                if (j == playerObjectGridData.y) continue;
                if (gridManager.CheckGridData(i,j,TileType.Block)) continue;
                Vector3 cellPosition = gridManager.FindCellPositionByGridData(i, j);
                Vector3 cellWorldPosition = gridManager.FindCellWorldPosition(cellPosition);
                crazySpawnableLocations.Add(cellWorldPosition);
            }
        }
    }
}
