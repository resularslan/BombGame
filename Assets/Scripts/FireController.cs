using System.Collections;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private float seconds = 1f;
    void OnEnable()
    {
        StartCoroutine(FireAnimation(seconds));
    }
    void OnDestroy()
    {
        StopAllCoroutines();
    }
    private IEnumerator FireAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectSpawner.Instance.DestroyObject(gameObject);
    }
}
