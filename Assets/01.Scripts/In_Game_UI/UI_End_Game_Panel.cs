using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_End_Game_Panel : MonoBehaviour
{
    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI score_Text;
    [SerializeField] 
    private TextMeshProUGUI playTime_Text;
    [SerializeField] 
    private TextMeshProUGUI finalRound_Text;
    [SerializeField]
    private TextMeshProUGUI gold_Text;
    [SerializeField] 
    private TextMeshProUGUI exp_Text;
    [SerializeField] 
    private TextMeshProUGUI character_Name_Text;

    [Space(20)]
    [Header("Player Image")]
    [SerializeField] private Image player_Image;

    [Space(20)]
    [Header("Item Reward")]
    [SerializeField] private Transform reward_Content;
    [SerializeField] private Pool_ID reward_Item_Slot_Pool_ID;

    [Space(20)]
    [Header("Count Up")]
    [SerializeField] private float countUp_Duration = 1.2f;

    private Coroutine score_Coroutine;
    private Coroutine gold_Coroutine;
    private Coroutine exp_Coroutine;

    private readonly List<GameObject> spawned_Reward_Slots = new List<GameObject>();

    public void Show(End_Game_Result_Data data)
    {
        gameObject.SetActive(true);

        Set_Static_Data(data);
        Set_Player_Image(data.player_Sprite);
        Set_Character_Name(data.character_Name);
        Refresh_Reward_Items(data.material_Rewards);

        Start_Count_Up(ref score_Coroutine, score_Text, data.score, " Á¡");
        Start_Count_Up(ref gold_Coroutine, gold_Text, data.gold_Reward, " G");
        Start_Count_Up(ref exp_Coroutine, exp_Text, data.exp_Reward, " XP");
    }

    private void Set_Static_Data(End_Game_Result_Data data)
    {
        if (finalRound_Text != null)
            finalRound_Text.text = $"{data.cleared_Round:00}";

        if (playTime_Text != null)
            playTime_Text.text = Format_Time(data.play_Time);
        
        if (score_Text != null)
            score_Text.text = "0 Á¡";

        if (gold_Text != null)
            gold_Text.text = "0 G";

        if (exp_Text != null)
            exp_Text.text = "0 XP";
    }

    private void Set_Player_Image(Sprite sprite)
    {
        if (player_Image == null) return;

        player_Image.sprite = sprite;
        player_Image.enabled = sprite != null;
    }

    private void Set_Character_Name(string name)
    {
        if (character_Name_Text == null) return;
        character_Name_Text.text = string.IsNullOrEmpty(name) ? "None" : name;
    }

    private void Refresh_Reward_Items(List<Material_Reward_Data> rewards)
    {
        Clear_Reward_Items();

        if (reward_Content == null) return;
        if (rewards == null || rewards.Count == 0) return;

        for (int i = 0; i < rewards.Count; i++)
        {
            Material_Reward_Data reward = rewards[i];

            Base_Manager.Pool_Mng.Pooling_OBJ(reward_Item_Slot_Pool_ID).Get(obj =>
            {
                obj.transform.SetParent(reward_Content, false);

                UI_End_Game_Item_Slot slot = obj.GetComponent<UI_End_Game_Item_Slot>();
                if (slot != null)
                {
                    slot.Init(reward);
                }

                spawned_Reward_Slots.Add(obj);
            });
        }
    }

    private void Clear_Reward_Items()
    {
        for (int i = 0; i < spawned_Reward_Slots.Count; i++)
        {
            if (spawned_Reward_Slots[i] != null)
            {
                Base_Manager.Pool_Mng.pool_Dictionary[reward_Item_Slot_Pool_ID].Return(spawned_Reward_Slots[i]);
            }
        }

        spawned_Reward_Slots.Clear();
    }

    private void Start_Count_Up(ref Coroutine coroutine, TextMeshProUGUI targetText, int endValue, string suffix)
    {
        if (targetText == null) return;

        if (coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(Count_Up_Coroutine(targetText, endValue, suffix));
    }

    private IEnumerator Count_Up_Coroutine(TextMeshProUGUI targetText, int endValue, string suffix)
    {
        float elapsed = 0f;
        int startValue = 0;

        while (elapsed < countUp_Duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / countUp_Duration);
            t = 1f - Mathf.Pow(1f - t, 3f);

            int value = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, t));
            targetText.text = $"{value:N0}{suffix}";

            yield return null;
        }

        targetText.text = $"{endValue:N0}{suffix}";
    }

    private string Format_Time(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    private void OnDisable()
    {
        Clear_Reward_Items();
    }
}