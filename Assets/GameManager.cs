using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    int graphSize = 2;
    [SerializeField]
    int maxLines = 1;

    public enum GameState {
        DRAW,COUNT,DESTROY
    }
    GameState state = GameState.DRAW;

    void Start() {
        GetComponent<Graph>().NewGraph(graphSize, maxLines);
    }


    void Update() {
        if (state == GameState.DRAW) {
            UpdateDraw();
        }
        else if (state == GameState.DESTROY) {
            GetComponent<Graph>().NewGraph(graphSize, maxLines);
            state = GameState.DRAW;
        }
    }

    void UpdateDraw() {
        if (Input.GetButtonDown("Fire1")) {
            Count();
        }

        if (Input.GetButtonDown("Fire2")) {
            GetComponent<Graph>().ClearEvent();
        }
    }

    void Count() {
        if (GetComponent<Graph>().IsEmpty()) {
            return;
        }
        state = GameState.COUNT;
        // ScoreGraph starts an animation that will trigger when finished
        GetComponent<Graph>().CountLock();

        GetComponent<ScoreManager>().ScoreGraph(GetComponent<Graph>());
    }

    public void CountFinished() {
        Debug.Log("Count Finished");
        int totalScore = GetComponent<ScoreManager>().TotalScore;
        graphSize = GetSizeFromScore(totalScore);
        maxLines = GetLinesFromScore(totalScore);
        state = GameState.DESTROY;
    }

    int GetSizeFromScore(int totalScore) {
        return Mathf.Max(2, Mathf.CeilToInt(StarGrowth(totalScore)));
    }

    int GetLinesFromScore(int totalScore) {
        return Mathf.Max(1, Mathf.CeilToInt(2 * StarGrowth(totalScore)) - 3);
    }

    float StarGrowth(int totalScore) {
        return Mathf.Pow(totalScore + 7, 1f / 3f);
    }

    public GameState GetState() {
        return state;
    }
}
