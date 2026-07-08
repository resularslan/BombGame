using System.Collections;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public float seconds = 1f;
    void OnEnable()
    {
        StartCoroutine(fireAnimation(seconds));
    }
    private IEnumerator fireAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectSpawner.Instance.DestroyObject(gameObject);
    }
}
