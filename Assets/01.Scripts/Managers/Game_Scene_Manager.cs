using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class Game_Scene_Manager : MonoBehaviour
{
    public static Game_Scene_Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void LoadSceneByName(string scene_Name)
    {
        if (!Is_Valid_Scene_Name(scene_Name))
        {
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(scene_Name))
        {
            Debug.LogWarning($"[GameSceneManager] LoadSceneByName 실패: Build Settings에 없는 씬입니다. sceneName={scene_Name}");
            return;
        }

        SceneManager.LoadScene(scene_Name);
    }

    public void Reload_Current_Scene()
    {
        Scene active_Scene = SceneManager.GetActiveScene();
        if (!active_Scene.IsValid())
        {
            Debug.LogWarning("[GameSceneManager] ReloadCurrentScene 실패: 현재 활성 씬이 유효하지 않습니다.");
            return;
        }

        LoadSceneByName(active_Scene.name);
    }

    public AsyncOperation Load_Scene_Async_By_Name(string scene_Name, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        if (!Is_Valid_Scene_Name(scene_Name))
        {
            return null;
        }

        if (!Application.CanStreamedLevelBeLoaded(scene_Name))
        {
            Debug.LogWarning($"[GameSceneManager] LoadSceneAsyncByName 실패: Build Settings에 없는 씬입니다. sceneName={scene_Name}");
            return null;
        }

        return SceneManager.LoadSceneAsync(scene_Name, loadSceneMode);
    }

    private bool Is_Valid_Scene_Name(string scene_Name)
    {
        if (string.IsNullOrWhiteSpace(scene_Name))
        {
            Debug.LogWarning("[GameSceneManager] sceneName이 비어 있습니다.");
            return false;
        }

        return true;
    }
}
