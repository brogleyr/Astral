using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;

public class LineTicker : ScoreTicker {
    Line line;
    TextMeshPro textMesh;

    bool moving;
    Vector3 startPosition, endPosition;
    float moveDuration;
    float moveTimer = 0f;
    
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

        if (moving) {
            moveTimer += Time.deltaTime;
            transform.position = Vector3.Lerp(
                startPosition,
                endPosition,
                Mathf.SmoothStep(0.0f, 1.0f, moveTimer / moveDuration)
            );
            if (moveTimer > moveDuration) {
                moving = false;
            }
        }

        if (currentTotal == targetTotal) {
            timer = 0;
            ticking = false;
        } 
    }

    public IEnumerator MoveToPoint(Vector3 point, float duration) {
        startPosition = transform.position;
        endPosition = point;
        moveDuration = duration;
        moveTimer = 0f;
        moving = true;
        yield return null;
    }
}