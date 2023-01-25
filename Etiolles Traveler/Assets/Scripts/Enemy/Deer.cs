using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deer : Enemy
{
    private Animator animator;
    private static readonly int hashRun = Animator.StringToHash("Run"); 
    private static readonly int hashWalk = Animator.StringToHash("Walk"); 
    private static readonly int hashIdle = Animator.StringToHash("Chill");

    private bool panic = false;
    private bool isCalming = false;
    private bool isEating = true;
    private bool isMoving = false;
    //private bool isEatingBis = false;

    private void Start()
    {
        animator = transform.GetChild(1).GetComponent<Animator>();
    }

    protected override void Move()
    {
        if (panic)
        {
            PanicMove();
        }
        else
        {
            NormalMove();  
        }

    }

    private void PanicMove()
    {
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 direction = (-30)*Vector3.MoveTowards(transform.position, playerPos, Time.deltaTime);
        direction.Normalize();
        agent.SetDestination(direction);
        //Rotation on direction
        Vector3 pos = transform.position;
        float deltaX = pos.x - direction.x;
        bool going = deltaX > 0;
        if (going != goingRight)
        {
            goingRight = going;
            spriteRenderer.Rotate(new Vector3(0,180,0));
        }
    }

    private void NormalMove()
    {
        if (isEating)
        {
            //Debug.Log($"Eating {isEating} ; isMoving {isMoving}");
            StartCoroutine(Eat());
        } else if (isMoving)
        {
            //Debug.Log($"Moving {isMoving} ; isEating {isEating}");
            Vector3 pos = transform.position;
            float newX = pos.x + Random.Range(-10f, 10f);
            float newY = pos.y + Random.Range(-10f, 10f);
            Vector3 newDirection = new Vector3(newX, newY, pos.z);
            //Debug.Log($"New Direction is {newDirection}");
            agent.SetDestination(newDirection);
            StartCoroutine(Walking());
        }
    }

    private IEnumerator Eat()
    {
        //Debug.Log("Start Coroutine Eat");
        isEating = false;
        //StartCoroutine(WaitForAnimation(0));
        Idle();
        //yield return new WaitUntil(() => isEatingBis);
        yield return new WaitForSeconds(2); // 2s is the length of the animation
        if (!panic)
        {
            isEating = false;
            //isEatingBis = false;
            isMoving = true;
        }

        yield return null;

    }

    private IEnumerator Walking()
    {
        //Debug.Log("Start Coroutine Walking");
        isMoving = false;
        Walk();
        yield return new WaitUntil(() => agent.velocity.magnitude < 0.1f);
        if (!panic)
        {
            isMoving = false;
            isEating = true;
        }

        yield return null;

    }
    
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        panic = true;
        isCalming = false;
        Run();
        agent.speed += 5;
        StartCoroutine(Calming());
    }

    private IEnumerator Calming()
    {
        isCalming = true;
        yield return new WaitForSeconds(2);
        if (isCalming)
        {
            panic = false;
            Idle();
            agent.SetDestination(transform.position);
            agent.speed -= 5;
        }
    }
    
    private void Run()
    {
        animator.SetBool(hashRun, true);
        animator.SetBool(hashIdle, false);
        animator.SetBool(hashWalk, false);
    }

    private void Walk()
    {
        animator.SetBool(hashWalk, true);
        animator.SetBool(hashIdle, false);
    }

    private void Idle()
    {
        animator.SetBool(hashIdle, true);
        animator.SetBool(hashWalk, false);
        animator.SetBool(hashRun, false);
    }
    
    //DEPRECATED BECAUSE IT IS SHIT
    private IEnumerator WaitForAnimation(int i)
    {
        AnimatorClipInfo[] animClip;
        animClip = animator.GetCurrentAnimatorClipInfo(0);
        string name = animClip[i].clip.name;
        float length = animClip[i].clip.length;
        //Debug.Log($"Animation {name} of length {length}");
        yield return new WaitForSeconds(length);
        switch (i)
        {
            case 0:
                //isEatingBis = true;
                break;
            case 1:
                isMoving = true;
                break;
            default:
                isEating = false;
                isMoving = false;
                break;
        }
    }
}
