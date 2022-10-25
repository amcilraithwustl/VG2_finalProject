using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinAudio : MonoBehaviour
{
    AudioSource audioData;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision  other)
    {
        if (other.gameObject.CompareTag("grabbable"))
        {
            print("audioData.Play(0)");
            audioData.Play(0);
        }
        
    }
}
