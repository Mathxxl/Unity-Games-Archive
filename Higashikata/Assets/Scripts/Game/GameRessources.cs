using System.Collections.Generic;
using UnityEngine;

public class GameRessources : Singleton<GameRessources>
{

    [SerializeField] private EnemyData[] enemies;
    
    public IReadOnlyList<EnemyData> Enemies => enemies;
    
    [SerializeField] private EnemyData boss;
    
    public EnemyData Boss => boss;

}
