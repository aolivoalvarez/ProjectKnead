// SCRIPT TAKEN FROM A VIDEO BY EXPAT STUDIOS
using UnityEngine;

public class DungeonCameraController : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 0.5f;
    public Vector3 minPos, maxPos;
    Vector3 targetPos, newPos;

    void LateUpdate()
    {
        if (transform.position != PlayerController.instance.transform.position)
        {
            targetPos = PlayerController.instance.transform.position;

            Vector3 camBoundaryPos = new Vector3(
                Mathf.Clamp(targetPos.x, minPos.x, maxPos.x),
                Mathf.Clamp(targetPos.y, minPos.y, maxPos.y),
                Mathf.Clamp(targetPos.z, minPos.z, maxPos.z));

            newPos = Vector3.Lerp(transform.position, camBoundaryPos, smoothSpeed);
            transform.position = newPos;
        }
    }
}
