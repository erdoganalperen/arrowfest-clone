using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public Transform parent;
    }
    private Dictionary<string, Queue<GameObject>> _poolDictionary;
    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, pool.parent);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            _poolDictionary.Add(pool.tag, objectPool);
        }
    }

    [SerializeField] private List<Pool> pools;
    private void Start()
    {

    }
    void CreateObjectInPool(string tag)
    {
        //check is tag added to pools
        Pool pool = pools.Find(x => x.tag == tag);
        if (pool == null)
        {
            print("tag doesnt exist");
            return;
        }
        var instantiated = Instantiate(pool.prefab, pool.parent);
        instantiated.SetActive(false);
        _poolDictionary[tag].Enqueue(instantiated);
    }
    public GameObject InstantiateFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            print("tag doesnt exist");
            return null;
        }
        GameObject obj;
        if (_poolDictionary[tag].Count > 0)
        {
            obj = _poolDictionary[tag].Dequeue();
        }
        else
        {
            CreateObjectInPool(tag);
            obj = _poolDictionary[tag].Dequeue();
        }
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);
        return obj;
    }
    public GameObject InstantiateFromPool(string tag)
    {
        if (!_poolDictionary.ContainsKey(tag))
        {
            print("tag doesnt exist");
            return null;
        }
        GameObject obj;
        if (_poolDictionary[tag].Count > 0)
        {
            obj = _poolDictionary[tag].Dequeue();
        }
        else
        {
            CreateObjectInPool(tag);
            obj = _poolDictionary[tag].Dequeue();
        }
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(string tag, GameObject gameObject)
    {
        gameObject.SetActive(false);
        _poolDictionary[tag].Enqueue(gameObject);
    }
}
