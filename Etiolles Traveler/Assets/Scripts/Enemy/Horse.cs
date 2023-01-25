using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Horse : Enemy
{
    private bool moving = false;

    private void Start()
    {
        agent.speed /= 2;
    }

    protected override void Move()
    {
        if (!moving)
        {
            moving = true;
            
            Vector3 destination = new Vector3();
            NavMeshPath navMeshPath = new NavMeshPath();

            do
            {
                float newX = Random.Range(-35, 35);
                float newY = Random.Range(-17, 17);
                destination = new Vector3(newX, newY, transform.position.z);
            } while (!agent.CalculatePath(destination, navMeshPath) &&
                     navMeshPath.status == NavMeshPathStatus.PathComplete);

            agent.SetDestination(destination);
            
            //Rotation on direction
            float deltaX = transform.position.x - agent.destination.x;
            bool going = deltaX > 0;
            if (going != goingRight)
            {
                goingRight = going;
                spriteRenderer.Rotate(new Vector3(0,180,0));
            }
            StartCoroutine(RandomWalk());
        }
    }
    
    private IEnumerator RandomWalk()
    {
        moving = true;
        yield return new WaitUntil(() => agent.velocity.magnitude < 0.01f);
        moving = false;
    }

    private IEnumerator Calming()
    {
        yield return new WaitForSeconds(3);
        agent.speed /= 2;
    }
    

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        agent.speed *= 2;
        StartCoroutine(Calming());
    }
}
