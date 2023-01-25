using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueApparition dialChooseDeath;
    [SerializeField] private DialogueApparition dialChooseLife;
    [SerializeField] private Ending end1;
    [SerializeField] private Ending end2;
    
    [SerializeField] private BossScript boss;

    [SerializeField] private GameObject Ange;
    [SerializeField] private GameObject Démon;
    
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);

        Ange.SetActive(false);
        Démon.SetActive(false);
        
        
        
        if (boss.isChoosingDeath)
        {
            end1.setDeath();
            end2.setDeath();
            dialChooseDeath.StartDialogue();
        }
        else
        {
            dialChooseLife.StartDialogue();
        }
        
    }
}
