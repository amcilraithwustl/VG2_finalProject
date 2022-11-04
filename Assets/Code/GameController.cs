using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine.ProBuilder;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public GameObject pinPrefab;
    [NotNull] public GameObject pinPosition;
    
    public TMP_Text _textMeshPro;
    public float timeout = 3.0f;

    //TODO: outdated. Do during update?
    private List<GameObject> pins = new();
    private int totalScore = 0;
    public int shots = 2;
    private int currentShot;

    //Recording the scores
    public List<List<int>> record;
    public int currentRound = 0;
    public int currentTier { get; private set; }
    GlobalAudioController globalAudioController;

    //Methods
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void updateTier()
    {
        //Get all game objects with bowlable
        var objs = FindObjectsOfType<Bowlable>();
        //set tier to lowest current bowlable value
        int tier = Int32.MaxValue;
        foreach (var bowlable in objs)
        {
            if (bowlable.hasTier && tier > bowlable.tier)
            {
                
                tier = bowlable.tier;
            }
        }

        //Alternately, we could use trigger "setGrabbable" on all relevant objects.
        //However, this could make for more confusing code to maintain, albeit with better performance.
       // Debug.Log("currentTier" + tier + " " + objs.Length);
        currentTier = tier;
    }

    void resetPins()
    {
        foreach (var t in pins)
        {
            Destroy(t);
        }

        pins.Clear();

        List<Transform> childPositions = new();
        for (int i = 0; i < pinPosition.transform.childCount; i++)
        {
            childPositions.Add(pinPosition.transform.GetChild(i));
        }
        print("RESETTING PINS" + childPositions.Count);
        foreach (var t in childPositions)
        {
            pins.Add(Instantiate(pinPrefab, t));
        }
        
        
    }

    bool isStandingUp(GameObject p){
        
        if (p.transform.rotation.x > -0.1 
            && p.transform.rotation.x < 0.1 
            && p.transform.rotation.z < 0.1
            && p.transform.rotation.z > -0.1)
        {
            return true;
        }
        
        return false;
    }

    

    public int numPinsFallen()
    {
        return pins.Count(t => !isStandingUp(t));
    }
    
    void removeDownPins()
    {
        for (int i = pins.Count - 1; i >= 0; i--)
        {
            if (!isStandingUp(pins[i]))
            {
                print("DESTROYING PIN" + i);
                Destroy(pins[i]);
                pins.RemoveAt(i);
                //i--;
            }
        }
    }
    
    void destroyThrown(GameObject ball)
    {
        print("destroyThrown: Destroy the ball");
        Destroy(ball);
    }
    void updateScore()
    {
        if (shots <= 0)
        {
            _textMeshPro.text = "score: " + totalScore;
        }
        else
        {
            _textMeshPro.text = "SingleGameScore"   + " shots left: " + shots;
        }
    }

    public void recordScore()
    {
        
        
    }
    
    public void WaitForThrow(GameObject ball)
    {
        --currentShot;
        StartCoroutine(TidyUpGame(ball));
    }
    public IEnumerator TidyUpGame(GameObject ball)
    {
        yield return new WaitForSeconds(timeout);

        //first destroy ball
        destroyThrown(ball);
       

        //update scoreboard
        updateScore();
        
        //play cheer audio
        globalAudioController.playCheerAudio();
        
        print("SingleGameScore: " + numPinsFallen());
        // Strike
        if (numPinsFallen() == 10)
        {
            resetPins();
            currentShot++;
            //totalScore *= 10;
        } 
        // Spare
        else if (pins.Count == numPinsFallen())
        {
            resetPins();
            totalScore += numPinsFallen();
            //totalScore *= 2;
        } 
        else
        {
            removeDownPins();
            totalScore += numPinsFallen();
        }
        
        // One Round end
        if (currentShot == 0)
        {
            currentShot = shots;
            resetPins();
        }

        recordScore();

    }

    private void Start()
    {
        currentShot = shots;
        globalAudioController = FindObjectOfType(typeof(GlobalAudioController)) as GlobalAudioController;
        pins = new List<GameObject>();
        resetPins();
        updateTier();
        globalAudioController.playCheerAudio();

        //_textMeshPro.text = "Welcome" + " shots left: " + shots;
    }

    void Update()
    {
        updateTier();

        //Debug.Log(score);
        //_textMeshPro.text = "score: " + score;
    }
}