using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Star : MonoBehaviour
{

    public enum StarColor {WHITE, RED, BLUE, WILD};
    public StarColor Color { get; set; } = StarColor.WHITE;
    [SerializeField]
    float redChance = 1f / 3f;
    [SerializeField]
    float blueChance = 1f / 3f;

    [SerializeField]
    SpriteAtlas spriteAtlas;

    public Graph Graph;
    public List<Line> Lines { get; set; } = new List<Line>();

    void Start() {
        // float colorPick = Random.value;
        // if (colorPick < redChance) {
        //     GetComponent<SpriteRenderer>().color = new Color(1f, 0.7f, 0.7f, 1f);
        //     Color = StarColor.RED;
        // }
        // else if (colorPick < blueChance + redChance) {
        //     GetComponent<SpriteRenderer>().color = new Color(0.7f, 0.7f, 1f, 1f);
        //     Color  = StarColor.BLUE;
        // }

        // transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Random.Range(-36f, 36f));
    }

    public bool Matches(StarColor matchColor) {
        if (matchColor == StarColor.WILD) {
            return true;
        }
        else if (matchColor == this.Color) {
            return true;
        }
        return false;
    }

    void OnMouseEnter() {
        Graph.StarEvent(this);
    }

    static T GetRandomEnum<T>() {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
        return V;
    }
}
