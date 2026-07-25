using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed { get; private set; } = 2f;
    [SerializeField] private LayerMask excludedLayers; 
    private LayerMask layerMask;
    private SpriteRenderer spriteRenderer;
    private Vector2 half;
    private Vector2 direction;
    private GridManager gridManager;
    private PlayerController playerController;
    void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        playerController = gameObject.GetComponent<PlayerController>();
    }
    void Start()
    {
        layerMask = ~excludedLayers;
        half = spriteRenderer.bounds.extents;
        gridManager = GridManager.Instance;
    }
    void Update()   
    {
        if (!playerController.isDied)
        {
            Movement();
        }
    }

    private void Movement()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction.x != 0)
        {
            direction.y = 0;
        }
        if (direction != Vector2.zero)
        {
            Vector3 currentPosition = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Abs(Vector2.Dot(half,direction)), layerMask);
            if (!hit)
            {
                Vector3 movement = speed * Time.deltaTime * (Vector3)direction;
                Vector3 movedPosition = currentPosition + movement;
                Vector3 lastPosition = PreventIntersection(movedPosition);
                transform.position = lastPosition;
            }
            else
            {
                Vector3 hitPosition = new Vector3(hit.point.x - (direction.x * half.x), hit.point.y - (direction.y * half.y), 0);
                transform.position = hitPosition;   
            }
            // if (hit.collider != null)
            // {
            //     Debug.Log($"Hit object name: {hit.collider.name} on Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            // }
            // Debug.DrawRay(transform.position,direction * half,Color.red);
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
}
