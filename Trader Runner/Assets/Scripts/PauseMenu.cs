using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] TextMeshProUGUI resumeText;
    [SerializeField] GameObject buttonHolder;
    [SerializeField] private GameObject resumeButton;

    void Start(){
        ui.SetActive(false);
        resumeText.gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }
    
    public void Pause(){
        if (buttonHolder.activeInHierarchy)
        {
            Resume(3);
            return;
        }

        if (Time.timeScale == 0)
        {
            return;
        }
        ui.SetActive(true);
        buttonHolder.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume(int totalTime = 3)
    {
        buttonHolder.SetActive(false);
        resumeText.gameObject.SetActive(true);
        StartCoroutine(Resuming(totalTime));
    }

    IEnumerator Resuming(int totalTime)
    {
        //Animation for the text
        for (int i = 0; i < totalTime; i++)
        {
            resumeText.text = (totalTime - i).ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        yield return null;
        //Resume Time
        resumeText.gameObject.SetActive(false);
        ui.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnQuit()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.ReturnToMainMenu();
    }
}
