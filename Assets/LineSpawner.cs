using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSpawner : MonoBehaviour
{
    
    public static LineSpawner Instance { get; private set; }

    [SerializeField]
    GameObject linePrefab;

    // Line currLine = null;

    void Awake() {
        if (Instance != null && Instance != this) { 
            Destroy(this);
        } 
        else {
            Instance = this;
        }
    }

    public Line StartLine(Star star) {
        Line line = Instantiate(linePrefab, star.transform.position, Quaternion.identity).GetComponent<Line>();
        line.SetStart(star);
        return line;
    }
}
