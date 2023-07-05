using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioClip BossDeadShound;
    public List<AudioClip> BGMusic; 
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().PlayOneShot(BGMusic[0]);
    }

    public void PlayMusic()
    {
        int i = UnityEngine.Random.Range(0, BGMusic.Count);
        GetComponent<AudioSource>().clip = BGMusic[i];
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!GetComponent<AudioSource>().isPlaying)
        {
            PlayMusic();
        }
    }
}
