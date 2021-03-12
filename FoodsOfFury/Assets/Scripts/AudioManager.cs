using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Jose Villanueva
//
// Description: This class acts as an AudioManager. It holds a dictionary of references
//              to AudioSource's and allows the playing of their audio
//----------------------------------------------------------------------------------------

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance; // AudioController instance

    private Dictionary<string, AudioSource> records = new Dictionary<string, AudioSource>(); // dictionary of AudioSources by Name

    public AudioSource[] audioInfo; // list of references to be set up
    
    // this creates the instance if there is none when accessed
    public static AudioManager Instance
    {
        get
        {
            if (instance == null) // create from resources folder if accessed but none exists (if moved in folder, this needs to be relinked)
            {
                GameObject go = Instantiate(Resources.Load("AudioManager", typeof(GameObject))) as GameObject;
                instance = go.GetComponent<AudioManager>();
                go.name = "AudioManager";
                DontDestroyOnLoad(instance);
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null) // assign as instace if none is
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this) // get rid of copies
        {
            Destroy(this);
        }

        // add AudioInfo to dictionary
        if (audioInfo != null)
        {
            foreach (AudioSource info in audioInfo)
            {
                if (info.clip == null) // check if clip is set up
                {
                    print("audioInfo at index: " + records.Count + " does not have a clip!");
                }
                else if (!records.ContainsKey(info.gameObject.name)) // add new audio source
                {
                    records.Add(info.gameObject.name, info);
                }
                else // audio source already exists (or the gameObjects have the same name)
                {
                    print("Audio list already contains: " + info.gameObject.name);
                }
            }
        }
    }

    // play audio source from dictionary and return copy
    public AudioSource play(string clipName, Vector3 position, bool destroyAfterPlay = true)
    {
        AudioSource info;

        if (records.TryGetValue(clipName, out info))
        {
            GameObject go = Instantiate(info.gameObject); // create copy from dictionary
            go.transform.position = position;

            AudioSource source = go.GetComponent<AudioSource>();
            source.Play(); // play audio source

            if (destroyAfterPlay) // destroy after playing clip
            {
                Destroy(go, source.clip.length);
            }

            return source;
        }
        else
        {
            print("\"" + clipName + "\" was not found in audio list");
            return null;
        }
    }

    // play random audio source from clips
    public AudioSource playRandom(Vector3 position, params string[] clips)
    {
        int index = Random.Range(0, clips.Length);
        return play(clips[index], position);
    }

    // play random audio source from clips
    public AudioSource playRandom(Vector3 position, bool destroyAfterPlay, params string[] clips)
    {
        int index = Random.Range(0, clips.Length);
        return play(clips[index], position, destroyAfterPlay);
    }

    // DEBUG: print all objects in dictionary
    public void printList()
    {
        foreach (AudioSource info in records.Values)
        {
            print(info.gameObject.name);
        }
    }

    // release instance on destroy (if this was it)
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
