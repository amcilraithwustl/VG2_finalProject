using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    //public Rigidbody rb;
    private bool hasReleased = false;
    //public bool collided = false;
    private GameController s;

    void Start()
    {
        s = GameObject.FindObjectOfType(typeof(GameController)) as GameController;
        //testing to make sure it rolls
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = new Vector3 ( 2, 0, 0 );
    }

    // Update is called once per frame
    
  

     private void OnTriggerEnter(Collider other)
     {
        // print("TIRGGER");
         if (!hasReleased && other.gameObject.CompareTag("scorezone")) {
             print("TAKING THE SHOT");
             s.WaitForThrow(this.gameObject);
             hasReleased = true;
         }
     }
}
