using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance { get; private set; }

    public int TotalScore { get; set; } = 0;

    [SerializeField]
    ScoreTicker totalScoreUI, roundScoreUI;

    [SerializeField]
    AnimationQueue animationQueue;

    void Awake() {
        if (Instance != null && Instance != this) { 
            Destroy(this);
        } 
        else {
            Instance = this;
        }
    }

    public void ScoreGraph(Graph graph) {
        // n-m+f=2.

        // Display all line scores
        animationQueue.EnqueueShowLineScores(graph);

        //Apply bonuses to lines
        BonusManager.Instance.CalculateBonuses(graph);

        int lineScore = graph.TotalLineScore();

        // int starCount = graph.StarCount();
        // int faceCount = 2 + lineScore - starCount;

        int roundScore = lineScore; // + starScore + faceScore;
        int newTotal = TotalScore + roundScore;
        TotalScore = newTotal;

        animationQueue.EnqueueTally(graph, roundScoreUI, roundScore, totalScoreUI, TotalScore);
        animationQueue.AnimateScore();
    }
}