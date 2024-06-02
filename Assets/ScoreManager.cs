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
        animationQueue.EnqueueShowLineScores(graph);

        //Apply bonuses to lines
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