using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class OnlySpawner : MonoBehaviour
{
    public GameObject[] spawnPrefabs;
    public Vector3 fixedScale = new Vector3(0.2f, 0.2f, 0.2f);
    public Button[] ObjectButtons;

    private int selectedIndex = 0;

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedObject;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        // Button Selection Setup
        for (int i = 0; i < ObjectButtons.Length; i++)
        {
            int index = i;

            ObjectButtons[i].onClick.AddListener(() =>
            {
                SelectButton(index);
            });
        }
    }

    void Update()
    {
        //  Only allow single finger tap
        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);

        //  Only first frame of touch
        if (touch.phase != TouchPhase.Began)
            return;

        Ray ray = Camera.main.ScreenPointToRay(touch.position);

        //  If touching existing object → don't spawn
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.GetComponent<ARObjectManipulator>() != null)
            {
                return;
            }
        }

        // If touching plane → spawn
        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            // Destroy previous object
            if (spawnedObject != null)
            {
                Destroy(spawnedObject);
            }

            // Spawn new object
            spawnedObject = Instantiate(
                spawnPrefabs[selectedIndex],
                hitPose.position,
                Quaternion.identity
            );

            spawnedObject.transform.localScale = fixedScale;

            // Add manipulation script if not present
            if (spawnedObject.GetComponent<ARObjectManipulator>() == null)
            {
                spawnedObject.AddComponent<ARObjectManipulator>();
            }
        }
    }

    public void ResetScene()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }

    void SelectButton(int index)
    {
        selectedIndex = index;

        for (int i = 0; i < ObjectButtons.Length; i++)
        {
            ColorBlock colors = ObjectButtons[i].colors;

            if (i == index)
            {
                colors.normalColor = Color.cyan;
            }
            else
            {
                colors.normalColor = Color.white;
            }

            ObjectButtons[i].colors = colors;
        }
    }
}
