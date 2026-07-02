using System.Collections;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private float seconds = 3f;
    private LevelGenerator levelGenerator;
    private PlayerController playerController;
    void Start()
    {
        levelGenerator = GameObject.FindWithTag("LevelGenerator").GetComponent<LevelGenerator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        StartCoroutine(Explode(seconds));
    }
    private IEnumerator Explode(float waitSeconds)
    {
        while (true)
        {
            int x = Mathf.FloorToInt(transform.position.x - levelGenerator.bounds.xMin);
            int y = Mathf.FloorToInt(transform.position.y - levelGenerator.bounds.yMin);
            yield return new WaitForSeconds(waitSeconds);
            Destroy(gameObject);
            levelGenerator.gridData[x,y] = 0;
            playerController.bombCount = playerController.maxBomb;
            // explodeanimation()
        }
    }
}
