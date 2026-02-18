using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ReturnToMenuManager : MonoBehaviour
{
    private VisualElement root;
    private Button returnButton;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UIDocument doc = GetComponent<UIDocument>();
        root = doc.rootVisualElement;

        returnButton = root.Q<Button>("returnButton");

        returnButton.clicked += () =>
        {
            SceneManager.LoadScene("CombineScene");
        };

        // Listen for scene change
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Apply visibility immediately
        UpdateVisibility(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVisibility(scene.name);
    }

    void UpdateVisibility(string sceneName)
    {
        if (sceneName == "CombineScene")
        {
            root.style.display = DisplayStyle.None; // Hide button
        }
        else
        {
            root.style.display = DisplayStyle.Flex; // Show button
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}