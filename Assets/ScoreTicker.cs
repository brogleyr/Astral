using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ScoreTicker : MonoBehaviour
{

    public bool Revealed { get; private set; } = false;
    
    protected bool ticking;
    protected int currentTotal = 0;
    protected int startTotal = 0;
    protected int targetTotal = 0;
    protected float timer = 0.0f;
    protected float timerDuration = 0.0f;

    TextMeshProUGUI scoreGUI;

    void Start() {
        scoreGUI = GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        if (Revealed && currentTotal != 0) {
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
        Revealed = true;
        startTotal = currentTotal;
        targetTotal = newTotalScore;
        timerDuration = duration;
        ticking = true;

        yield return null;
    }

    public IEnumerator SetValue(int value) {
        this.currentTotal = value;
        yield return null;
    }

    public IEnumerator ClearTicker() {
        currentTotal = 0;
        Revealed = false;
        yield return null;
    }

    internal IEnumerator ToggleVisible()
    {
        Revealed = !Revealed;
        yield return null;
    }
}
