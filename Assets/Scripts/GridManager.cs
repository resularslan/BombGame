using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private Grid grid;
    [System.NonSerialized] public Tilemap tilemap_bricks;
    [System.NonSerialized] public Tilemap tilemap_blocks;
    public Tile tile_brick;
    [System.NonSerialized] public int[,] gridData;
    [System.NonSerialized] public BoundsInt bounds;
    void Start()
    {
        tilemap_bricks.ClearAllTiles();
        tilemap_blocks.CompressBounds();
        bounds = tilemap_blocks.cellBounds;
        int width = bounds.size.x;
        int height = bounds.size.y;
        gridData = new int[width,height];
        // print(bounds.xMin);
        // print(bounds.yMin);
        // tilemap_bricks.SetTile(new Vector3Int(0,0,0), tile_brick);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = GridManager.Instance.findCellPosition(new Vector3(x + bounds.xMin, y + bounds.yMin,0));
                if ((x == 1 && y == height - 2) || (x == 2 && y == height - 2) || (x == 1 && y == height - 3))
                {
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
                }
            }
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
}
