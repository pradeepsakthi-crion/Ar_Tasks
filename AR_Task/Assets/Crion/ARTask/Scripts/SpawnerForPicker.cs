using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UIElements;
using UnityEngine.UI;  

public class SpawnerForPicker : MonoBehaviour
{
    [Header("Prefab Settings")]
    public GameObject[] prefabs;
    public Vector3 fixedScale = new Vector3(0.2f, 0.2f, 0.2f);

    private int selectedPrefabIndex = 0;

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject spawnedObject;
    
    // UI TOOLKIT (Rotation)
    private VisualElement rotationPanel;
    private UnityEngine.UIElements.Slider rotationSliderUI;
    private float currentYRotation = 0f;

   
    // NORMAL UI (Scale)
    public UnityEngine.UI.Slider scaleSlider;   

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        
        // UI TOOLKIT SETUP
        UIDocument uiDoc = FindFirstObjectByType<UIDocument>();
        VisualElement root = uiDoc.rootVisualElement;

        rotationPanel = root.Q<VisualElement>("rotationPanel");

        if (rotationPanel != null)
            rotationPanel.style.display = DisplayStyle.None;  // Hide initially

        rotationSliderUI = root.Q<UnityEngine.UIElements.Slider>("rotationSlider");

        if (rotationSliderUI != null)
        {
            rotationSliderUI.RegisterValueChangedCallback(evt =>
            {
                currentYRotation = evt.newValue;

                if (spawnedObject != null)
                {
                    spawnedObject.transform.rotation =
                        Quaternion.Euler(0, currentYRotation, 0);
                }
            });
        }
        
        // NORMAL UI SCALE SLIDE
        if (scaleSlider != null)
        {
            scaleSlider.gameObject.SetActive(false);  // Hide initially

            scaleSlider.onValueChanged.AddListener((value) =>
            {
                if (spawnedObject != null)
                {
                    spawnedObject.transform.localScale =
                        new Vector3(value, value, value);
                }
            });
        }
        
        // PREFAB BUTTONS (Auto Detect)
        var prefabButtons = root.Query<UnityEngine.UIElements.Button>(className: "prefab-button").ToList();

        for (int i = 0; i < prefabButtons.Count; i++)
        {
            int index = i;
            prefabButtons[i].clicked += () => SelectPrefab(index);
        }
    }

    void SelectPrefab(int index)
    {
        if (index < 0 || index >= prefabs.Length)
            return;

        selectedPrefabIndex = index;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    // Destroy previous object (single object mode)
                    if (spawnedObject != null)
                        Destroy(spawnedObject);

                    spawnedObject = Instantiate(
                        prefabs[selectedPrefabIndex],
                        hitPose.position,
                        Quaternion.identity
                    );

                    spawnedObject.transform.localScale = fixedScale;

                    //  Reset Scale Slider Value
                    if (scaleSlider != null)
                        scaleSlider.value = fixedScale.x;

                    //  Show Rotation Panel
                    if (rotationPanel != null)
                        rotationPanel.style.display = DisplayStyle.Flex;

                    //  Show Scale Slider
                    if (scaleSlider != null)
                        scaleSlider.gameObject.SetActive(true);
                }
            }
        }
    }
}
