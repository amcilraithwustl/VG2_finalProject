using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    //public Rigidbody rb;
    private bool hasReleased = false;
    //public bool collided = false;
    private Score s;

    void Start()
    {
        s = GameObject.FindObjectOfType(typeof(Score)) as Score;
        //testing to make sure it rolls
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = new Vector3 ( 2, 0, 0 );
    }

    // Update is called once per frame
    
     void OnCollisionEnter(Collision other) {

        if (other.gameObject.tag == "pins") {
            s.takeOneShot();

        }
     }

}
