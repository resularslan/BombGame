using System.Collections;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private float seconds = 1f;
    void OnEnable()
    {
        StartCoroutine(Destroy(seconds));
    }
    private IEnumerator Destroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectSpawner.Instance.DestroyObject(gameObject);
    }
}
