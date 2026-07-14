using System.Collections;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public float seconds = 1f;
    void OnEnable()
    {
        StartCoroutine(FireAnimation(seconds));
    }
    private IEnumerator FireAnimation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ObjectSpawner.Instance.DestroyObject(gameObject);
    }
}
