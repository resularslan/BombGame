using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float seconds = 3f;
    [SerializeField] private GameObject fire;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;
    private Vector2 size;
    private int x,y;
    private IEnumerator coroutine;
    private GridManager gridManager;
    private ObjectSpawner objectSpawner;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        gridManager = GridManager.Instance;
    }
    void Start()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        size = spriteRenderer.bounds.size;
        objectSpawner = ObjectSpawner.Instance;
    }
    void OnEnable()
    {   
        x = gridManager.FindGridDataIndex(transform.position).x;
        y = gridManager.FindGridDataIndex(transform.position).y;
        coroutine = Explode(seconds);
        StartCoroutine(coroutine);
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
    void OnDisable()
    {
        gridManager.SetGridData(x,y,0);
        if (playerController != null)
        {
            playerController.bombCount++;
        }
        gameObject.layer = LayerMask.NameToLayer("_Bomb");
        if (gridManager.CheckBounds(x,y))
        {
            CreateExplode();
        }
    }
    private IEnumerator Explode(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        if (this == null) yield break;
        objectSpawner.DestroyObject(gameObject);
    }

    private void ExplodeImmediately()
    {
        StopCoroutine(coroutine);
        StartCoroutine(Explode(0f));
    }

    private void CreateExplode()
    {
        objectSpawner.InstantiateObject(fire, transform.position, Quaternion.identity);
        Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
        foreach (Vector2Int direction in directions)
        {
            for (int i = 1; i <= playerController.bombLevel; i++)
            {
                int targetX = x + i * direction.x;
                int targetY = y + i * direction.y;
                if (!gridManager.CheckBounds(targetX,targetY))
                {
                    break;
                }
                TileType cellData = gridManager.gridData[targetX,targetY];
                float sizeX = size.x * i * direction.x;
                float sizeY = size.y * i * direction.y;
                Vector3 position = new Vector3(transform.position.x + sizeX, transform.position.y + sizeY, transform.position.z);
                if (cellData == TileType.Empty)
                {
                    objectSpawner.InstantiateObject(fire, position, quaternion.identity);
                }
                else
                {
                    if (cellData == TileType.Brick)
                    {
                        Vector3Int tilePos = gridManager.FindCellPosition(position);
                        gridManager.CallBreakBrick(tilePos);
                        gridManager.SetGridData(targetX,targetY,0);
                        objectSpawner.InstantiateObject(fire, position, quaternion.identity);
                    }
                    else if (cellData == TileType.Bomb)
                    {
                        GameObject targetBomb = objectSpawner.findGameObjectByPosition[position];
                        if (targetBomb != null)
                        {
                            BombController targetBombController = targetBomb.GetComponent<BombController>();
                            targetBombController.ExplodeImmediately();
                        }
                    }
                    break;
                }

            }
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject != null)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                gameObject.layer = LayerMask.NameToLayer("Bomb");
            }
        }
    }
}
