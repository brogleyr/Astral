using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public int TotalScore { get; set; } = 0;

    [SerializeField]
    ScoreTicker totalScoreUI, roundScoreUI;

    List<Bonus> active_bonuses = new List<Bonus>();
    public List<Bonus> all_bonuses = new List<Bonus>();

    AnimationQueue animationQueue;

    private void Start() {
        animationQueue = GetComponent<AnimationQueue>();
    }

    public void ScoreGraph(Graph graph) {
        // n-m+f=2.

        // Display all line scores
        graph.ClearCurrLine();
        graph.DisplayScores(true);

        //Apply bonuses to lines

        // Add moving line scores to the animation queue

        int lineScore = graph.TotalLineScore();

        // int starCount = graph.StarCount();
        // int faceCount = 2 + lineScore - starCount;

        int roundScore = lineScore; // + starScore + faceScore;
        int newTotal = TotalScore + roundScore;
        TotalScore = newTotal;

        // TODO: Execute AnimationQueue
        animationQueue.EnqueueTick(roundScoreUI, roundScore, 0.5f);
        animationQueue.EnqueuePause(0.5f);
        animationQueue.EnqueueTicks(new [] {
            (roundScoreUI, 0, 0.5f ),
            (totalScoreUI, newTotal, 0.5f)
        });
        animationQueue.EnqueueClearTicker(roundScoreUI);
        animationQueue.EnqueuePause(1.0f);

        animationQueue.AnimateScore();
    }

    // public int ScoreGraphOld(Graph graph) {
    //     IEnumerator animateScore = AnimateScore(graph);
    //     StartCoroutine(animateScore);
    //     return TotalScore;
    // }

    // IEnumerator AnimateScore(Graph graph) {    
    //     // n-m+f=2.

    //     // Display all line scores
    //     graph.ClearCurrLine();
    //     graph.DisplayScores(true);

    //     //Apply bonuses to lines


    //     int lineScore = graph.TotalLineScore();
    //     // int starCount = graph.StarCount();
    //     // int faceCount = 2 + lineScore - starCount;
    //     int roundScore = lineScore;

    //     Debug.Log("Score Calculated");
        
    //     // graph.MoveScoresToTotal(new Vector3(0f, 4f, 0f), 1.5f);
    //     // Debug.Log("Scores Moved");
        
    //     // float roundScoreTickerTime = 0.5f;
    //     // roundScoreUI.AddScoreToTotal(roundScore, roundScoreTickerTime);
    //     // Debug.Log("Round Score");
    //     // Debug.Log(roundScore);

    //     // yield return new WaitForSeconds(roundScoreTickerTime + 1f);
        
    //     int newTotal = TotalScore + roundScore;

    //     // float totalScoreTickerTime = 0.5f;
    //     // roundScoreUI.AddScoreToTotal(0, totalScoreTickerTime);
    //     // scoreUI.AddScoreToTotal(newTotal, totalScoreTickerTime);
    //     // Debug.Log("Total Score");
    //     // Debug.Log(newTotal);
        
    //     TotalScore = newTotal;
    //     yield return new WaitForSeconds(2.0f);

    // }
}