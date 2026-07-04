using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private Vector3 bombPosition;
    private Vector3 cellPosition;
    private LevelGenerator levelGenerator;
    [System.NonSerialized] public int maxBomb = 1;
    [System.NonSerialized] public int bombLevel = 1;
    public int bombCount;
    void Start()
    {
        bombCount = maxBomb;
        levelGenerator = GameObject.FindWithTag("LevelGenerator").GetComponent<LevelGenerator>();
    }

    void Update()
    {
        bombPosition = GridManager.Instance.findCellWorldPosition(transform.position);
        cellPosition = GridManager.Instance.findCellPosition(transform.position);
        // Debug.Log($"CP: {cellPosition}, BP: {bombPosition}");
        int x = Mathf.RoundToInt(cellPosition.x - levelGenerator.bounds.xMin);
        int y = Mathf.RoundToInt(cellPosition.y - levelGenerator.bounds.yMin);
        // print(levelGenerator.gridData[x,y]);
        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            if (levelGenerator.gridData[x, y] == 0)
            {
                Instantiate(bomb, bombPosition, Quaternion.identity);
                levelGenerator.gridData[x, y] = 3;
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
