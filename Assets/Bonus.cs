using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour {
    
    public enum BonusOperation {
        ADD,MULT
    };

    [SerializeField]
    BonusOperation operation;
    [SerializeField]
    int amount;

    [SerializeField]
    Star.StarColor[] propertyChain;

    public void SetUp(BonusOperation operation, int amount, Star.StarColor[] propertyChain) {
        this.operation = operation;
        this.amount = amount;
        this.propertyChain = propertyChain;
    }

    // MOVE TO SCORE MANAGER
    public void ApplyBonus(List<Line> lines) {
        foreach (Line line in lines) {
            if (operation == BonusOperation.ADD) {
                line.Score += amount;
            }
            else if (operation == BonusOperation.MULT) {
                line.Score *= amount;
            }
        }
    }

    public List<Line> SearchGraph(List<Star> stars, List<Line> lines) {

        List<Star> startingStars = new List<Star>();
        foreach (Star star in stars) {
            if (star.Matches(propertyChain[0])) {
                startingStars.Add(star);
            }
        }
        if (startingStars.Count <= 0) {
            return null;
        }

        Queue<Line> linePath = new Queue<Line>();
        Queue<Star> starPath = new Queue<Star>();


        for (int i = 0; i < propertyChain.Length; i++) {
            
        }
        return null;
    }

    public Star BFS(Star root, Star.StarColor target, int maxDepth) {
        Queue<Star> q = new Queue<Star>();
        List<Star> visited = new List<Star>();
        q.Enqueue(root);
        int searchDepth = 0;
        while(searchDepth < maxDepth)
        {
            Star current = q.Dequeue();
            if(current == null)
                continue;
            visited.Add(current);
            foreach (Line line in current.Lines) {
                Star otherStar = line.OtherStar(current);
                if (otherStar != null && !visited.Contains(otherStar)) {
                    q.Enqueue(otherStar);
                }
            }
            if (current.Color == target) {
                return current;
            }
            searchDepth += 1;
        }
        return null;
    }


    
}
