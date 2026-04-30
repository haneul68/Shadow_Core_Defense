using TMPro;
using UnityEngine;

public class In_Game_Canvas : Base_Canvas
{
    public static In_Game_Canvas Instance { get; private set; }

    [Space(20)]
    [Header("In_Game_Info")]
    [SerializeField] 
    private GameObject enemy_Count_Obj;
    [SerializeField] 
    private TextMeshProUGUI enemy_Count_Text;
    [SerializeField] 
    private TextMeshProUGUI timer_Text;
    [SerializeField] 
    private TextMeshProUGUI[] current_Round_Text;
    [SerializeField] 
    private GameObject Count_Down_Obj;
    [SerializeField] 
    private TextMeshProUGUI Count_Down_Text;
    [SerializeField] 
    private Transform minimap_Rect;
    [SerializeField] 
    private UI_End_Game_Panel end_Game_Panel;

    [SerializeField]
    private UI_Character_Stat_Info character_Stat_Info;

    public Transform Minimap_Rect => minimap_Rect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Base_Manager.UI_Mng.Init(this);
    }

    private void OnEnable()
    {
        Enemy_Spawn_Manager.On_Enemy_Count_Changed += Update_Enemy_Count_Text;
        Round_Manager.On_Round_Text_Changed += Update_Round_Text;
        Round_Manager.On_Timer_Changed += Update_Timer_Text;
        Round_Manager.On_Count_Down += Update_Count_Down_Text;

        Delegate_Holder.OnBossReady += Hide_Enemy_Count;
        Delegate_Holder.OnReady += End_Boss_Round;

        Delegate_Holder.OnVictory += End_Game_Panels;
        Delegate_Holder.OnDeath += End_Game_Panels;
    }

    private void OnDisable()
    {
        Enemy_Spawn_Manager.On_Enemy_Count_Changed -= Update_Enemy_Count_Text;
        Round_Manager.On_Round_Text_Changed -= Update_Round_Text;
        Round_Manager.On_Timer_Changed -= Update_Timer_Text;
        Round_Manager.On_Count_Down -= Update_Count_Down_Text;

        Delegate_Holder.OnBossReady -= Hide_Enemy_Count;
        Delegate_Holder.OnReady -= End_Boss_Round;

        Delegate_Holder.OnVictory -= End_Game_Panels;
        Delegate_Holder.OnDeath -= End_Game_Panels;
    }
    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (character_Stat_Info == null)
            {
                Debug.Log("character_Stat_Info == null");
                return;
            }

            character_Stat_Info.Init();
        }
    }
    private void Update_Enemy_Count_Text(int max, int current)
    {
        enemy_Count_Text.text = $"{current:00}/{max:00}";
    }

    private void Update_Round_Text(int round)
    {
        string text = $"{round + 1:00}";

        foreach (TextMeshProUGUI tmp in current_Round_Text)
        {
            if (tmp == null) continue;
            tmp.text = text;
        }
    }

    private void Update_Timer_Text(float time)
    {
        time = Mathf.Max(0f, time);

        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timer_Text.text = $"{minutes:00}:{seconds:00}";
    }

    private void Update_Count_Down_Text(int value)
    {
        Count_Down_Obj_Active(true);
        Count_Down_Text.text = value.ToString();
        Invoke(nameof(Count_Down_Obj_HIde), 3.2f);
    }

    private void Count_Down_Obj_Active(bool active)
    {
        Count_Down_Obj.SetActive(active);
    }

    private void Count_Down_Obj_HIde()
    {
        Count_Down_Obj.SetActive(false);
    }

    private void Hide_Enemy_Count()
    {
        enemy_Count_Obj.SetActive(false);
    }

    private void End_Boss_Round()
    {
        enemy_Count_Obj.SetActive(true);
        enemy_Count_Text.text = $"00/00";
        timer_Text.text = $"00:00";
    }

    private void End_Game_Panels()
    {
        if (end_Game_Panel == null) return;

        int cleared_Round = Round_Manager.Current_Round;
        float play_Time = 0f;

        if (Round_Manager.Instance != null)
            play_Time = Round_Manager.Instance.Game_Play_Time;

        Sprite player_Sprite = Get_Current_Player_Sprite();

        End_Game_Result_Data data = End_Game_Result_Calculator.Calculate(
            cleared_Round,
            play_Time,
            player_Sprite
        );

        Set_Character_Name(data);

        data.material_Rewards = Material_Reward_Filter.Filter_By_Inventory_Capacity(data.material_Rewards);

        Apply_Rewards(data);
        end_Game_Panel.Show(data);
    }

    private void Apply_Rewards(End_Game_Result_Data data)
    {
        if (Base_Manager.Data_Mng != null)
        {
            Base_Manager.Data_Mng.Add_Gold(data.gold_Reward);
        }

        if (Base_Manager.Character_Mng != null)
        {
            string equippedName = Base_Manager.Character_Mng.Equipped_Character_Name;

            if (!string.IsNullOrEmpty(equippedName))
            {
                Base_Manager.Character_Mng.Add_Character_Exp(equippedName, data.exp_Reward);
            }
        }

        Apply_Material_Rewards(data.material_Rewards);
    }
    private void Apply_Material_Rewards(System.Collections.Generic.List<Material_Reward_Data> rewards)
    {
        if (rewards == null || rewards.Count == 0) return;
        if (Base_Manager.Inventory_Mng == null) return;
        if (Base_Manager.Inventory_Mng.inventory_Logic == null) return;

        for (int i = 0; i < rewards.Count; i++)
        {
            Material_Reward_Data reward = rewards[i];

            if (reward == null || reward.item == null) continue;
            if (reward.amount <= 0) continue;

            Base_Manager.Inventory_Mng.inventory_Logic.Get_Item(reward.item, reward.amount);
        }
    }
    private Sprite Get_Current_Player_Sprite()
    {
        if (Base_Manager.Character_Mng == null) return null;
        if (Base_Manager.Character_Mng.current_Character == null) return null;

        Sprite sprite = Utils.Get_Character_Atlas(Base_Manager.Character_Mng.Equipped_Character_Name);
        
        return sprite;
    }

    private string Get_Equipped_Character_Display_Name()
    {
        if (Base_Manager.Character_Mng == null)
            return string.Empty;

        string equipped_Name = Base_Manager.Character_Mng.Equipped_Character_Name;

        if (string.IsNullOrEmpty(equipped_Name))
            return string.Empty;

        if (!Base_Manager.Data_Mng.d_Character_Data.ContainsKey(equipped_Name))
            return string.Empty;

        return Base_Manager.Data_Mng.d_Character_Data[equipped_Name].Character_Name;
    }

    private void Set_Character_Name(End_Game_Result_Data data)
    {
        if (data == null) return;

        data.character_Name = Get_Equipped_Character_Display_Name();
    }

    public override bool Get_Setting_UI()
    {
        bool handled = base.Get_Setting_UI();

        if (handled)
            return true;

        UI_Base ui = Base_Manager.UI_Mng.Get_UI(Pool_ID.UI_Setting, Canvas_Layer.Laver_03);

        if (ui != null)
        {
            UI_Setting setting = ui.GetComponent<UI_Setting>();

            if (setting != null)
            {
                setting.On_Give_Up_Button(true);
            }
        }

        Time.timeScale = 0f;
        return true;
    }
}