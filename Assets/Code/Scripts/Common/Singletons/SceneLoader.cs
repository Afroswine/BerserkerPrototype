using UnityEngine;
using UnityEngine.SceneManagement;

// singleton class
// Used to load and reset scenes
public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [Header("SceneLoader")]
    [SerializeField] string _resetSceneName = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        if (_resetSceneName == null || _resetSceneName == "")
        {
            _resetSceneName = GetActiveSceneName();
            Debug.Log("SceneLoader: No Reset Scene name given. Reset scene has been defaulted to the active scene.");
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public string GetActiveSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
