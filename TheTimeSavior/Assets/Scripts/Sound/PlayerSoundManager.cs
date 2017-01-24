using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    AudioSource myAudioSource;

    //Variabili per il suono del jump
    public AudioClip jumpSound;
    [Range(0.0f, 1.0f)] public float jumpSoundVolume;

    //Variabili per il suono su hit del nemico
    public AudioClip enemyHitSound;
    [Range(0.0f, 1.0f)] public float enemyHitSoundVolume;

    //Variabili per il suono alla morte del player
    public AudioClip deathSound;
    [Range(0.0f, 1.0f)] public float deathSoundVolume;

    void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSound()
    {
        myAudioSource.clip = jumpSound;
        myAudioSource.volume = jumpSoundVolume;
        myAudioSource.Play();
    }

    public void PlayEnemyHitSound ()
    {
        myAudioSource.clip = enemyHitSound;
        myAudioSource.volume = jumpSoundVolume;
        myAudioSource.Play();
    }

    public void PlayDeathSound()
    {
        myAudioSource.clip = deathSound;
        myAudioSource.volume = deathSoundVolume;
        myAudioSource.Play();
    }
}
