using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeventhDialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueApparition dial;

    [SerializeField] private Player _player;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        _player._canMove = false;
        
        yield return new WaitForSeconds(1.5f);
        
        dial.StartDialogue();
        
    }
}
