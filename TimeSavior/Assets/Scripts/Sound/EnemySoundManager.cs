using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    AudioSource myAudioSource;

    //Variabili per il suono on hit del bullet
    public AudioClip myHitByBulletSound;
    [Range(0.0f, 1.0f)] public float HitByBulletVolume = 0.5f;

    //Variabili per il suono della morte
    public AudioClip myDeathSound;
    [Range(0.0f, 1.0f)] public float DeathVolume = 0.5f;

    void Awake()
    {
        myAudioSource = GameObject.Find("_EnemySound").GetComponent<AudioSource>();
    }

    public void PlayOnHitByBullet()
    {
        myAudioSource.clip = myHitByBulletSound;
        myAudioSource.volume = HitByBulletVolume;
        myAudioSource.Play();
    }

    public void PlayOnDeath()
    {
        myAudioSource.clip = myDeathSound;
        myAudioSource.volume = HitByBulletVolume;
        myAudioSource.Play();
    }
}
