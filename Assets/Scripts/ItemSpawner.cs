using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject fireUp;
    [SerializeField] private GameObject gate;
    private GridManager gridManager;
    void Start()
    {
        gridManager = GridManager.Instance;
        CreateFireUp();
        CreateGate();
    }

    private void CreateGate()
    {
        int randomIndex = Random.Range(0, gridManager.brickLocations.Count);
        Vector3 position = gridManager.brickLocations[randomIndex];
        Instantiate(gate, position, Quaternion.identity);
        gridManager.brickLocations.RemoveAt(randomIndex);
    }
    private void CreateFireUp()
    {
        int randomIndex = Random.Range(0, gridManager.brickLocations.Count);
        Vector3 position = gridManager.brickLocations[randomIndex];
        Instantiate(fireUp, position, Quaternion.identity);
        gridManager.brickLocations.RemoveAt(randomIndex);
    }
}
