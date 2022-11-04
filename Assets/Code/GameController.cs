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
using UnityEngine.Serialization;
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
    [FormerlySerializedAs("shots")] public int totalShots = 2;
    private int currentShot;

    //Recording the scores
    public List<List<int>> record;
    private List<int> currentRound;
    public int totalRound = 10;
    
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
    void updateDisplay(String s)
    {
        _textMeshPro.text = s;
       
    }

    public void recordScore()
    {
        if (currentShot > 0) {
            currentRound.Add(numPinsFallen());
        }
        else {
            currentRound.Add(numPinsFallen());
            record.Add(currentRound);
            currentRound = new List<int>();
        }
        
    }

    private void recalculateRecord() {
        List<List<int>> newRecord = new List<List<int>>();
        for (int i = 0; i < record.Count; i++) {
            if (record[i][0] == 10) {
                int temp = 10;

                if (record[i + 1][0] == 10 && i + 2 < record.Count) {
                    temp += record[i + 1][0] + record[i + 2][0];
                }
                else if (record[i + 1][0] == 10 && i + 1 < record.Count) {
                    temp += 10;
                }
                else if (i + 1 < record.Count) {
                    temp += record[i + 1][0] + record[i + 1][1];
                }
                newRecord[i][0] = temp;
                newRecord[i][1] = 0;

            } else if (record[i][0] + record[i][0] == 10) {
                newRecord[i][0] = record[i][0];
                int temp = record[i][1];
                if (i + 1 < record.Count) {
                    temp += record[i + 1][0];
                }
                newRecord[i][1] = temp;
            }
            else {
                newRecord[i][0] = record[i][0];
                newRecord[i][1] = record[i][1];
            }
        }
        record = newRecord;
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
       

        //update tv
        updateDisplay("SingleGameScore: " + numPinsFallen());
        print("SingleGameScore: " + numPinsFallen());

        
        //play cheer audio
        globalAudioController.playCheerAudio();
        
        // Strike
        if (numPinsFallen() == 10) {
            currentShot = 0;
            resetPins();
            
        } 
        // Spare
        else if (pins.Count == numPinsFallen())
        {
            resetPins();
            //totalScore += numPinsFallen();
            //totalScore *= 2;
        } 
        else
        {
            removeDownPins();
            //totalScore += numPinsFallen();
        }
        
        //record
        recordScore();
        recalculateRecord();
        print(record);
        
        // One Round end
        if (currentShot == 0)
        {
            currentShot = totalShots;
        }

        

    }

    private void Start()
    {
        currentShot = totalShots;
        currentRound = new List<int>();
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