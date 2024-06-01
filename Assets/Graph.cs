using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    List<Line> lines = new List<Line>();
    List<Star> stars = new List<Star>();
    Line currLine = null;

    int maxLines = 1;

    public void NewGraph(int size, int maxLines) {
        ClearCurrLine();
        ClearLines();
        ClearStars();
        this.stars = StarSpawner.Instance.SpawnStarMap(size);
        this.maxLines = maxLines;
    }

    public void ClearEvent() {
        if (currLine == null) {
            ClearLines();
        }
        else {
            ClearCurrLine();
        }
    }

    void ClearLines() {
        foreach (Line line in lines) {
            Destroy(line.gameObject);
        }
        lines = new List<Line>();
    }

    public void DisplayScores(bool display) {
        foreach (Line line in lines) {
            line.DisplayScore();
        }
    }

    public void MoveScoresToTotal(Vector3 finalPosition, float time) {
        foreach (Line line in lines) {
            line.MoveScoreToTotal(finalPosition, time);
        }
    }

    public void ClearCurrLine() {
        if (currLine == null) {
            return;
        }
        Destroy(currLine.gameObject);
        currLine = null;
    }

    void ClearStars() {
        foreach (Star star in stars) {
            Destroy(star.gameObject);
        }
        stars = new List<Star>();
    }
    

    public void StarEvent(Star star) {

        if (lines.Count >= maxLines) {
            return;
        }

        if (currLine == null) {
            CreateNewCycle(star);
            return;
        }

        if (currLine.Stars[0] == star) {
            return;
        }

        if (LineAlreadyExists(star, currLine.Stars[0])) {
            return;
        }

        if (LineIntersects(star, currLine.Stars[0])) {
            return;
        }

        ExtendCycle(star);

    }

    void CreateNewCycle(Star star) {
        currLine = GetComponent<LineSpawner>().StartLine(star);
    }

    void ExtendCycle(Star star) {
        currLine.SetEnd(star);
        lines.Add(currLine);

        if (lines.Count < maxLines) {
            currLine = GetComponent<LineSpawner>().StartLine(star);
        }
        else {
            currLine = null;
        }
    }

    bool LineAlreadyExists(Star star1, Star star2) {
        foreach (Line line in lines) {
            
            if (line.Stars[0] == null || line.Stars[1] == null) {
                continue;
            }
            // [star1, star2] is a line
            bool lineCheck1 = (star1 == line.Stars[0]) && (star2 == line.Stars[1]);
            // [star2, star1] is a line
            bool lineCheck2 = (star2 == line.Stars[0]) && (star1 == line.Stars[1]);

            if (lineCheck1 || lineCheck2) {
                return true;
            }
        }
        return false;
    }

    public int TotalLineScore() {
        int sum = 0;
        foreach (Line line in lines) {
            sum += line.Score;
        }
        return sum;
    }

    public int GetLinesRemaining() {
        return maxLines - lines.Count;
    }

    public int StarCount() {
        return stars.Count;
    }

    bool LineIntersects(Star star1, Star star2) {
        foreach (Line line in lines) {
            Vector2 pos1 = new Vector2(star1.transform.position.x, star1.transform.position.y);
            Vector2 pos2 = new Vector2(star2.transform.position.x, star2.transform.position.y);
            Vector2 pos3 = new Vector2(line.Stars[0].transform.position.x, line.Stars[0].transform.position.y);
            Vector2 pos4 = new Vector2(line.Stars[1].transform.position.x, line.Stars[1].transform.position.y);
            Vector2 intersection2d;
            if (LineUtil.IntersectLineSegments2D(pos1, pos2, pos3, pos4, out intersection2d)) {
                // Add buffer
                float buffer = 0.1f;
                if (Vector2.Distance(intersection2d, pos1) < buffer || Vector2.Distance(intersection2d, pos2) < buffer) {
                    continue;
                }
                return true;
            }
        }
        return false;
    }
}
