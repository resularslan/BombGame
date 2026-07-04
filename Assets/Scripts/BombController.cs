using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float seconds = 3f;
    private LevelGenerator levelGenerator;
    private PlayerController playerController;
    [SerializeField] private GameObject fire;
    private SpriteRenderer spriteRenderer;
    private Vector2 size;
    private int x,y;

    void Start()
    {
        levelGenerator = GameObject.FindWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        size = spriteRenderer.bounds.size;
        x = Mathf.FloorToInt(transform.position.x - levelGenerator.bounds.xMin);
        y = Mathf.FloorToInt(transform.position.y - levelGenerator.bounds.yMin);
        StartCoroutine(Explode(seconds));
    }
    private IEnumerator Explode(float waitSeconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitSeconds);
            levelGenerator.gridData[x,y] = 0;
            playerController.bombCount = playerController.maxBomb;
            explodeAnimation();
            Destroy(gameObject);
        }
    }

    private void explodeAnimation()
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
       
            if (levelGenerator.gridData[thisX,thisY] == 0)
            {
                Vector3 position = new Vector3(transform.position.x + sizeX, transform.position.y + sizeY, transform.position.z);
                GameObject fireObject = Instantiate(fire, position, quaternion.identity);
                Destroy(fireObject, 1);
            }
    }
}
