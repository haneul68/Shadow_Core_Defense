using UnityEngine;

public class Player_Spawner : MonoBehaviour
{
    [Header("REF")]
    [SerializeField]
    private Player_State_Bar_Manager player_State_Bar_Manager;
    [SerializeField] 
    private Transform spawn_Point;

    private void Start()
    {
        Spawn_Player();
    }

    public void Spawn_Player()
    {
        string ch_Name = Base_Manager.Character_Mng.Equipped_Character_Name;

        if (string.IsNullOrEmpty(ch_Name))
        {
            Debug.LogWarning("장착된 캐릭터 없음");
            return;
        }

        if (!Base_Manager.Data_Mng.p_Character_Holder.ContainsKey(ch_Name))
        {
            Debug.LogError($"캐릭터 데이터 없음 : {ch_Name}");
            return;
        }

        Character_Holder holder = Base_Manager.Data_Mng.p_Character_Holder[ch_Name];
        Character_Scriptable data = holder.Data;

        if (data == null || data.Character_Prefab == null)
        {
            Debug.LogError("캐릭터 프리팹 없음");
            return;
        }

        GameObject player_Obj = GameObject.Instantiate(data.Character_Prefab);

        player_Obj.transform.position = spawn_Point != null ? spawn_Point.position : Vector3.zero;

        Player player = player_Obj.GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("player 컴포넌트 없음");
            return;
        }

        player.Init_Player(data);
        Base_Manager.Character_Mng.Set_Current_Character(player);

        Camera_Manager.Instance.Set_Target(player.transform);

        player_State_Bar_Manager.Set_Player(player.gameObject);

        Debug.Log($"플레이어 생성 : {data.Character_Name}");
    }
}