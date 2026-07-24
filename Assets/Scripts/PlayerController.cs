using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private Vector3 bombPosition;
    [System.NonSerialized] public int maxBomb = 10; 
    public int bombLevel = 1;
    public int bombCount = 1;
    private GridManager gridManager;
    private ObjectSpawner objectSpawner;
    public bool isDied {get; private set;} = false;
    public static event Action OnPlayerDied; 
    void Start()
    {
        gridManager = GridManager.Instance;
        objectSpawner = ObjectSpawner.Instance;
    }
    void Update()
    {
        if (!isDied)
        {
            PlantBomb();
        }
    }

    private void PlantBomb()
    {
        if (bombCount > maxBomb)
        {
            bombCount = maxBomb;
        }
        bombPosition = gridManager.FindCellWorldPosition(transform.position);
        // Debug.Log($"CP: {cellPosition}, BP: {bombPosition}");
        int x = gridManager.FindGridDataIndex(transform.position).x;
        int y = gridManager.FindGridDataIndex(transform.position).y;
        // print(levelGenerator.gridData[x,y]);
        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            if (gridManager.CheckGridData(x,y,0))
            {
                objectSpawner.InstantiateObject(bomb, bombPosition, Quaternion.identity);
                gridManager.SetGridData(x,y, TileType.Bomb);
                bombCount--;
            }
        }
    }
    private void Die()
    {
        isDied = true;
        OnPlayerDied?.Invoke();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Fire"))
        {
            if (collision.gameObject != null)
            {
                Die();
            }
        }
    }
}
