using System.Collections;
using PlayerScript;
using Ui;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for enemies
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour
{
    #region Attributes
    
	//Enemy definition parameters
    protected int Life = 0;
    protected float Speed = 0;
    protected float Cooldown = 1;
    protected int Attack { get; private set; }
    protected Object Drop;

    protected float KnockbackSpeed = 0;
	//Serialized fields
    [SerializeField] protected EnemyData data;
    [SerializeField] protected Transform spriteRenderer;
    
    //Internal Parameters
    protected bool goingRight = true;
    protected NavMeshAgent agent;
    protected bool isCooldown = false;

    public static int number_enemy_left;

    public static int enemy_killed;
    
    #endregion
    
    #region MonoBehaviour Methods
    
    protected void Awake()
    {
        Life = data.life;
        Speed = data.speed;
        Attack = data.attack;
		Cooldown = data.cooldown;
        Drop = data.drop;
        number_enemy_left++;
        
        KnockbackSpeed = data.knockbackSpeed;
        //NavMesh
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.speed = Speed;
    }

    protected void Update()
    {
        Move();
    }

    ~Enemy()
    {
        number_enemy_left--;
    }
    
    
    #endregion

    #region Public Methods
    
    /// <summary>
    /// Reduce life of enemy by damage
    /// It dies if its life points drop below 0
    /// </summary>
    /// <param name="damage">The amount of damages</param>
    public virtual void TakeDamage(int damage)
    {
        Life -= damage;
        GetComponent<EnemyUi>().SetHealthBar(Life, data.life);
        if (Life <= 0)
        {
            enemy_killed++;
            //TODO : Add score and other interactions
            var testMeat = (GameObject) Instantiate(Drop);
            testMeat.transform.position = transform.position;
            Destroy(gameObject);
        }
        
    }
    
    public void takeKnockBack(Vector3 attackorigin)
    {
        Vector2 knockBackDirection = transform.position - attackorigin;
        StartCoroutine(PlayKnockBack(0.3f, knockBackDirection));


    }

    
    #endregion
    
    #region Protected Methods

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            other.transform.GetComponent<Player>().TakeDamage(Attack);
            StartCoroutine(WaitCooldown());
        }
    }
    
    protected IEnumerator WaitCooldown(){
        yield return new WaitForSeconds(Cooldown);
        isCooldown = false;
    }

    protected IEnumerator PlayKnockBack(float knockTime,Vector2 direction)
    {
        float speedAgent = agent.speed;
        agent.speed = 0;
        while (knockTime>0)
        {
            transform.Translate(direction.normalized * (KnockbackSpeed * Time.deltaTime));
            knockTime -= Time.deltaTime;
            yield return null;
        }
        print("finish");
        agent.speed = speedAgent;
    }

    #endregion
    
    #region Protected Override Methods
    
    protected virtual void Move()
    {
        
        //Calcul of goal
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        
        
        //Move analysis
        Vector3 pos = transform.position;
        Vector3 goal = Vector3.MoveTowards(pos, playerPos, Time.deltaTime * Speed);
        
        
        //Rotation on direction
        float deltaX = pos.x - goal.x;
        bool going = deltaX > 0;
        if (going != goingRight)
        {
            goingRight = going;
            spriteRenderer.Rotate(new Vector3(0,180,0));
        }

        //Moving
        agent.SetDestination(playerPos);
    }
    


    
    #endregion
    
}
