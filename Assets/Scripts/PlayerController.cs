using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private Vector3 bombPosition;
    public int maxBomb = 1;
    public int bombLevel = 1;
    [System.NonSerialized] public int bombCount;
    void Start()
    {
        bombCount = maxBomb;
    }

    void Update()
    {
        bombPosition = GridManager.Instance.findCellWorldPosition(transform.position);
        // Debug.Log($"CP: {cellPosition}, BP: {bombPosition}");
        int x = GridManager.Instance.findGridDataIndex(transform.position).x;
        int y = GridManager.Instance.findGridDataIndex(transform.position).y;
        // print(levelGenerator.gridData[x,y]);
        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            if (GridManager.Instance.gridData[x, y] == 0)
            {
                ObjectSpawner.Instance.InstantiateObject(bomb, bombPosition, Quaternion.identity);
                GridManager.Instance.gridData[x, y] = 3;
                bombCount--;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Bomb");
        }
    }
}
