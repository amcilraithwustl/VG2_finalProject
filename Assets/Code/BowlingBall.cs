using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    AudioSource audioData;

    //public Rigidbody rb;
    private bool hasReleased = false;
    //public bool collided = false;
    private GameController s;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
        s = GameObject.FindObjectOfType(typeof(GameController)) as GameController;
        //testing to make sure it rolls
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = new Vector3 ( 2, 0, 0 );
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision  other)
    {
        if (other.gameObject.CompareTag("alley"))
        {
            //print("audioData.Play(0)");
            audioData.Play(0);
        }
        
    }
  

     private void OnTriggerEnter(Collider other)
     {
        // print("TIRGGER");
         if (!hasReleased && other.gameObject.CompareTag("scorezone")) {
             print("object entered the score zone, calling WaitForThrow");
             s.WaitForThrow(this.gameObject);
             hasReleased = true;
         }
     }
}
