using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance {get; private set;}

    public MyPair<GameObject, int>[] gameObjects;
    private Dictionary<EntityId, Queue<GameObject>> dict = new Dictionary<EntityId, Queue<GameObject>>();
    private Dictionary<EntityId, EntityId> cloneToPrefabMap = new Dictionary<EntityId, EntityId>();
    [System.NonSerialized] public Dictionary<Vector3,GameObject> findGameObjectByPosition = new Dictionary<Vector3, GameObject>();

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (MyPair<GameObject,int> pair in gameObjects)
        {
            EntityId entityId = pair.Key.GetEntityId();
            dict.Add(entityId, new Queue<GameObject>());
            for (int i = 0; i < pair.Value; i++)
            {
                GameObject clone = Instantiate(pair.Key, new Vector3(-50f, -50f, 0f), Quaternion.identity);
                clone.SetActive(false);
                dict[entityId].Enqueue(clone);
                cloneToPrefabMap.Add(clone.GetEntityId(),entityId);
            }
        }
    }

    public GameObject InstantiateObject(GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject thisObject;
        EntityId key = original.GetEntityId();
        if (dict.ContainsKey(key) && dict[key].Count != 0)
        {
            thisObject = dict[key].Dequeue();
        }
        else
        {
            thisObject = Instantiate(original, position, rotation);
            dict.Add(key, new Queue<GameObject>());
            cloneToPrefabMap.Add(thisObject.GetEntityId(), key);
        }
        thisObject.transform.position = position;
        findGameObjectByPosition[position] = thisObject;
        thisObject.transform.rotation = rotation;
        thisObject.SetActive(true);
        return thisObject;
    }

    public void DestroyObject(GameObject original)
    {
        EntityId destroyObjectId = original.GetEntityId();
        Vector3 position = original.transform.position;
        if (findGameObjectByPosition.ContainsKey(position))
        {
            findGameObjectByPosition.Remove(position);
        }
        original.SetActive(false);
        original.transform.position = new Vector3(-50f,-50f,0f);
        original.transform.rotation = Quaternion.identity;
        if (cloneToPrefabMap.TryGetValue(destroyObjectId, out EntityId key))
        {
            dict[key].Enqueue(original);
        }
        else
        {
            Destroy(original);
        }
    }
}
