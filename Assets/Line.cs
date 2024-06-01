using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    public Star[] Stars { get; set; } = new Star[2];
    public int Score { get; set; } = 0;

    Vector3[] positions = new Vector3[2];
    Vector3 scorePosition;
    

    void Update() {
        if (Stars[0] == null) {
            return;
        }

        positions[0] = Stars[0].transform.position;

        if (Stars[1] == null) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            positions[1] = mousePosition;
        }
        else {
            positions[1] = Stars[1].transform.position;
        }

        GetComponent<LineRenderer>().SetPositions(positions);
    }

    public void SetStart(Star start) {
        Stars[0] = start;
    }

    public void SetEnd(Star end) {
        Stars[1] = end;
        Score = 1;
        PlaceScoreInCenter();
    }

    public Star OtherStar(Star star) {
        if (star == Stars[0]) {
            return Stars[1];
        }
        else if (star == Stars[1]) {
            return Stars[0];
        }
        return null;
    }

    public void DisplayScore() { 
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
    }

    public void MoveScoreToTotal(Vector3 finalLocation, float time) {
        transform.GetChild(0).GetComponent<Animator>().SetFloat("MoveSpeed", 1f / time);
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("MoveScore");
    }

    void PlaceScoreInCenter() {
        Vector3 center = ((positions[0] - positions[1]) * 0.5f) + positions[1];
        transform.GetChild(0).position = new Vector3(center.x, center.y, 0f);
    }
}
