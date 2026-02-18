using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ReturnToMenuManager : MonoBehaviour
{
 
    private VisualElement root;
    private Button returnButton;

    void Start()
    {
        UIDocument doc=GetComponent<UIDocument>();
        root = doc.rootVisualElement;
        
        returnButton = root.Q<Button>("returnButton");

        returnButton.clickable.clicked += () =>
        {
            SceneManager.LoadScene("CombineScene");
        };
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CombineScene")
        {
            root.style.display = DisplayStyle.None;
        }
        else
        {
            root.style.display = DisplayStyle.Flex;
        }
    }

    private void Awake()
    {
        
        DontDestroyOnLoad(gameObject);
    }
}