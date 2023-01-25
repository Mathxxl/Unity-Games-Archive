using UnityEngine;

public class Boss : MonoBehaviour
{

    [SerializeField] private GameObject victoryMenu;

    private bool _isVictory = false;

    public bool isVictory => _isVictory;

    private GameObject menu;
    
    void Start()
    {
        menu = Instantiate(victoryMenu);
        menu.SetActive(false);
        GetComponent<Enemy>().OnKill += GetKill;
    }

    private void GetKill()
    {
        menu.SetActive(true);
        Time.timeScale = 0;
        _isVictory = true;
        
        TestPause.Instance.CanPause = false;
    }
}
