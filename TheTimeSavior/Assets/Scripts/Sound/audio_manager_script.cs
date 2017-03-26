using UnityEngine;
using System.Collections;
//Script che gestisce le musiche in-game è collegato al game master e non viene mai distrutto
//Permette di creare gli oggetti per le musiche tramite codice e di cambiare valori come volume e pitch

[System.Serializable]
public class Sound {

	public string name;
	public AudioClip clip;
	private AudioSource source;

	[Range(0f,1f)]
	public float volume = 0.7f;
	//[Range(0.5f,1.5f)]
	//public float pitch = 1f;

	public void SetSource (AudioSource _source)
	{
		source = _source;
		source.clip = clip;
	
	}

	public void Play()
	{
		source.volume = volume;
		//source.pitch = pitch;
		source.Play ();
	
	}

    public void Stop()
    {
        source.volume = volume;
        //source.pitch = pitch;
        source.Stop();

    }


}


public class audio_manager_script : MonoBehaviour {
	//Script richiamabile ovunque
	public static audio_manager_script _audioM;

	//Vettore che contiene tutte le canzoni da inserire nel inspector
	[SerializeField]
	Sound[] sounds;

	void Awake()
	{
		_audioM = this;
	}

	void Start ()
	{
		
		for ( int i = 0; i < sounds.Length; i++)
		{
			GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
			sounds [i].SetSource (_go.AddComponent<AudioSource> ());
			GameObject.DontDestroyOnLoad (_go);
		}

		PlaySound ("test");
	}

	//Funzione che fa partire una canzone dandogli come parametro una stringa
	public void PlaySound (string _name)
	{
		for (int i = 0; i < sounds.Length; i++) 
		{
			if (sounds [i].name == _name) 
			{
				sounds [i].Play ();
				return;
			}
		}
	}
    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
    }
}