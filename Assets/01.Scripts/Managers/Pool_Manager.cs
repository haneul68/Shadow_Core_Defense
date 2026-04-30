using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPool
{
    Transform parent_Transform { get; set; }
    public Queue<GameObject> pool { get; set; }

    GameObject Get(Action<GameObject> action = null);

    void Return(GameObject obj, Action<GameObject> action = null);
}

public class Object_Pool : IPool
{
    public Transform parent_Transform { get; set; }
    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();

    public GameObject Get(Action<GameObject> action = null)
    {
        GameObject obj = pool.Dequeue();
        obj.SetActive(true);

        action?.Invoke(obj);

        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        pool.Enqueue(obj);
        obj.transform.SetParent(parent_Transform, false);
        obj.SetActive(false);
        action?.Invoke(obj);
    }
}

public class Pool_Manager
{
    public Dictionary<Pool_ID, IPool> pool_Dictionary = new Dictionary<Pool_ID, IPool>();
    Transform base_Parents;
    public void Init(Transform T)
    {
        base_Parents = T;
    }
    public IPool Pooling_OBJ(Pool_ID path)
    {
        if (pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }
        if (pool_Dictionary[path].pool.Count <= 0)
        {
            Add_Queue(path);
        }
        return pool_Dictionary[path];
    }

    public GameObject Add_Pool(Pool_ID path)
    {
        GameObject obj = new GameObject("##" + path);
        obj.transform.parent = base_Parents;

        Object_Pool pool = new Object_Pool();

        pool_Dictionary[path] = pool;

        pool.parent_Transform = obj.transform;

        return obj;
    }

    public void Add_Queue(Pool_ID path)
    {

        string prefab_Path = Pool_Path.Get_Path(path);

        var go = Base_Manager.Instance.Get_Prefab_OBJ("Pool_Obj/" + prefab_Path);

        go.name = path.ToString();

        go.transform.SetParent(pool_Dictionary[path].parent_Transform, false);

        pool_Dictionary[path].Return(go);
    }
}
