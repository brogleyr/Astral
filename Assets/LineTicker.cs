using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;

public class LineTicker : ScoreTicker {
    Line line;
    TextMeshPro textMesh;
    
    void Start() {
        line = transform.parent.GetComponent<Line>();
        currentTotal = line.Score;
        textMesh = GetComponent<TextMeshPro>();
    }

    void Update() {
        if (Revealed) {
            textMesh.text = currentTotal.ToString();
        }
        else {
            textMesh.text = "";
        }

        if (ticking) {
            timer += Time.deltaTime;
            currentTotal = (int) Mathf.Round(Mathf.SmoothStep(startTotal, targetTotal, timer / timerDuration));
        }

        if (currentTotal == targetTotal) {
            timer = 0;
            ticking = false;
        } 
    }
}