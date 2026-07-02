using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    public Tilemap tilemap_bricks;
    public Tilemap tilemap_blocks;
    public Tile tile_brick;
    public GameObject player;
    [System.NonSerialized] public int[,] gridData;
    void Start()
    {
        tilemap_bricks.ClearAllTiles();
        tilemap_blocks.CompressBounds();
        BoundsInt bounds = tilemap_blocks.cellBounds;
        int width = bounds.size.x;
        int height = bounds.size.y;
        gridData = new int[width,height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = new Vector3Int(x + bounds.xMin, y + bounds.yMin,0);
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
}
