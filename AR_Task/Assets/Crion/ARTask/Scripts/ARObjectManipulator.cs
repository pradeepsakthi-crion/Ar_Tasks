using UnityEngine;

public class ARObjectManipulator : MonoBehaviour
{
    public static ARObjectManipulator SelectedObject;

    private Vector3 offset;
    private Camera arCamera;

    private float initialDistance;
    private Vector3 initialScale;

    private float initialAngle;
    private float currentYRotation;

    void Start()
    {
        arCamera = Camera.main;
    }

    void Update()
    {
        // Only allow interaction if this object is selected
        if (SelectedObject != this)
            return;

        // -------- SINGLE FINGER DRAG --------
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                Plane plane = new Plane(Vector3.up, transform.position);

                if (plane.Raycast(ray, out float distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    transform.position = hitPoint;
                }
            }
        }

        // -------- TWO FINGER PINCH + ROTATE --------
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(t0.position, t1.position);

            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                initialDistance = currentDistance;
                initialScale = transform.localScale;

                initialAngle = GetAngle(t0.position, t1.position);
            }
            else
            {
                // ---- SCALE ----
                float scaleFactor = currentDistance / initialDistance;
                transform.localScale = initialScale * scaleFactor;

                // ---- ROTATE ----
                float currentAngle = GetAngle(t0.position, t1.position);
                float angleDelta = currentAngle - initialAngle;

                currentYRotation += angleDelta;
                transform.rotation = Quaternion.Euler(0, currentYRotation, 0);

                initialAngle = currentAngle;
            }
        }
    }

    float GetAngle(Vector2 p1, Vector2 p2)
    {
        Vector2 dir = p2 - p1;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }

    // Call this when tapping object
    void OnMouseDown()
    {
        SelectedObject = this;
    }
}
