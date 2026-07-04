using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }
    private Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
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
}
