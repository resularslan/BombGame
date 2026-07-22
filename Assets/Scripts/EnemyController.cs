using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    public float speed = 2f;
    private SpriteRenderer spriteRenderer;
    private Vector2 half;
    private LayerMask layerMask;
    private int x,y;
    private GridManager gridManager;
    private Vector2Int lastDecisionCell = new Vector2Int(-999, -999);
    private bool isDied = false;
    private float directionChangePossibility = 0.25f;

    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void OnEnable()
    {
        GameManager.OnTimeUp += OpenCrazyMode;
    }
    void OnDisable()
    {
        GameManager.OnTimeUp -= OpenCrazyMode;
    }
    void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Fire"));
        half = spriteRenderer.bounds.extents;
        gridManager = GridManager.Instance;
        DetermineDirection();
    }
    void Update()
    {
        if (!isDied)
        {
            Movement();
        }
    }

    private void Movement()
    {
        x = gridManager.FindGridDataIndex(transform.position).x;
        y = gridManager.FindGridDataIndex(transform.position).y;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Abs(Vector2.Dot(half,direction)), layerMask);
        Vector3 currentPosition = transform.position;
        if (hit)
        {
            direction = -direction;
        }
        ChangeDirection();
        Vector3 movement = speed * (Vector3)direction * Time.deltaTime;
        Vector3 movedPosition = currentPosition + movement;
        Vector3 lastPosition = PreventIntersection(movedPosition);
        transform.position = lastPosition;
    }

    private void OpenCrazyMode()
    {
        layerMask = 1 << LayerMask.NameToLayer("Block");
        DetermineDirection();
        speed *= 3;
        directionChangePossibility = 0.5f;
    }
    private void DetermineDirection()
    {
        x = gridManager.FindGridDataIndex(transform.position).x;
        y = gridManager.FindGridDataIndex(transform.position).y;
        if (gridManager.CheckGridData(x + 1, y,0) || gridManager.CheckGridData(x - 1, y,0))
        {
            direction = Vector2.right;
        }
        else if (gridManager.CheckGridData(x, y + 1,0) || gridManager.CheckGridData(x, y - 1,0))
        {
            direction = Vector2.up;
        }
    }
    private Vector3 PreventIntersection(Vector3 movedPosition)
    {
        float step =  speed * Time.deltaTime;
        Vector3 cellWorldPosition = gridManager.FindCellWorldPosition(transform.position);
        if (Mathf.Abs(direction.x) > 0)
        {
            Vector3 target = new Vector3(movedPosition.x, cellWorldPosition.y, 0);
            movedPosition = Vector3.MoveTowards(movedPosition, target, step);
        }
        else if (Mathf.Abs(direction.y) > 0)
        {
            Vector3 target = new Vector3(cellWorldPosition.x, movedPosition.y, 0);
            movedPosition = Vector3.MoveTowards(movedPosition, target, step);
        }
        return movedPosition;
    }
    private void ChangeDirection()
    {
        Vector2Int cellData = new Vector2Int(x + (int)direction.y, y + (int)direction.x);
        float enemyPositionDotRounded = (float)Math.Round(Vector3.Dot(transform.position,direction),1);
        Vector3Int emptyCellPosition = gridManager.FindCellPositionByGridData(cellData.x, cellData.y);
        Vector3 emptyWorldPosition = gridManager.FindCellWorldPosition(emptyCellPosition);
        float emptyPositionDot = Vector3.Dot(emptyWorldPosition,direction);
        Vector2Int currentCell = new Vector2Int(x, y);
        if (gridManager.CheckGridData(cellData.x, cellData.y, 0) && Mathf.Approximately(enemyPositionDotRounded,emptyPositionDot))
        {
            if (currentCell != lastDecisionCell)
            {
                float randomValue = UnityEngine.Random.value;
                if (randomValue < directionChangePossibility)
                {
                    direction = new Vector2(direction.y, direction.x);
                }
                lastDecisionCell = currentCell;
            }
        }
    }
    private IEnumerator Die()
    {
        isDied = true;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        UIManager.Instance.CreateScore(100, transform.position);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Fire"))
            {
                StartCoroutine(Die());
            }
        }
    }
}
