using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private Vector3 bombPosition;
    private LevelGenerator levelGenerator;
    [System.NonSerialized] public int maxBomb = 1;
    public int bombCount;
    void Start()
    {
        bombCount = maxBomb;
        levelGenerator = GameObject.FindWithTag("LevelGenerator").GetComponent<LevelGenerator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            bombPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
            int x = Mathf.FloorToInt(bombPosition.x - levelGenerator.bounds.xMin);
            int y = Mathf.FloorToInt(bombPosition.y - levelGenerator.bounds.yMin);
            if (levelGenerator.gridData[x, y] != 3)
            {
                Instantiate(bomb, bombPosition, Quaternion.identity);
                levelGenerator.gridData[x, y] = 3;
                bombCount--;
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Bomb");
        }
    }
}
