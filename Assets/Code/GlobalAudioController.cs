using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip clip; //make sure you assign an actual clip here in the inspector

    void Start()
    {
    }

    public void playCheerAudio()
    {
        print("playCheerAudio");
        AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
