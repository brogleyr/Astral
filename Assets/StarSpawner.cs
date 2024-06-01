using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public static StarSpawner Instance { get; private set;}

    [SerializeField]
    GameObject starPrefab;
    // List<Star> stars = new List<Star>();

    public Rect starfield;
    float topMargin = 1f;
    float bottomMargin = 1f;
    float leftMargin = 2f;
    float rightMargin = 2f;

    void Awake() {
        if (Instance != null && Instance != this) { 
            Destroy(this); 
        } 
        else { 
            Instance = this; 
        }
        SetStarfield();
    }

    public List<Star> SpawnStarMap(int starCount) {
        List<Star> stars = new List<Star>();
        for (int i = 0; i < starCount; i++) {
            Star newStar = SpawnStar();
            stars.Add(newStar);
        }
        return stars;
    }

    Star SpawnStar() {
        
        Vector3 newStarPosition = new Vector3(Random.Range(starfield.xMin, starfield.xMax), Random.Range(starfield.yMin, starfield.yMax), 0);
        Star newStar = Instantiate(starPrefab, newStarPosition, Quaternion.identity).GetComponent<Star>();
        newStar.Graph = GetComponent<Graph>();
        return newStar;
    }

    void SetStarfield() {
        var camHeight = Camera.main.GetComponent<Camera>().orthographicSize;	
    	var camWidth = camHeight * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        Vector2 camOrigin = Camera.main.transform.position;
    	float minX = (camOrigin.x - camWidth) + leftMargin;
    	float maxY = (camOrigin.y + camHeight) - topMargin;
    
        float height = -camHeight * 2f + (topMargin + bottomMargin);
        float width = camWidth * 2f - (rightMargin + leftMargin);

        starfield = new Rect(minX, maxY, width, height);
    }
}
