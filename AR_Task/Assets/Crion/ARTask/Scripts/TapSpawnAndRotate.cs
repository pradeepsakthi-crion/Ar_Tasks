using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UIElements;

public class TapSpawnAndRotate : MonoBehaviour
{
    public GameObject spawnPrefab;
    public Vector3 fixedScale = new Vector3(0.2f, 0.2f, 0.2f);

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject spawnedObject;

    // UI Toolkit
    private Slider rotationSlider;
    private float currentYRotation = 0f;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        //  UI Toolkit hookup
        UIDocument uiDoc = FindFirstObjectByType<UIDocument>();
        
        VisualElement root = uiDoc.rootVisualElement;

        rotationSlider = root.Q<Slider>("rotationSlider");

        if (rotationSlider == null)
        {
            Debug.LogError("Slider with name 'rotationSlider' not found");
            return;
        }

        rotationSlider.RegisterValueChangedCallback(evt =>
        {
            currentYRotation = evt.newValue;
        });
    }

    void Update()
    {
        // -------- SPAWN --------
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    if (spawnedObject != null)
                        Destroy(spawnedObject);

                    spawnedObject = Instantiate(
                        spawnPrefab,
                        hitPose.position,
                        Quaternion.identity
                    );

                    spawnedObject.transform.localScale = fixedScale;
                }
            }
        }

        // -------- ROTATION --------
        if (spawnedObject != null)
        {
            spawnedObject.transform.rotation =
                Quaternion.Euler(0, currentYRotation, 0);
        }
    }
}
