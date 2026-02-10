using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;   //  IMPORTANT

public class TapSpawnAndRotate : MonoBehaviour
{
    public GameObject spawnPrefab;
    public Vector3 fixedScale = new Vector3(0.2f, 0.2f, 0.2f);

    public Slider rotationSlider;   //  SLIDER REFERENCE

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedObject;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
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

        // -------- ROTATION (SAFE WAY) --------
        if (spawnedObject != null && rotationSlider != null)
        {
            float y = rotationSlider.value;
            spawnedObject.transform.rotation = Quaternion.Euler(0, y, 0);
        }
    }
}