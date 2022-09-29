using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    //public Rigidbody rb;
    private bool hasReleased = false;
    void Start()
    {
        //testing to make sure it rolls
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = new Vector3 ( 2, 0, 0 );
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J) && !hasReleased)
        {
            gameObject.transform.parent = null;
            hasReleased = true;
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().velocity =new Vector3(-1,0,0) * 40;
            gameObject.GetComponent<Rigidbody>().rotation = Quaternion.Euler(100, 0, 0);
        }
    }
}
