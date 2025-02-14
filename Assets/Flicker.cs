using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Flicker : MonoBehaviour
{
    public SpriteAtlas spriteAtlas { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        int spriteCount = spriteAtlas.spriteCount;
        Sprite[] sprites = new Sprite[spriteCount];
        spriteAtlas.GetSprites(sprites);
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, spriteCount)];

        float colorPick = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
