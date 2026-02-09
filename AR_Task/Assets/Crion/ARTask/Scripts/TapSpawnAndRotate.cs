using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TapSpawnAndRotate : MonoBehaviour
{
    public GameObject spawnPrefab;
    public Vector3 fixedScale = new Vector3(0.2f, 0.2f, 0.2f);

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private GameObject spawnedObject;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                // Old object delete
                if (spawnedObject != null)
                {
                    Destroy(spawnedObject);
                }

                // New object spawn
                spawnedObject = Instantiate(
                    spawnPrefab,
                    hitPose.position,
                    Quaternion.identity
                );

                // Fixed size
                spawnedObject.transform.localScale = fixedScale;
            }
        }
    }

    //  ROTATION 
    public void RotateY(float y)
    {
        if (spawnedObject != null)
        {
            spawnedObject.transform.rotation = Quaternion.Euler(0, y, 0);
        }
    }
}
