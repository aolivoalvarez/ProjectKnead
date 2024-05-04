/*-----------------------------------------
Creation Date: 5/1/2024 8:29:16 PM
Author: theco
Description: Controls animation for title screen clouds.
-----------------------------------------*/

using UnityEngine;

public class TS_CloudsScript : MonoBehaviour
{
    [SerializeField] float foregroundSpeed = 1f;
    [SerializeField] float backgroundSpeed = 0.3f;
    [SerializeField] bool smallCloud = false;
    float rightSide;
    float leftSide;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("SmallCloud", smallCloud);
        rightSide = Screen.width + (GetComponent<RectTransform>().rect.width * 0.6f);
        leftSide = 0f - (GetComponent<RectTransform>().rect.width * 0.6f);
        //transform.position = new Vector3(rightSide, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (smallCloud) transform.position = new Vector3(transform.position.x - backgroundSpeed * 60 * Time.deltaTime, transform.position.y, transform.position.z);
        else transform.position = new Vector3(transform.position.x - foregroundSpeed * 60 * Time.deltaTime, transform.position.y, transform.position.z);

        if (transform.position.x <= leftSide) 
        {
            transform.position = new Vector3(rightSide, transform.position.y, transform.position.z);
        }
    }
}
