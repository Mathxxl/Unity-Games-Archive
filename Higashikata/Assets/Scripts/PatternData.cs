using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Pattern", menuName= "Game/Pattern")]
public class PatternData : ScriptableObject
{
    [SerializeField] private List<subList> angles;
    [SerializeField] private float speed;
    [SerializeField] private List<float> origins;
    [SerializeField] private List<GameObject> projectiles;
    [SerializeField] private float shootingTime;
    [SerializeField] private bool tirEnMemeTemps;
    [SerializeField] private List<float> latences;

    public List<subList> Angles => angles;
    public float Speed => speed;
    public List<float> Origins => origins;
    public List<GameObject> Projectiles => projectiles;
    public float ShootingTime => shootingTime;
    public bool TirEnMemeTemps => tirEnMemeTemps;
    public List<float> Latences => latences;
}

[System.Serializable]
public class subList
{
    public List<float> list;
}
