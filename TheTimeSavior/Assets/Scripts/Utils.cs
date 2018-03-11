using Sound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static bool IsColliding(Transform object1, Transform object2)
    {
        var collider2D = object1.GetComponent<Collider2D>();
        if (collider2D == null)
            throw new System.Exception("Object1 does not have a collider 2D");

        var dimentsioniOggetto1 = collider2D.bounds.size;
        var posizioneOggetto1 = object1.transform.position;
        var posizioneOggetto2 = object2.transform.position;
        var larghezzaOggetto1DaAggiungere = dimentsioniOggetto1.x / 2;
        var lunghezzaOggetto1DaAggiungere = dimentsioniOggetto1.y / 2;

        var xDentro = (
                posizioneOggetto2.x >= posizioneOggetto1.x &&
                posizioneOggetto2.x <= posizioneOggetto1.x + larghezzaOggetto1DaAggiungere
            ) || (
                posizioneOggetto2.x <= posizioneOggetto1.x &&
                posizioneOggetto2.x >= posizioneOggetto1.x - larghezzaOggetto1DaAggiungere
            );

        var yDentro = (
                posizioneOggetto2.y >= posizioneOggetto1.y &&
                posizioneOggetto2.y <= posizioneOggetto1.y + lunghezzaOggetto1DaAggiungere
            ) || (
                posizioneOggetto2.y <= posizioneOggetto1.y &&
                posizioneOggetto2.y >= posizioneOggetto1.y - lunghezzaOggetto1DaAggiungere
            );

        return (xDentro && yDentro);
    }

    public static List<GameObject> PopAdnDestroy (List<GameObject> list, int indexToDestroy)
    {
        var elementToDestroy = list[indexToDestroy];        
        GameObject.Destroy(elementToDestroy);
        list.RemoveAt(indexToDestroy);
        return list;
    }

    
}
public static class Extensions
{
    public static AudioManager GetSetEmitter(this MonoBehaviour obj, string emitterName)
    {
        var _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _audioManager.SetEmitter(obj.GetComponent<FMODUnity.StudioEventEmitter>(), emitterName);

        return _audioManager;
    }
}
