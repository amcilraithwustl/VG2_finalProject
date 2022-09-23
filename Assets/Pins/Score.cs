using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour {
    public GameObject[] pins;
    private bool[] states = new bool[10];
    private int score = 0;
    public bool collided = false;
    public TMP_Text _textMeshPro;

    void Update() {
        for (int i = 0; i < 10; i++) {
            GameObject pin = pins[i];
            if (pin.transform.rotation.x > -0.01 
                && pin.transform.rotation.x < 0.01 
                && pin.transform.rotation.z < 0.01
                && pin.transform.rotation.z > -0.01) {
                //pin is standing up (do nothing)
                states[i] = false;
            }
            else {
                //pin is knocked over
                states[i] = true;
            }
        }
        score = 0;
        for (int i = 0; i < 10; i++) {
            //Debug.Log(i + "&&&&" + states[i]);

            if (states[i]) {
                score++;
            }
        }
        Debug.Log(score);
        _textMeshPro.text = "score: " + score;

    }
}