using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoundMuter : MonoBehaviour
{
    private GameObject SFXHolder;
    private GameObject MusicHolder;
    private bool hasSfx;
    private bool hasMusic;

    [SerializeField] private TextMeshProUGUI textMusic;
    [SerializeField] private TextMeshProUGUI textSfx;

    void Start()
    {
        if (Camera.main)
        {
            SFXHolder = Camera.main.gameObject.transform.GetChild(0).gameObject;
            MusicHolder = Camera.main.gameObject.transform.GetChild(1).gameObject;
        }
    }
    
    public void ToggleSfx()
    {
        hasSfx = !hasSfx;
        SFXHolder.GetComponent<AudioSource>().mute = hasSfx;
        textSfx.text = hasSfx ? "Unmute Sfx" : "Mute Sfx";
    }
    
    public void ToggleMusic()
    {
        hasMusic = !hasMusic;
        MusicHolder.GetComponent<AudioSource>().mute = hasMusic;
        textMusic.text = hasMusic ? "Unmute Music" : "Mute Music";
    }
    
}
