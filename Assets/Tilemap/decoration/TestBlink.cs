using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBlink : MonoBehaviour{

    SpriteRenderer sprite;
    float startTime;

    // Start is called before the first frame update
    void Start(){
        sprite = GetComponent<SpriteRenderer>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update(){
        if (Time.time - startTime > 0.7f){
            sprite.enabled = !sprite.enabled;
            startTime = Time.time;
        }
    }
}
