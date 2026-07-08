using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Vector2 direction = Vector2.right;
    public float speed = 2f;
    private SpriteRenderer spriteRenderer;
    private Vector2 half;
    private LayerMask layerMask;
    private Vector2[] directions = {Vector2.right, Vector2.left, Vector2.up, Vector2.down};
    
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        layerMask = ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Fire"));
        half = spriteRenderer.bounds.extents;
    }
    void Update()
    {
        int x = GridManager.Instance.findGridDataIndex(transform.position).x;
        int y = GridManager.Instance.findGridDataIndex(transform.position).y;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Abs(Vector2.Dot(half,direction)), layerMask);
        if (hit)
        {
            direction = -direction;
        }
        if (GridManager.Instance.checkGridData(x + (int)direction.y, y + (int)direction.x, 0))
        {
            int randomNum = Random.Range(0,1000);
            if (randomNum == 0)
            {
                int randomIndex = Random.Range(0,4);
                direction = directions[randomIndex];
            }
        }
        transform.Translate(speed * direction * Time.deltaTime);
        preventIntersection();
    }

    private void preventIntersection()
    {
        float step =  speed * Time.deltaTime;
        Vector3 cellWorldPosition = GridManager.Instance.findCellWorldPosition(transform.position);
        if (Mathf.Abs(direction.x) > 0)
        {
            Vector3 target = new Vector3(transform.position.x, cellWorldPosition.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
        else if (Mathf.Abs(direction.y) > 0)
        {
            Vector3 target = new Vector3(cellWorldPosition.x, transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }
}
