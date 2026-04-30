using System.Collections;
using UnityEngine;

public class Base_Manager : MonoBehaviour
{
    public static Base_Manager Instance;

    private static Data_Manager Data_Manager = new Data_Manager();
    private static Character_Manager Character_Manager = new Character_Manager();
    private static Pool_Manager Pool_Manager = new Pool_Manager();
    private static Enemy_Manager Enemy_Manager = new Enemy_Manager();
    private static UI_Manager UI_Manager = new UI_Manager();
    private static Inventory_Manager Inventory_Manager = new Inventory_Manager();
    private static Rune_Manager Rune_Manager = new Rune_Manager();
    private static Sound_Manager Sound_Manager = new Sound_Manager();
    public static Data_Manager Data_Mng { get => Data_Manager; }
    public static Character_Manager Character_Mng { get => Character_Manager; }
    public static Pool_Manager Pool_Mng { get => Pool_Manager; }
    public static Enemy_Manager Enemy_Mng { get => Enemy_Manager; }
    public static UI_Manager UI_Mng { get => UI_Manager; }
    public static Inventory_Manager Inventory_Mng { get => Inventory_Manager; }
    public static Rune_Manager Rune_Mng { get => Rune_Manager; }
    public static Sound_Manager Sound_Mng { get => Sound_Manager; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Init();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Init()
    {
        Data_Mng.Init();
        Pool_Mng.Init(this.transform);
        Inventory_Mng.Init();
        Rune_Mng.Init();
        Sound_Mng.Init();   
    }

    #region Pool_Mng
    public GameObject Get_Prefab_OBJ(string path) 
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }
    public void Return_Pool_Obj_Delay(Pool_ID path, GameObject obj, float delay)
    {
        StartCoroutine(Return_Pool_Obj_Delay_Coroutine(path, obj, delay));
    }
    private IEnumerator Return_Pool_Obj_Delay_Coroutine(Pool_ID path, GameObject obj , float delay)
    {
        yield return new WaitForSeconds(delay); 

        if (Base_Manager.Pool_Mng.pool_Dictionary.TryGetValue(path, out var pool))
        {
            pool.Return(obj);
        }
        else
        {
            Debug.LogWarning($"풀 경로 {path} 존재하지 않음");
        }
    }
    #endregion
}
