using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdTrainDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueApparition dial;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dial.StartDialogue();
        }
    }
}
