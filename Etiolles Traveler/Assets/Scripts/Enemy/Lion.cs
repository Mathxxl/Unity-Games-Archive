using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Lion : Enemy
{
    private bool jump = false;
    private bool jumping = false;
    private Animator animator;
    private static readonly int Attacking = Animator.StringToHash("Attacking");
    [SerializeField] private bool active = true;
    [SerializeField] private UnityEvent onLionDeath;
    
    
    private void Start()
    {
        StartCoroutine(RandomJump());
        animator = transform.GetChild(1).GetComponent<Animator>();
    }

    ~Lion()
    {
        GameManager.Instance.OnLionDeath.Invoke();
    }
    

    protected override void Move()
    {
        if (jump)
        {
            if (!jumping)
            {
                StartCoroutine(Jumping());
            }
            
        }
        else
        {
            base.Move();
        }
    }

    private IEnumerator Jumping()
    {
        jumping = true;
        agent.speed = 0;
        
        animator.SetBool(Attacking, true);
        yield return new WaitForSeconds((2f-(1/6f)));

        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 direction = Vector3.MoveTowards(transform.position, playerPos, Time.deltaTime);
        Debug.Log($"direction is {direction}");
        direction.Normalize();
        Debug.Log($"Moving Towards {direction}");
        
        float timer = 0;

        while (timer < (1/6f))
        {
            float delta = Time.deltaTime;
            timer += delta;
            transform.Translate(50*direction*delta);
            yield return null;
        }
        
        jump = false;
        animator.SetBool(Attacking, false);
        agent.speed = Speed;
        yield return null;
    }

    private IEnumerator RandomJump()
    {
        while (active)
        {
           int randomTime = Random.Range(10,30);
            Debug.Log($"Waiting for {randomTime}"); 
            yield return new WaitForSeconds(randomTime);
            jump = true; 
        }
    }
    
}
