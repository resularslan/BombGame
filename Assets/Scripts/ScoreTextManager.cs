using System.Collections;
using UnityEngine;

public class ScoreTextManager : MonoBehaviour
{
    [SerializeField] private float seconds = 1f;
    void OnEnable()
    {
        StartCoroutine(Destroy(seconds));
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
    private IEnumerator Destroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectSpawner.Instance.DestroyObject(gameObject);
    }
}
