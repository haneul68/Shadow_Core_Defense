using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Spawn_Manager : MonoBehaviour
{
    public static Enemy_Spawn_Manager Instance { get; private set; }

    [Header("Spawn_Center")]
    [SerializeField] private Transform spawn_Center_Obj;

    [Space(20)]
    [Header("Spawn_Radius")]
    [SerializeField] private float min_Radius;
    [SerializeField] private float max_Radius;

    [Space(20)]
    [Header("Round_Data")]
    [SerializeField] private List<Round_Data> round_Data;

    [Space(20)]
    [Header("Spawn_Delay")]
    [SerializeField] private float spawn_Delay;

    [Space(20)]
    [Header("Spawn_Range")]
    [SerializeField] private int min_Spawn_Range;
    [SerializeField] private int max_Spawn_Range;

    [Space(20)]
    [Header("Boss_Spawn_Point")]
    [SerializeField] private Transform[] boss_Spawn_Point;

    private List<Enemy_Base> spawn_Enemys = new List<Enemy_Base>();
    private List<Enemy_Base> boss_Spawned_Enemys = new List<Enemy_Base>();

    private int alive_Count = 0;

    public static event Action<int, int> On_Enemy_Count_Changed;

    private int kill_Count = 0;

    private Coroutine spawn_Coroutine;
    private Coroutine boss_Spawn_Coroutine;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #region SPAWN_SYSTEM
    public void Spawn_Round_Enemy(int round_index)
    {
        kill_Count = 0;

        if (round_index >= round_Data.Count)
        {
            Debug.Log("round_index >= round_Data.Count");
            return;
        }

        Round_Data data = round_Data[round_index];
        if (spawn_Coroutine != null)
        {
            StopCoroutine(spawn_Coroutine);
            spawn_Coroutine = null;
        }
        spawn_Coroutine = StartCoroutine(Spawn_Coroutine(data));
    }

    public void Start_Boss_Spawn_Skill(Round_Data data)
    {
        if (boss_Spawn_Coroutine != null) 
        {
            StopCoroutine (boss_Spawn_Coroutine);
            boss_Spawn_Coroutine = null;
        }
        boss_Spawn_Coroutine = StartCoroutine(Spawn_Coroutine(data, true));
    }

    private IEnumerator Spawn_Coroutine(Round_Data data, bool is_Boss_Spawn = false)
    {
        List<Pool_ID> spawn_List = new List<Pool_ID>();

        Add(spawn_List, Pool_ID.Enemy_Magma_Slime, data.magma_Count);
        Add(spawn_List, Pool_ID.Enemy_Blue_Slime, data.blue_Count);
        Add(spawn_List, Pool_ID.Enemy_Green_Slime, data.green_Count);

        if (is_Boss_Spawn == false)
        {
            alive_Count = spawn_List.Count;
        }

        Shuffle(spawn_List);

        int index = 0;
        while (index < spawn_List.Count)
        {
            if (Round_Manager.Instance.Game_Ended)
                yield break;

            int batch_Size = UnityEngine.Random.Range(min_Spawn_Range, max_Spawn_Range + 1);

            for (int i = 0; i < batch_Size && index < spawn_List.Count; i++)
            {
                if (Round_Manager.Instance.Game_Ended)
                    yield break;

                Spawn(spawn_List[index], is_Boss_Spawn);
                index++;
            }

            yield return new WaitForSeconds(spawn_Delay);
        }
    }

    private void Add(List<Pool_ID> list, Pool_ID id, int count)
    {
        for (int i = 0; i < count; i++)
            list.Add(id);
    }

    private void Shuffle(List<Pool_ID> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = UnityEngine.Random.Range(0, i + 1);

            Pool_ID temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private void Spawn(Pool_ID path, bool is_Boss_Spawn = false)
    {
        Vector2 pos = Get_Random_Position();

        Base_Manager.Pool_Mng.Pooling_OBJ(path).Get(enemy =>
        {
            enemy.transform.position = pos;
            Enemy_Base enemy_Clone = enemy.GetComponent<Enemy_Base>();

            enemy.GetComponent<Enemy>().ReSpawn_Enemy_Init();

            if (is_Boss_Spawn == true)
            {
                boss_Spawned_Enemys.Add(enemy_Clone);
            }
            else 
            {
                spawn_Enemys.Add(enemy_Clone);
            }

            Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Minimap_Enemy_Icon).Get(icon =>
            {
                icon.SetActive(true);

                RectTransform rect = icon.GetComponent<RectTransform>();

                rect.SetParent(In_Game_Canvas.Instance.Minimap_Rect, false);

                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.zero;
                rect.pivot = new Vector2(0.5f, 0.5f);

                rect.localScale = Vector3.one;
                rect.localRotation = Quaternion.identity;
                rect.anchoredPosition = Vector2.zero;

                enemy_Clone.minimap_Icon = rect;

                Minimap_Manager.Instance.Register(enemy_Clone);
            });

            Get_Round_Max_Enemy_Count(Round_Manager.Current_Round, out int max_Enemy_value);
            On_Enemy_Count_Changed?.Invoke(max_Enemy_value, kill_Count);
        });
    }
    #endregion

    #region SPAWN_POINT
    public Vector2 Get_Random_Position()
    {
        if (spawn_Center_Obj == null)
        {
            Debug.LogError("spawn_Center_Obj == null");
            return Vector2.zero;
        }

        if (min_Radius >= max_Radius)
        {
            Debug.LogError("min_Radius >= max_Radius");
            return spawn_Center_Obj.position;
        }

        int safety = 0;
        const int max_Try = 100;

        while (safety < max_Try)
        {
            safety++;

            float x = UnityEngine.Random.Range(-max_Radius, max_Radius);
            float y = UnityEngine.Random.Range(-max_Radius, max_Radius);

            if (Mathf.Abs(x) < min_Radius && Mathf.Abs(y) < min_Radius)
                continue;

            return (Vector2)spawn_Center_Obj.position + new Vector2(x, y);
        }

        return spawn_Center_Obj.position;
    }

    private void OnDrawGizmosSelected()
    {
        if (spawn_Center_Obj == null) return;

        Vector3 center = spawn_Center_Obj.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, Vector2.one * (min_Radius * 2));

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, Vector2.one * (max_Radius * 2));
    }
    #endregion

    #region SPAWNED_ENEMY_LIST

    public void Remove_Enemy(Enemy_Base enemy)
    {
        if (enemy == null) return;

        bool is_Boss_Spawned = boss_Spawned_Enemys.Contains(enemy);

        if (is_Boss_Spawned)
        {
            boss_Spawned_Enemys.Remove(enemy);
        }
        else
        {
            if (spawn_Enemys.Contains(enemy))
            {
                spawn_Enemys.Remove(enemy);
                kill_Count++;
                alive_Count--;

                Get_Round_Max_Enemy_Count(Round_Manager.Current_Round, out int max_Enemy_value);
                On_Enemy_Count_Changed?.Invoke(max_Enemy_value, kill_Count);
            }
        }

        if (enemy.minimap_Icon != null)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Minimap_Enemy_Icon]
                .Return(enemy.minimap_Icon.gameObject);

            enemy.minimap_Icon = null;
        }

        if (Minimap_Manager.Instance != null)
            Minimap_Manager.Instance.Unregister(enemy);

        if (!is_Boss_Spawned && alive_Count <= 0 && InGame_State_Manager.m_state != InGame_State.BOSS_BATTLE)
        {
            Round_Manager.Instance.On_Enemy_All_Dead();
        }
    }

    public void Remove_Boss(Enemy_Base enemy)
    {
        if (spawn_Enemys == null || spawn_Enemys.Count <= 0)
        {
            Debug.Log("spawn_Enemys == null || spawn_Enemys.Count <= 0");
            return;
        }

        spawn_Enemys.Remove(enemy);
        alive_Count = 0;

        if (enemy.minimap_Icon != null)
        {
            Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Minimap_Enemy_Icon].Return(enemy.minimap_Icon.gameObject);

            enemy.minimap_Icon = null;
        }

        Minimap_Manager.Instance.Unregister(enemy);

        if (alive_Count <= 0)
        {
            Round_Manager.Instance.On_Boss_Dead();
        }
    }

    public void Get_Round_Max_Enemy_Count(int round_index, out int max_Enemy_Count)
    {
        if (spawn_Enemys == null)
        {
            max_Enemy_Count = -999;
            Debug.Log("spawn_Enemys == null || spawn_Enemys.Count <= 0");
            return;
        }
        Round_Data data = round_Data[round_index];
        max_Enemy_Count = data.blue_Count + data.green_Count + data.magma_Count;
    }

    public void Get_Current_Enemy_Count(out int curent_Enemy_Count)
    {
        if (spawn_Enemys == null)
        {
            curent_Enemy_Count = -999;
            Debug.Log("spawn_Enemys == null || spawn_Enemys.Count <= 0");
            return;
        }
        curent_Enemy_Count = spawn_Enemys.Count;
    }
    #endregion

    public void Spawn_Boss(int round_index)
    {
        spawn_Enemys.Clear();
        kill_Count = 0;

        if (round_index >= round_Data.Count)
        {
            Debug.Log("Boss round index out of range");
            return;
        }

        Round_Data data = round_Data[round_index];

        Pool_ID bossID = Get_Boss_ID(data);

        Spawn_Boss_Internal(bossID);
    }
    private void Spawn_Boss_Internal(Pool_ID boss_ID)
    {
        int rand_Index = UnityEngine.Random.Range(0, boss_Spawn_Point.Length);
        Transform selected_Point = boss_Spawn_Point[rand_Index];

        Vector2 pos = selected_Point.position;

        Base_Manager.Pool_Mng.Pooling_OBJ(boss_ID).Get(enemy =>
        {
            enemy.transform.position = pos;

            Enemy_Base enemy_Clone = enemy.GetComponent<Enemy_Base>();

            if (enemy.TryGetComponent<Boss>(out var boss))
            {
                boss.Init_Boss(boss_Spawn_Point, rand_Index);
            }
            else
            {
                Debug.LogError("보스 없음");
                return;
            }

            Base_Manager.Pool_Mng.Pooling_OBJ(Pool_ID.Minimap_Enemy_Icon).Get(icon =>
            {
                icon.SetActive(true);

                RectTransform rect = icon.GetComponent<RectTransform>();

                rect.SetParent(In_Game_Canvas.Instance.Minimap_Rect, false);

                rect.anchorMin = Vector2.zero;
                rect.anchorMax = Vector2.zero;
                rect.pivot = new Vector2(0.5f, 0.5f);

                rect.localScale = Vector3.one;
                rect.localRotation = Quaternion.identity;
                rect.anchoredPosition = Vector2.zero;

                enemy_Clone.minimap_Icon = rect;

                Minimap_Manager.Instance.Register(enemy_Clone);
            });
            spawn_Enemys.Add(enemy_Clone);

            Get_Round_Max_Enemy_Count(Round_Manager.Current_Round, out int max);
            On_Enemy_Count_Changed?.Invoke(max, kill_Count);
        });
    }
    private Pool_ID Get_Boss_ID(Round_Data data)
    {
        return data.boss_ID;
    }

    public void Stop_All() 
    {
        StopAllCoroutines();
        Return_All_Spawned_Enemies();
        Return_Boss_Spawned_Enemies();
    }

    public void Return_All_Spawned_Enemies()
    {
        if (spawn_Enemys == null || spawn_Enemys.Count <= 0)
        {
            return;
        }
        for (int i = spawn_Enemys.Count - 1; i >= 0; i--)
        {
            Enemy_Base enemy = spawn_Enemys[i];
            if (enemy == null) continue;

            if (enemy.TryGetComponent<Health_Manager>(out var hp))
            {
                hp.Return_HP_Bar();
            }

            if (enemy.minimap_Icon != null)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Minimap_Enemy_Icon].Return(enemy.minimap_Icon.gameObject);
                enemy.minimap_Icon = null;
            }
            Minimap_Manager.Instance.Unregister(enemy);

            Base_Manager.Pool_Mng.pool_Dictionary[enemy.Enemy_Pool_ID].Return(enemy.gameObject);
        }

        spawn_Enemys.Clear();
        alive_Count = 0;
        kill_Count = 0;
    }

    public void Return_Boss_Spawned_Enemies()
    {
        if (boss_Spawn_Coroutine != null)
        {
            Debug.Log("보스 슬라임 소환 진행중");
            StopCoroutine(boss_Spawn_Coroutine);
            boss_Spawn_Coroutine = null;
        }

        Debug.Log("보스 소환몹 삭제 시작");
        for (int i = boss_Spawned_Enemys.Count - 1; i >= 0; i--)
        {
            Enemy_Base enemy = boss_Spawned_Enemys[i];
            if (enemy == null) continue;

            if (enemy.TryGetComponent<Health_Manager>(out var hp))
            {
                hp.Return_HP_Bar();
            }

            if (enemy.minimap_Icon != null)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[Pool_ID.Minimap_Enemy_Icon].Return(enemy.minimap_Icon.gameObject);
                enemy.minimap_Icon = null;
            }

            Minimap_Manager.Instance.Unregister(enemy);
            boss_Spawned_Enemys.Remove(enemy);
            Base_Manager.Pool_Mng.pool_Dictionary[enemy.Enemy_Pool_ID].Return(enemy.gameObject);
        }

        boss_Spawned_Enemys.Clear();
        Debug.Log("보스 소환몹 삭제 완료");
    }
}