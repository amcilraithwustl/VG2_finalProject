using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePanel : MonoBehaviour {
    public List<int> currentTotals = new();
    public List<List<int>> throws = new();
    public GameObject panelPrefab;

    private List<PanelScript> panels;

    private void Start() {
        panels = new();
    }

    private void Update() {
        print(currentTotals);
        print(throws);
        for (int i = 0; i < throws.Count; i++) {
            var t = throws[i];
            if (i > panels.Count) {
                var a = Instantiate(panelPrefab, transform);
                var r = a.GetComponent<RectTransform>().rect;
                r.position = new Vector2(-900 + 200 * i, 150);
                panels.Add(a.GetComponent<PanelScript>());
            }

            panels[i].first.text = t[0].ToString();
            if (t.Count == 2) {
                panels[i].second.text = t[1].ToString();
            }
        }

        for (int i = 0; i < currentTotals.Count; i++) {
            panels[i].sum.text = currentTotals[i].ToString();
        }

    }
}