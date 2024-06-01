using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;


public class ScoreTicker : MonoBehaviour
{

    bool revealed = false;
    bool ticking;
    
    int currentTotal = 0;
    int startTotal = 0;
    int targetTotal = 0;
    float timer = 0.0f;
    float timerDuration = 0.0f;

    TextMeshProUGUI scoreGUI;

    void Start() {
        scoreGUI = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        if (revealed && currentTotal != 0) {
            scoreGUI.text = currentTotal.ToString();
        }
        else {
            scoreGUI.text = "";
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

    public IEnumerator TickTo(int newTotalScore, float duration) {
        revealed = true;
        startTotal = currentTotal;
        targetTotal = newTotalScore;
        timerDuration = duration;
        ticking = true;

        yield return null;
    }

    public IEnumerator ClearTicker() {
        currentTotal = 0;
        revealed = false;
        yield return null;
    }
}
