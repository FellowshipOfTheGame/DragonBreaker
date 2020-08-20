using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {

    [System.Serializable]
    public class Audio {
        public string title;
        public AudioClip clip;
    }

    [System.Serializable]
    public class RandomAudio {
        public string title;
        public AudioClip[] clip;
    }

    public Audio[] single;
    public RandomAudio[] random;

    AudioSource source;


    // Start is called before the first frame update
    void Start() {
        source = this.gameObject.AddComponent<AudioSource>();
        source.loop = false;
        source.playOnAwake = false;
    }

    public void Play(string label){
        foreach(Audio a in single){
            if (a.title == label){
                source.clip = a.clip;
                source.Play();
                //Debug.Log("found!");
                return;
            }
        }

        foreach (RandomAudio ra in random) {
            if (ra.title == label) {
                source.clip = ra.clip[Random.Range(0, ra.clip.Length - 1)];
                source.Play();
                return;
            }
        }

        Debug.Log("Audio not found");
    }
}
