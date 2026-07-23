using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTextManager : MonoBehaviour
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
