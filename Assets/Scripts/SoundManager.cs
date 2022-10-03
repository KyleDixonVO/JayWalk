using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource moneyAudio;
    public AudioSource damageAudio;
    public AudioSource healAudio;



    // Start is called before the first frame update
    void Start()
    {
        moneyAudio.playOnAwake = false;
        damageAudio.playOnAwake = false;
        healAudio.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayHealAudio()
    {
        if (healAudio.isPlaying) return;
        healAudio.Play();
    }

    public void PlayDamageAudio()
    {
        if (damageAudio.isPlaying) return;
        damageAudio.Play();
    }

    public void PlayMoneyAudio()
    {
        if (moneyAudio.isPlaying) return;
        moneyAudio.Play();
    }
}
