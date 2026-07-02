using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2.5f;
    private LayerMask layerMask;
    private SpriteRenderer spriteRenderer;
    private Vector2 half;
    Vector2 direction;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        layerMask = ~(1 << LayerMask.NameToLayer("Player"));;
        half = spriteRenderer.bounds.extents;
    }
    void Update()   
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (direction.x != 0)
        {
            direction.y = 0;
        }
        if (direction != Vector2.zero)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Abs(Vector2.Dot(half,direction)), layerMask);
            if (!hit)
            {
                transform.Translate(direction * speed * Time.deltaTime);
                preventIntersection();
            }
            else
            {
                transform.position = new Vector3(hit.point.x - (direction.x * half.x), hit.point.y - (direction.y * half.y), 0);
            }
            if (hit.collider != null)
            {
                Debug.Log($"Hit object name: {hit.collider.name} on Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            }
            Debug.DrawRay(transform.position,direction * half,Color.red);
        }
    }
    private void preventIntersection()
    {
        float step =  speed * Time.deltaTime;
        Vector3 target = transform.position;
        if (Mathf.Abs(direction.x) > 0)
        {
            target = new Vector3(transform.position.x, MathF.Round(transform.position.y), 0);
        }
        else if (Mathf.Abs(direction.y) > 0)
        {
            target = new Vector3(MathF.Round(transform.position.x), transform.position.y, 0);
        }
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }
}
