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
        createFire(x,y,0,0);
        for (int i = 1; i <= playerController.bombLevel; i++)
        {
            createFire(x+i,y,i*size.x,0);
            createFire(x-i,y,-i*size.x,0);
            createFire(x,y+i,0,i*size.y);
            createFire(x,y-i,0,-i*size.y);
        }
    }

    private void createFire(int thisX, int thisY, float sizeX, float sizeY)
    {
        if (thisX < 0 || thisX >= GridManager.Instance.bounds.size.x || thisY < 0 || thisY >= GridManager.Instance.bounds.size.y) return;
        if (GridManager.Instance.gridData[thisX,thisY] == 0)
        {
            Vector3 position = new Vector3(transform.position.x + sizeX, transform.position.y + sizeY, transform.position.z);
            ObjectSpawner.Instance.InstantiateObject(fire, position, quaternion.identity);
        }
        else if (GridManager.Instance.gridData[thisX,thisY] == 2)
        {
            Vector3Int position = GridManager.Instance.findCellPosition(new Vector3(transform.position.x + sizeX, transform.position.y + sizeY, transform.position.z));
            GridManager.Instance.tilemap_bricks.SetTile(position, null);
            GridManager.Instance.gridData[thisX,thisY] = 0;
        }
    }
}
