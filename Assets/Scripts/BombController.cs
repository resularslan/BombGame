using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float seconds = 3f;
    private PlayerController playerController;
    [SerializeField] private GameObject fire;
    private SpriteRenderer spriteRenderer;
    private Vector2 size;
    private int x,y;
    private IEnumerator coroutine;

    void OnEnable()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        size = spriteRenderer.bounds.size;
        x = GridManager.Instance.findGridDataIndex(transform.position).x;
        y = GridManager.Instance.findGridDataIndex(transform.position).y;
        coroutine = Explode(seconds);
        StartCoroutine(coroutine);
    }
    void OnDisable()
    {
        GridManager.Instance.gridData[x,y] = 0;
        playerController.bombCount = playerController.maxBomb;
        gameObject.layer = LayerMask.NameToLayer("Default");
        createExplode();
    }
    private IEnumerator Explode(float waitSeconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitSeconds);
            ObjectSpawner.Instance.DestroyObject(gameObject);
        }
    }

    private void explodeImmediately()
    {
        StopCoroutine(coroutine);
        StartCoroutine(Explode(0f));
    }

    private void createExplode()
    {
        ObjectSpawner.Instance.InstantiateObject(fire, transform.position, quaternion.identity);
        Vector2Int[] directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
        foreach (Vector2Int direction in directions)
        {
            for (int i = 1; i <= playerController.bombLevel; i++)
            {
                int targetX = x + i * direction.x;
                int targetY = y + i * direction.y;
                int cellData = GridManager.Instance.gridData[targetX,targetY];
                float sizeX = size.x * i * direction.x;
                float sizeY = size.y * i * direction.y;
                Vector3 position = new Vector3(transform.position.x + sizeX, transform.position.y + sizeY, transform.position.z);
                if (targetX < 0 || targetX >= GridManager.Instance.bounds.size.x || 
                targetY < 0 || targetY >= GridManager.Instance.bounds.size.y)
                {
                    break;
                }
                if (cellData == 0)
                {
                    ObjectSpawner.Instance.InstantiateObject(fire, position, quaternion.identity);
                }
                else
                {
                    if (cellData == 2)
                    {
                        Vector3Int tilePos = GridManager.Instance.findCellPosition(position);
                        GridManager.Instance.tilemap_bricks.SetTile(tilePos, null);
                        GridManager.Instance.gridData[targetX,targetY] = 0;
                        ObjectSpawner.Instance.InstantiateObject(fire, position, quaternion.identity);
                    }
                    else if (cellData == 3)
                    {
                        GameObject targetBomb = ObjectSpawner.Instance.findGameObjectByPosition[position];
                        BombController targetBombController = targetBomb.GetComponent<BombController>();
                        targetBombController.explodeImmediately();
                    }
                    break;
                }

            }
        }
    }
}
