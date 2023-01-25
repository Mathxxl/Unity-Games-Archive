using System.Collections.Generic;
using UnityEngine;

//Classe pour le pool d'objets
public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        [SerializeField] private string tag;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int size;
        public int Size => size;
        public GameObject Prefab => prefab;
        public string Tag => tag;

        public Pool(string t, GameObject p, int s)
        {
            tag = t;
            prefab = p;
            size = s;
        }
    }

    List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDict;

    protected override void OnAwake()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();
        pools = new List<Pool>();
    }
    
    public void Init()
    {
        foreach (var pool in pools)
        {
            AddPool(pool);
        }
    }

    public GameObject Spawn(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("Tag cannot be found in the pool");
            return null;
        }
        
        var obj = poolDict[tag].Dequeue();
        
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);
        poolDict[tag].Enqueue(obj);

        return obj;
    }

    public void AddPool(Pool pool)
    {
        if (poolDict.ContainsKey(pool.Tag))
        {
            return;
        }
        
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.Size; i ++)
        {
            var obj = Instantiate(pool.Prefab, transform);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
            
        poolDict.Add(pool.Tag, objectPool);
    }
}
