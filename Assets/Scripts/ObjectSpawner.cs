using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct MyPair<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}
public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance {get; private set;}

    public MyPair<GameObject, int>[] gameObjects;
    private Dictionary<String,Queue<GameObject>> dict = new Dictionary<String, Queue<GameObject>>();
    [System.NonSerialized] public Dictionary<Vector3,GameObject> findGameObjectByPosition = new Dictionary<Vector3, GameObject>();

    void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        foreach (MyPair<GameObject,int> pair in gameObjects)
        {
            dict.Add(pair.Key.name + "(Clone)", new Queue<GameObject>());
            for (int i = 0; i < pair.Value; i++)
            {
                GameObject clone = Instantiate(pair.Key, new Vector3(-50f, -50f, 0f), Quaternion.identity);
                dict[clone.name].Enqueue(clone);
            }
        }
    }

    public void InstantiateObject(GameObject original, Vector3 position, Quaternion rotation)
    {
        GameObject thisObject = dict[original.name + "(Clone)"].Dequeue();
        thisObject.transform.position = position;
        findGameObjectByPosition[position] = thisObject;
        thisObject.transform.rotation = rotation;
        thisObject.SetActive(true);
    }

    public void DestroyObject(GameObject original)
    {
        if (findGameObjectByPosition.ContainsKey(original.transform.position))
        {
            findGameObjectByPosition.Remove(original.transform.position);
        }
        original.SetActive(false);
        original.transform.position = new Vector3(-50f,-50f,0f);
        original.transform.rotation = quaternion.identity;
        dict[original.name].Enqueue(original);
    }
}
