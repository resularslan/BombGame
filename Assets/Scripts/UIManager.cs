using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject stageTextObject;
    public GameObject bacground;
    void Start()
    {
        Time.timeScale = 0f;
        TextMeshProUGUI stageText = stageTextObject.GetComponent<TextMeshProUGUI>();
        stageText.SetText(SceneManager.GetActiveScene().name);
        StartCoroutine(DeactivateInformationUI());
    }
    private IEnumerator DeactivateInformationUI()
    {
        yield return new WaitForSecondsRealtime(2f);
        stageTextObject.SetActive(false);
        bacground.SetActive(false);
        Time.timeScale = 1f;
    }

}
