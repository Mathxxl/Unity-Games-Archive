using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] ParticleSystem explosion;
    [SerializeField] EnemyData enemyData;
    private float hp;
    private Enemy _enemy;
    [SerializeField] private Pattern _pattern;

    public event Action OnKill;
    
    protected void Start()
    {
        _enemy = GetComponent<Enemy>();
        _pattern.StartShoot();
    }
    
    public void Init(EnemyData data)
    {
        enemyData = data;
        hp = data.EnemyHP;
        _pattern.Patterns = data.Patterns;
        GetComponent<FollowPath>().SetPath(data.EnemyPath);
        GetComponent<FollowPath>().SetSpeed(data.EnemySpeed);
    }


    public void ReceiveDamage (float damage)
    {
        if (damage > 0)
        {
            hp -= damage;
            if (hp <= 0)
            {
                GetKilled ();
            }
        }
    }

    public void GetKilled ()
    {
        var exp = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(exp.gameObject,0.5f);
        Game.Instance.RemoveEnemie = _enemy;
        if (OnKill != null) OnKill();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectiles"))
        {
            ReceiveDamage(1);
        }
    }
}
