using UnityEngine;

public class Scene_Sound_Player : MonoBehaviour
{
    [SerializeField] private BGM_Type bgm_Type;

    private void Start()
    {
        if (Base_Manager.Sound_Mng == null) 
        {
            Debug.Log("Base_Manager.Sound_Mng == null");
            return;
        }

        Base_Manager.Sound_Mng.Play_BGM(bgm_Type);
    }
}