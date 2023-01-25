using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTrainDialogueTrigger2 : MonoBehaviour
{
    [SerializeField] private DialogueApparition dial;
    
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("ouais in trigger");
        if (other.CompareTag("Player"))
        {
            Debug.Log("player entrez");
            dial.StartDialogue();
        }
    }
}
