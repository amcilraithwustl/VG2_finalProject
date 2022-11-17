using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour {
    public static GameController Instance { get; private set; }
    public GameObject pinPrefab;
    [NotNull] public GameObject pinPosition;

    public TMP_Text _textMeshPro;
    public TMP_Text mainMenuText;
    public float timeout = 3.0f;

    public ScorePanel scorePanel;

    //TODO: outdated. Do during update?
    private List<GameObject> pins = new();
    private int totalScore = 0;
    [FormerlySerializedAs("shots")] public int totalShots = 2;
    private int shotsLeft;

    //Recording the scores
    public List<List<int>> record = new();
    private List<int> currentRound;
    public int currentlyGrabbed = 0;
    public int totalRound = 10;

    public int currentTier { get; private set; }
    GlobalAudioController globalAudioController;

    public Collider barrier = new();

    //Methods
    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    private void Start() {
        shotsLeft = totalShots;
        currentRound = new List<int>();
        record = new List<List<int>>();
        globalAudioController = FindObjectOfType(typeof(GlobalAudioController)) as GlobalAudioController;
        pins = new List<GameObject>();
        resetPins();
        updateTier();
        // globalAudioController.playCheerAudio();

        //_textMeshPro.text = "Welcome" + " shots left: " + shots;
    }

    private void updateTier() {
        //Get all game objects with bowlable
        var objs = FindObjectsOfType<Bowlable>();
        //set tier to lowest current bowlable value
        int tier = Int32.MaxValue;
        foreach (var bowlable in objs) {
            if (bowlable.hasTier && tier > bowlable.tier) {
                tier = bowlable.tier;
            }
        }

        //Alternately, we could use trigger "setGrabbable" on all relevant objects.
        //However, this could make for more confusing code to maintain, albeit with better performance.
        // Debug.Log("currentTier" + tier + " " + objs.Length);
        currentTier = tier;
    }

    void resetPins() {
        foreach (var t in pins) {
            Destroy(t);
        }

        pins.Clear();

        List<Transform> childPositions = new();
        for (int i = 0; i < pinPosition.transform.childCount; i++) {
            childPositions.Add(pinPosition.transform.GetChild(i));
        }

        print("RESETTING PINS" + childPositions.Count);
        foreach (var t in childPositions) {
            pins.Add(Instantiate(pinPrefab, t));
        }
    }

    bool isStandingUp(GameObject p) {
        if (p.transform.rotation.x > -0.1
            && p.transform.rotation.x < 0.1
            && p.transform.rotation.z < 0.1
            && p.transform.rotation.z > -0.1) {
            return true;
        }

        return false;
    }


    public int numPinsFallen() {
        print(pins.Count(t => !isStandingUp(t)));
        return pins.Count(t => !isStandingUp(t));
    }

    void removeDownPins() {
        for (int i = pins.Count - 1; i >= 0; i--) {
            if (!isStandingUp(pins[i])) {
                print("DESTROYING PIN" + i);
                Destroy(pins[i]);
                pins.RemoveAt(i);
                //i--;
            }
        }
    }

    void destroyThrown(GameObject ball) {
        print("destroyThrown: Destroy the ball");
        Destroy(ball);
    }

    void updateDisplay(String s) {
        _textMeshPro.text = s;
    }

    public void lockBarrier() {
        barrier.enabled = true;
    }

    public void unlockBarrier() {
        barrier.enabled = false;
    }

    public void recordScore() {
        if (shotsLeft > 0) {
            currentRound.Add(numPinsFallen());
        }
        else {
            currentRound.Add(numPinsFallen());
            record.Add(currentRound);
            currentRound = new List<int>();
        }
    }

    private List<int> recalculateRecord() {
        List<List<int>> newRecord = new List<List<int>>();
        for (int i = 0; i < record.Count; i++) {
            newRecord.Add(new List<int>(record[i]));
            List<int> shot = new List<int>(record[i]);
            if (shot.Count == 1) {
                shot.Add(-1);
            }

            //Strike
            if (shot[0] == 10) {
                int temp = 10;

                //If the next one is strike a strike, and the one after has been taken
                if (i + 2 < record.Count && record[i + 1][0] == 10) {
                    temp += record[i + 1][0] + record[i + 2][0];
                }

                //If the next one is a strike, but there is no one after that
                else if (i + 1 < record.Count && record[i + 1][0] == 10) {
                    temp += 10;
                }

                //if this is a strike nad the next one isn't (and it exists)
                else if (i + 1 < record.Count) {
                    temp += record[i + 1][0] + record[i + 1][1];
                }

                int[] t = { temp, 0 };
                newRecord[i] = new List<int>(t);
            }
            //Spare
            else if (shot[0] + shot[1] == 10) {
                newRecord[i][0] = shot[0];
                int temp = shot[1];
                if (i + 1 < record.Count) {
                    temp += record[i + 1][0];
                }

                newRecord[i][1] = temp;
            }
            else {
                newRecord[i][0] = shot[0];

                if (shot[1] != -1)
                    newRecord[i][1] = shot[1];
            }
        }

        List<int> final = new();
        foreach (var r in newRecord) {
            final.Add(r[0]);
            if (r.Count > 1) {
                final[^1] += r[1];
            }
        }

        return final;
    }

    public void WaitForThrow(GameObject ball) {
        --shotsLeft;
        StartCoroutine(TidyUpGame(ball));
    }

    public IEnumerator TidyUpGame(GameObject ball) {
        yield return new WaitForSeconds(timeout);

        //first destroy ball
        destroyThrown(ball);


        //update tv
        print("SingleGameScore: " + numPinsFallen());


        //play cheer audio
        globalAudioController.playCheerAudio();

        // Strike, so no shots left this round
        if (numPinsFallen() == 10) {
            shotsLeft = 0;
        }

        //record
        recordScore();
        print("Record");
        foreach (var v in record) {
            print(v.ToSeparatedString(", "));
        }

        Debug.Log(record.ToSeparatedString(", "));
        var newRecord = recalculateRecord();
        scorePanel.throws = record;
        scorePanel.currentTotals = newRecord;
        print("Record2");
        string str = "";
        foreach (var v in newRecord) {
            str += v.ToString();
            str += " | ";
            print(v.ToString());
        }

        updateDisplay("ShotScore: \n" + numPinsFallen() + "Record: \n" + str);


        // One Round end
        if (shotsLeft == 0) {
            shotsLeft = totalShots;
            resetPins();
        }
        else {
            removeDownPins();
        }

        unlockBarrier();
    }

    void Update() {
        updateTier();

        //Debug.Log(score);
        //_textMeshPro.text = "score: " + score;
    }
}