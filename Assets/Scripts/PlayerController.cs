using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    private Vector3 bombPosition;
    [System.NonSerialized] public int maxBomb = 10; 
    public int bombLevel = 1;
    public int bombCount;
    private GridManager gridManager;
    private ObjectSpawner objectSpawner;
    private PlayerMovement playerMovement;
    private bool isDied = false;
    void Start()
    {
        gridManager = GridManager.Instance;
        objectSpawner = ObjectSpawner.Instance;
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }
    void Update()
    {
        if (!isDied)
        {
            PlantBomb();
        }
    }

    private void PlantBomb()
    {
        if (bombCount > maxBomb)
        {
            bombCount = maxBomb;
        }
        bombPosition = gridManager.FindCellWorldPosition(transform.position);
        // Debug.Log($"CP: {cellPosition}, BP: {bombPosition}");
        int x = gridManager.FindGridDataIndex(transform.position).x;
        int y = gridManager.FindGridDataIndex(transform.position).y;
        // print(levelGenerator.gridData[x,y]);
        if (Input.GetKeyDown(KeyCode.X) && bombCount > 0)
        {
            if (gridManager.CheckGridData(x,y,0))
            {
                objectSpawner.InstantiateObject(bomb, bombPosition, Quaternion.identity);
                gridManager.SetGridData(x,y,3);
                bombCount--;
            }
        }
    }
    private IEnumerator Die()
    {
        isDied = true;
        playerMovement.enabled = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Stage-1");
        this.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Fire"))
        {
            StartCoroutine(Die());
        }
    }
}
