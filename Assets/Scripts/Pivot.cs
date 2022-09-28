using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Composites;

public class Pivot : MonoBehaviour
{
    private Transform _rigidbody;
    private float _rotationDirection = 0;
    public float rotationSpeed = 10;
    void Start()
    {
        _rigidbody = GetComponent<Transform>();
        
    }

    public void FixedUpdate()
    {
        Debug.Log("DING " + (_rotationDirection * rotationSpeed));
        _rigidbody.Rotate(new Vector3(0, 0, 1), _rotationDirection * rotationSpeed);
        // _rigidbody.Rotate(new Vector3(0, 0, 1), 1);

    }

    public void Fire(InputAction.CallbackContext ctx)
    {
        _rotationDirection = ctx.ReadValue<float>();
    
    }
    
    
}
