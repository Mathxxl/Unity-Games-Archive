using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : Singleton<Game>
{
    [SerializeField] private Enemy template;
    [SerializeField] private PlayerData playerData;
    [SerializeField] private DashUI dashUI;
    [SerializeField] private LifeUI lifeUI;
    [SerializeField] private Deplacement deplacement;
    [SerializeField] private int numberEnemies;
    [SerializeField] private int EnemiesSameTime;
    
    
    private readonly List<Enemy> _enemies = new List<Enemy>();

    public Enemy RemoveEnemie
    {
        set => _enemies.Remove(value);
    }
    
    
    [SerializeField] private GameObject boss;

    private int ennemySpawn = 0;

    
    public Player Player
    {
        get
        {
            if (_player == null)
            {
                _player = new Player(playerData, dashUI, lifeUI);
            }
            return _player;
        }
}

    private Player _player;
    public List<Enemy> Enemies => _enemies;
    

    private void AddEnemy()
    { 
        var allEnemyData = GameRessources.Instance.Enemies;
        var index = Random.Range(0, allEnemyData.Count);
        var enemyData = allEnemyData[index];
        var enemy = Instantiate(template);
        var ene = enemy.GetComponent<Enemy>();
        ene.Init(enemyData);
        _enemies.Add(ene);
        ennemySpawn++;
    }
    
    private void AddBoss()
    {
        var b = Instantiate(boss);
        b.GetComponent<Enemy>().Init(GameRessources.Instance.Boss);
    }

    protected void Start()
    {
        if(numberEnemies != 0)
            AddEnemy();

        Time.timeScale = 1;
        StartCoroutine(EnemyKeepSpawning());
        deplacement.Init(playerData, Player);
        
    }

    private IEnumerator EnemyKeepSpawning()
    {
        while (true)
        {
            if (_enemies.Count >= EnemiesSameTime)
            {
                yield return null;
            }
            else
            {
                if (numberEnemies == ennemySpawn)
                {
                    if (_enemies.Count == 0)
                    {
                        AddBoss();
                        yield break;
                    }
                    else
                    {
                        yield return null;
                    }
                }
                else
                {
                    float dt = Random.Range(5, 10);
                    yield return new WaitForSeconds(dt);
                    AddEnemy();    
                }
            }
        }
    }
}