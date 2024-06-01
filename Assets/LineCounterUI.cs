using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCounterUI : MonoBehaviour
{
    [SerializeField]
    Graph graph;

    [SerializeField]
    GameObject lineCounterPrefab;

    int lineCount = 1;
    
    void Update()
    {
        int linesRemaining = graph.GetLinesRemaining();

        while (linesRemaining != lineCount) {
            if (linesRemaining > lineCount) {
                AddCounter();
            }
            else {
                DestroyCounter();
            }
        }
    }

    void AddCounter() {
        Instantiate(lineCounterPrefab, this.transform);
        lineCount++;
    }

    void DestroyCounter() {
        Destroy(transform.GetChild(0).gameObject);
        lineCount--;
    }
}
