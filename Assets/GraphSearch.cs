using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphSearch
{
    List<Line> lines;
    List<Star> stars;

    // 2D array of Lines connecting paris of stars
    // Star intersections are indexed the same as in this.stars
    // Stars that are not adjacent have null as the value for their adjacency
    Line[,] adjacencies;

    public GraphSearch(List<Line> lines, List<Star> stars) {
        this.lines = lines;
        this.stars = stars;

        adjacencies = new Line[stars.Count, stars.Count];
        foreach (Line line in lines) {
            int index1 = stars.IndexOf(line.Stars[0]);
            int index2 = stars.IndexOf(line.Stars[1]);

            adjacencies[index1, index2] = line;
            adjacencies[index2, index1] = line;
        }
    }

    public List<List<Line>> FindSequence(List<Star.StarColor> starColors) {
        return null;
    }

    // public List<Line> SearchGraph(List<Star> stars, List<Line> lines) {

    //     List<Star> startingStars = new List<Star>();
    //     foreach (Star star in stars) {
    //         if (star.Matches(propertyChain[0])) {
    //             startingStars.Add(star);
    //         }
    //     }
    //     if (startingStars.Count <= 0) {
    //         return null;
    //     }

    //     Queue<Line> linePath = new Queue<Line>();
    //     Queue<Star> starPath = new Queue<Star>();


    //     for (int i = 0; i < propertyChain.Length; i++) {
            
    //     }
    //     return null;
    // }


    /***
    Given a starting star and a target color for another star, return the list of adjacent stars that match
    ***/
    // public List<Tuple<Star, Line>> BFS(Star root, Star.StarColor target) {
    //     Queue<Star> q = new Queue<Star>();
    //     List<Star> visited = new List<Star>();
    //     q.Enqueue(root);
    //     int searchDepth = 0;
    //     while(searchDepth < maxDepth)
    //     {
    //         Star current = q.Dequeue();
    //         if(current == null)
    //             continue;
    //         visited.Add(current);
    //         foreach (Line line in current.Lines) {
    //             Star otherStar = line.OtherStar(current);
    //             if (otherStar != null && !visited.Contains(otherStar)) {
    //                 q.Enqueue(otherStar);
    //             }
    //         }
    //         if (current.Color == target) {
    //             return current;
    //         }
    //         searchDepth += 1;
    //     }
    //     return null;
    // }


}
