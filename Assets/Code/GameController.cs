using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //TODO: make this a prefab rather than direct reference.
    // Maybe use transforms to mark spawn position?
    public GameObject[] pins;
    public GameObject pinPrefab;
    public Transform pinRespawnPosition;
    
    public TMP_Text _textMeshPro;
    public float timeout = 3.0f;
    
    //TODO: outdated. Do during update?
    private bool[] states = new bool[10];
    private int totalScore = 0;
    public int shots = 2;


    //public Pins p;
    
    public int getShots() {
        return shots;
    }

    void resetPins()
    {
        var newPins= Instantiate(pinPrefab,  pinRespawnPosition.position, Quaternion.identity);
        pinPrefab = newPins;
        
    }

    void updateScore()
    {
        if (shots <= 0) {
            _textMeshPro.text = "score: " + totalScore;
        }
        else {
            _textMeshPro.text = "score: " + totalScore + " shots left: " + shots;
        }
    }

    void removeDownPins()
    {
        Destroy(pinPrefab);
    }
    void destroyThrown(GameObject ball)
    {
        Destroy(ball);
    }

    public void WaitForThrow(GameObject ball) {
        --shots;
        StartCoroutine(TidyUpGame(ball));
    }

    public int numPinsRemaining()
    {
        return 0;
    }
    public int getScore()
    {
        //TODO: Remove double counting
        return totalScore + 10 - numPinsRemaining();
    }

    public IEnumerator TidyUpGame(GameObject ball)
    {
        yield return new WaitForSeconds(timeout);

        //first destroy ball
        destroyThrown(ball);
        //get current score
        totalScore = getScore();
        //update scoreboard
        updateScore();
        //conditionally reset pins (or destroy old ones)
        if (shots == 0 || numPinsRemaining() == 0)
        {
            resetPins();
        }
        else
        {
            removeDownPins();
        }
    }

    private void Start() {
        _textMeshPro.text = "Welcome" + " shots left: " + shots;
    }

    void Update() {
        
        //Debug.Log(score);
        //_textMeshPro.text = "score: " + score;

    }
}
