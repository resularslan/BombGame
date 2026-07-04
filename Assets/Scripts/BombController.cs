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

    void OnEnable()
    {
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        size = spriteRenderer.bounds.size;
        x = GridManager.Instance.findGridDataIndex(transform.position).x;
        y = GridManager.Instance.findGridDataIndex(transform.position).y;
        StartCoroutine(Explode(seconds));
    }
    private IEnumerator Explode(float waitSeconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitSeconds);
            GridManager.Instance.gridData[x,y] = 0;
            playerController.bombCount = playerController.maxBomb;
            createExplode();
            gameObject.layer = LayerMask.NameToLayer("Default");
            ObjectSpawner.Instance.DestroyObject(gameObject);
        }
    }

    private void createExplode()
    {
        createFire(x,y,0,0,playerController.bombLevel,Vector2Int.right);
        createFire(x+1,y,size.x,0,1,Vector2Int.right);
        createFire(x-1,y,-size.x,0,1,Vector2Int.left);
        createFire(x,y+1,0,size.y,1,Vector2Int.up);
        createFire(x,y-1,0,-size.y,1,Vector2Int.down);
    }
    private void createFire(int thisX, int thisY, float sizeX, float sizeY, int fireCount, Vector2Int direction)
    {
        int cellData = GridManager.Instance.gridData[thisX,thisY];
        Vector3 position = new Vector3(transform.position.x + sizeX, transform.position.y + sizeY, transform.position.z);
        if (Math.Abs(fireCount) > Math.Abs(playerController.bombLevel))
        {
            return;
        }
        if (thisX < 0 || thisX >= GridManager.Instance.bounds.size.x || 
        thisY < 0 || thisY >= GridManager.Instance.bounds.size.y)
        {
            return;
        }
        if (cellData == 0)
        {
            ObjectSpawner.Instance.InstantiateObject(fire, position, quaternion.identity);
            createFire(thisX + direction.x, thisY + direction.y, sizeX + (sizeX / fireCount), sizeY + (sizeY / fireCount), fireCount + 1, direction);
        }
        else if (cellData == 2)
        {
            Vector3Int tilePos = GridManager.Instance.findCellPosition(new Vector3(transform.position.x + sizeX, transform.position.y + sizeY, transform.position.z));
            GridManager.Instance.tilemap_bricks.SetTile(tilePos, null);
            GridManager.Instance.gridData[thisX,thisY] = 0;
            ObjectSpawner.Instance.InstantiateObject(fire, position, quaternion.identity);
        }
        else if (cellData == 3)
        {
            // Explode Bomb
        }
    }
}
