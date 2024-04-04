using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcornBulletScript : MonoBehaviour
{
    [SerializeField] GameObject target; //location of the target
    [SerializeField] float speed = 2; //speed of the acorn bullet
    [SerializeField] int desTime = 2; //how long before the acorn disappears
    Rigidbody2D acornRB; //acorn's rigidbody
    
    // Start is called before the first frame update
    void Start()
    {
        acornRB = GetComponent<Rigidbody2D>(); //gets rigidbody component
        target = GameObject.Find("Player"); //gets reference to player


        Vector2 movement = (target.transform.position - transform.position).normalized * speed; //calculates direction
        acornRB.velocity = new Vector2(movement.x, movement.y); //moves acorn in direction
        Destroy(this.gameObject, desTime); //destroys the acorn
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
