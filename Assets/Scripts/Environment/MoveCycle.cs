using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCycle : MonoBehaviour
{

    public  Vector2 moveDirection = Vector2.right;
    public float moveSpeed = 1f;
    public int objSize = 1;

    [SerializeField] Vector3 rightEdge;
    [SerializeField] Vector3 leftEdge;
    // Start is called before the first frame update
    void Start()
    {
        leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
    }

    // Update is called once per frame
    void Update()
    {
        if(moveDirection.x > 0 &&(transform.position.x - objSize) > rightEdge.x)
        {
            Vector3 _position = transform.position;
            _position.x = leftEdge.x - objSize;
            transform.position = _position;
        }
        else if (moveDirection.x < 0 &&(transform.position.x + objSize) < leftEdge.x)
        {
            Vector3 _position = transform.position;
            _position.x = rightEdge.x + objSize;
            transform.position = _position;
        }
        else
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
        
    }
}
