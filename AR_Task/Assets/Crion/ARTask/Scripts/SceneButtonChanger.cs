using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SceneButtonChanger : MonoBehaviour
{
    private VisualElement root;
    private Button sceneButton1;
    private Button sceneButton2;
    private Button sceneButton3;
    private Button sceneButton4;
    private Button quitButton;
    
    void Start()
    {
        
        UIDocument doc=GetComponent<UIDocument>();
        root = doc.rootVisualElement;
        sceneButton1 = root.Q<Button>("Scene_1Button");
        sceneButton2 = root.Q<Button>("Scene_2Button");
        sceneButton3 = root.Q<Button>("Scene_3Button");
        sceneButton4 = root.Q<Button>("Scene_4Button");
        quitButton = root.Q<Button>("QuitButton");

        sceneButton1.clickable.clicked += () => { SceneManager.LoadScene("HelloAR"); };
        sceneButton2.clickable.clicked += () => { SceneManager.LoadScene("ARPlacement"); };
        sceneButton3.clickable.clicked += () => { SceneManager.LoadScene("ARIntractScene"); };
        sceneButton4.clickable.clicked += () => { SceneManager.LoadScene("ARObjectPickerScene"); };
        
        quitButton.clickable.clicked += () =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else            
            Application.Quit();
#endif            

        };
        
        
    }
}
