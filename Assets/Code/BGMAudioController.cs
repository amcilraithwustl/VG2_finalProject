using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMAudioController : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip clip; //make sure you assign an actual clip here in the inspector

    void Start()
    {
    }

    public void PlayBgmAudio()
    {
        // print("playCheerAudio");
        AudioSource.PlayClipAtPoint(clip, new Vector3(0,0,0));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
