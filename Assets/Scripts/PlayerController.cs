using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private Vector3 bombPosition;
    [System.NonSerialized] public int maxBomb = 10; 
    public int bombLevel = 1;
    public int bombCount;
    void Update()
    {
        if (bombCount > maxBomb)
        {
            bombCount = maxBomb;
        }
        bombPosition = GridManager.Instance.findCellWorldPosition(transform.position);
        // Debug.Log($"CP: {cellPosition}, BP: {bombPosition}");
        int x = GridManager.Instance.findGridDataIndex(transform.position).x;
        int y = GridManager.Instance.findGridDataIndex(transform.position).y;
        // print(levelGenerator.gridData[x,y]);
        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            if (GridManager.Instance.checkGridData(x,y,0))
            {
                ObjectSpawner.Instance.InstantiateObject(bomb, bombPosition, Quaternion.identity);
                GridManager.Instance.setGridData(x,y,3);
                bombCount--;
            }
        }
    }
}
