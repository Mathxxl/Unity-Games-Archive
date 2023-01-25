using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Enemy", menuName= "Game/Enemy")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyHP;
    [SerializeField] private Path enemyPath;
    [SerializeField] private List<PatternData> patterns; 
    
    public float EnemySpeed => enemySpeed;
    public float EnemyHP => enemyHP;
    public Path EnemyPath => enemyPath;
    public List<PatternData> Patterns => patterns;
}
