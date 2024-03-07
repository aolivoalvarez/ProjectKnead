// SCRIPT TAKEN FROM A VIDEO BY EXPAT STUDIOS
using UnityEngine;

public class DungeonDoorScript : MonoBehaviour
{
    [SerializeField] Vector3 newCamPos, newPlayerPos;
    DungeonCameraController camControl;

    void Start()
    {
        camControl = Camera.main.GetComponent<DungeonCameraController>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            camControl.minPos += newCamPos;
            camControl.maxPos += newCamPos;

            other.transform.position += newPlayerPos;
            GameObject.FindGameObjectWithTag("Checkpoint").transform.position = other.transform.position;
        }
    }
}
