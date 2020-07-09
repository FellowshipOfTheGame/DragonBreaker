using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Maestro : MonoBehaviour {

    public float bpm = 160;
	int count = 1;
	float crotchet, startPos;
	
    bool counting = false, change = false, yelled = false;
	public AudioSource[] source;
    public AudioSource scream;
    public AudioMixer mixer;

	public delegate void Step(int counter);
	public Step step;

    public bool playOnAwake;

    int current;

	// Use this for initialization
    void Start(){
        if(playOnAwake) Initialize();
    }

	public void Initialize(){
		crotchet = 60.0f / bpm;
		step = delegate {count++;};
        counting = true;

        Reset(0);
	}

    void Reset(int song){
        startPos = (float)(AudioSettings.dspTime);
        count = 0;
		current = song;
        source[song].Play();
    }

	// Update is called once per frame
	void Update () {
		if (counting){
            float songPos = (float)(AudioSettings.dspTime) - startPos;
	
            if (songPos >= count * crotchet){
                if (change){
                    if (count % 4 == 2){
                        scream.Play();
                        yelled = true;
                    }
                    if(count % 4 == 0 && yelled){
                        yelled = false;
                        SwitchMusic();
                    }
                }
				
				step(count - 1);
			}
        }
	}

    void SwitchMusic(){
        change = false;
        //source.Stop();
        StartCoroutine( StartFade("vol"+(2 - current).ToString(), 0.01f, 1.0f) );
        StartCoroutine( StartFade("vol"+(current + 1).ToString(), 0.5f, 0.0f) );
        Reset(1-current);
    }

    public void ChangeMusic(){
        change = true;
    }

    public IEnumerator StartFade(string vol, float duration, float targetVolume){
        float currentTime = 0;
        float currentVol;
        mixer.GetFloat(vol, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration){
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            mixer.SetFloat(vol, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
