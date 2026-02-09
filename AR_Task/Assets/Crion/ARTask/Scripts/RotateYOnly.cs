using UnityEngine;

public class RotateYOnly : MonoBehaviour
{
    private GameObject targetObject;

    // Spawn script indha function-a call pannum
    public void SetTarget(GameObject obj)
    {
        targetObject = obj;
    }

    // Slider indha function-a call pannum
    public void RotateY(float y)
    {
        if (targetObject != null)
        {
            targetObject.transform.rotation = Quaternion.Euler(0, y, 0);
        }
    }
}