using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunCredits : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject backgroundBis;
    [SerializeField] private int waitingTime = 3;
    [SerializeField] private int transitioningTime = 1;
    private bool _running;

    IEnumerator ChangeColor(int waitTime, int transitionTime = 1)
    {
        while (true && _running)
        {
            yield return new WaitForSeconds(waitTime);
            
            Color newColor = Random.ColorHSV();
            Color oldColor = background.GetComponent<Image>().color;
            backgroundBis.GetComponent<Image>().color = newColor;

            while (background.GetComponent<Image>().color.a > 0)
            {
                yield return null;
                oldColor.a -= (Time.deltaTime / transitionTime);
                background.GetComponent<Image>().color = oldColor;
                //Debug.Log($"Transition Color alpha is {background.GetComponent<Image>().color.a}");
            }

            background.GetComponent<Image>().color = newColor;

            _running = (background != null);
        }
    }

    void Start()
    {
        _running = (background != null);
        StartCoroutine(ChangeColor(waitingTime, transitioningTime));
    }
    
}
