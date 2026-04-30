using UnityEngine;

public class UI_Base : MonoBehaviour
{
    [SerializeField]
    private Pool_ID pool_ID;

    public Pool_ID Pool_ID => pool_ID;

    private void Start()
    {
        Init();
    }
    protected virtual void Init()
    {
    }
    public virtual void Close_UI()
    {
        Base_Manager.UI_Mng.Close_Top_UI();
    }
    public void Close_Sound()
    {
        Base_Manager.Sound_Mng.Play_SFX(SFX_Type.Button_Click);
    }
}
