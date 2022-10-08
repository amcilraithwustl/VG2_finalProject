using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour {

    public GameObject[] pins;
    public GameObject ball;
    public Transform respawnPosition;
    public TMP_Text _textMeshPro;
    public float timeout = 3.0f;
        
    private bool[] states = new bool[10];
    private int score = 0;
    public int shots = 2;
    public int getShots() {
        return shots;
    }

    public int takeOneShot() {
        print("TAKING THE SHOT PART 2: " + shots);
        --shots;
        StartCoroutine(Wait());
        return shots;
    }

    public int getScore() {
        return score;
    }
    
    public IEnumerator Wait(){
        yield return new WaitForSeconds(timeout);
         

        Debug.Log("shots left " + shots);
        if (shots <= 0) {
            _textMeshPro.text = "score: " + score;

           SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else {
            _textMeshPro.text = "score: " + score + " shots left: " + shots;
            ball.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            ball.gameObject.GetComponent<Rigidbody>().useGravity = true;
            Debug.Log("respawnPosition" + respawnPosition.position);
            var newobj = Instantiate(ball,  respawnPosition.position, Quaternion.identity);
            Destroy(ball);
            ball = newobj;

        }

         
    }

    private void Start() {
        _textMeshPro.text = "Welcome" + " shots left: " + shots;
    }

    void Update() {
        for (int i = 0; i < 10; i++) {
            GameObject pin = pins[i];
            if (pin.transform.rotation.x > -0.1 
                && pin.transform.rotation.x < 0.1 
                && pin.transform.rotation.z < 0.1
                && pin.transform.rotation.z > -0.1) {
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
        //Debug.Log(score);
        //_textMeshPro.text = "score: " + score;

    }
}