using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EnemyData", order = 1)]
public class EnemyData : ScriptableObject
{
    public int life;
    public float speed;
    public int attack;
    public float cooldown;
    public Object drop;
    public float knockbackSpeed;
}
