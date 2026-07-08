using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private Grid grid;
    [System.NonSerialized] public Tilemap tilemap_bricks;
    private Tilemap tilemap_blocks;
    public Tile tile_brick;
    [System.NonSerialized] public int[,] gridData;
    [System.NonSerialized] public BoundsInt bounds;
    public GameObject player;
    public GameObject enemy;
    public int enemyCount = 6;
    private List<Vector3> spawnableLocations;
    void Start()
    {
        tilemap_bricks.ClearAllTiles();
        tilemap_blocks.CompressBounds();
        bounds = tilemap_blocks.cellBounds;
        int width = bounds.size.x;
        int height = bounds.size.y;
        gridData = new int[width,height];
        spawnableLocations = new List<Vector3>();
        // print(bounds.xMin);
        // print(bounds.yMin);
        // tilemap_bricks.SetTile(new Vector3Int(0,0,0), tile_brick);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = findCellPositionByGridData(x,y);
                if ((x == 1 && y == height - 2) || (x == 2 && y == height - 2) || (x == 1 && y == height - 3))
                {
                    if (x == 1 && y == height - 2)
                    {
                        Vector3 playerPosition = findCellWorldPosition(tilePos);
                        Instantiate(player, playerPosition, Quaternion.identity);
                    }
                    gridData[x,y] = 0;
                    continue;
                }
                if (tilemap_blocks.HasTile(tilePos))
                {
                    gridData[x,y] = 1;
                }
                else
                {
                    gridData[x,y] = 0;
                    if (Random.Range(0,3) == 1)
                    {
                        gridData[x,y] = 2;
                        tilemap_bricks.SetTile(tilePos, tile_brick);
                    }
                    else
                    {
                        spawnableLocations.Add(findCellWorldPosition(tilePos));
                    }
                }
            }
        }
        for (int i = 0; i < enemyCount; i++)
        {
            int randomIndex = Random.Range(0,spawnableLocations.Count);
            Instantiate(enemy, spawnableLocations[randomIndex], Quaternion.identity);
            spawnableLocations.RemoveAt(randomIndex);
        }
    }
    private void Awake()
    {
        grid = GetComponent<Grid>();
        tilemap_blocks = GetComponentsInChildren<Tilemap>()[0];
        tilemap_bricks = GetComponentsInChildren<Tilemap>()[1];
        Instance = this;
    }
    public Vector3 findCellWorldPosition(Vector3 position)
    {
        Vector3Int cellPosition = grid.WorldToCell(position);
        return grid.GetCellCenterWorld(cellPosition);
    }

    public Vector3Int findCellPosition(Vector3 position)
    {
        return grid.WorldToCell(position);
    }
    public Vector2Int findGridDataIndex(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x - bounds.xMin);
        int y = Mathf.FloorToInt(position.y - bounds.yMin);
        return new Vector2Int(x,y);
    }
    public Vector3Int findCellPositionByGridData(int x, int y)
    {
        return new Vector3Int(x + bounds.xMin, y + bounds.yMin,0);
    }
    public bool checkGridData(int x, int y, int value)
    {
        if (!checkBounds(x,y))
        {
            return false;
        }
        return gridData[x,y] == value;
    }
    public void setGridData(int x, int y, int value)
    {
        if (!checkBounds(x,y))
        {
            return;
        }
        gridData[x,y] = value;
    }
    public bool checkBounds(int x, int y)
    {
        if (x < 0 || x >= bounds.size.x || y < 0 || y >= bounds.size.y)
        {
            return false;
        }
        return true;
    }
}
