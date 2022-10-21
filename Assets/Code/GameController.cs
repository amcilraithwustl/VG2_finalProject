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

    public int currentTier { get; private set; }

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
        Debug.Log("currentTier" + tier + " " + objs.Length);
        currentTier = tier;
    }

    public int getShots()
    {
        return shots;
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
            _textMeshPro.text = "SingleGameScore" + getSingleGameScore()  + " shots left: " + shots;
        }
    }


   

    public int getSingleGameScore()
    {
        return numPinsFallen();
    }

    public int getTotalScore()
    {
        return totalScore + getSingleGameScore();
    }
    public void WaitForThrow(GameObject ball)
    {
        --shots;
        StartCoroutine(TidyUpGame(ball));
    }
    public IEnumerator TidyUpGame(GameObject ball)
    {
        yield return new WaitForSeconds(timeout);

        //first destroy ball
        destroyThrown(ball);
        //get current score
        getSingleGameScore();
        
        //get current totalScore
        totalScore = getTotalScore();
       
        //update scoreboard
        updateScore();
        
        print("#pins" + pins.Count);
        //conditionally reset pins (or destroy old ones)
        if (shots == 0 || pins.Count == numPinsFallen())
        {
            resetPins();
            shots = 2;//TODO: Remove magic number
        }
        else
        {
            removeDownPins();
        }
       // removeDownPins();
       // resetPins();

        //Update current tier
        updateTier();
    }

    private void Start()
    {
        pins = new List<GameObject>();
        resetPins();
        updateTier();

        _textMeshPro.text = "Welcome" + " shots left: " + shots;
    }

    void Update()
    {
        //Debug.Log(score);
        //_textMeshPro.text = "score: " + score;
    }
}