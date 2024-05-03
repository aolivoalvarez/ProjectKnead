/*-----------------------------------------
Creation Date: 5/2/2024 3:07:09 PM
Author: theco
Description: Controls animation for title screen clouds (far background).
-----------------------------------------*/

using UnityEngine;

public class TS_BGCloudsScript : MonoBehaviour
{
    [SerializeField] float speed = 0.2f;
    [SerializeField] RectTransform otherCloud;

    void Update()
    {
        if (otherCloud.position.x < transform.position.x)
        {
            transform.position = new Vector3(otherCloud.position.x + Screen.width, transform.position.y, transform.position.z);
        }
        else
            transform.position = new Vector3(transform.position.x - speed * 60 * Time.deltaTime, transform.position.y, transform.position.z);

        float leftSide = -Screen.width;
        float rightSide = Screen.width;

        if (transform.position.x <= leftSide)
        {
            transform.position = new Vector3(rightSide, transform.position.y, transform.position.z);
        }
    }
}
