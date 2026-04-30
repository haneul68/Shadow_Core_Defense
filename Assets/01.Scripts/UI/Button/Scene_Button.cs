using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Scene_Button : MonoBehaviour
{
    [Header("TYPE")]
    [SerializeField] private Scene_Button_Type button_Type;
    [SerializeField] private string scene_Name;

    private Button button;
    private Game_Scene_Manager scene_Manager;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        scene_Manager = Game_Scene_Manager.Instance;

        if (scene_Manager == null)
        {
            scene_Manager = FindFirstObjectByType<Game_Scene_Manager>();
        }

        Bind_Event();
    }

    private void Bind_Event()
    {
        button.onClick.RemoveAllListeners();

        switch (button_Type)
        {
            case Scene_Button_Type.Load_Scene:
                button.onClick.AddListener(() =>
                {
                    Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
                    if (scene_Name == "In_Game_Scene") 
                    {
                        if (string.IsNullOrEmpty(Base_Manager.Character_Mng.Equipped_Character_Name)) 
                        {
                            Lobby_Canvas.Instance.Get_Text_Pop_Up($"장착된 캐릭터가 없습니다. 캐릭터를 장착해주세요", Color.red);
                            Debug.Log("장착된 캐릭터 없음");
                            return;
                        }
                    }
                    scene_Manager.LoadSceneByName(scene_Name);
                });
                break;

            case Scene_Button_Type.Reload_Scene:
                button.onClick.AddListener(() =>
                {
                    Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
                    scene_Manager.Reload_Current_Scene();
                });
                break;
        }
    }
}