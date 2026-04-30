using UnityEngine;

public class Effect_Return_Delay : MonoBehaviour
{
    private Pool_ID pool_Path;

    private float timer;

    private ParticleSystem ps;

    public void Init(Pool_ID path, float delay)
    {
        pool_Path = path;

        Invoke(nameof(Return_To_Pool), delay);
    }

    private void Return_To_Pool()
    {
        Base_Manager.Pool_Mng.pool_Dictionary[pool_Path].Return(gameObject);
    }
}