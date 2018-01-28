using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsService : MonoBehaviour {

    public List<BotDefineScriptableObject> botDefines;

    public List<AudioSource> botEngineAudioSource;
    public List<ParticleSystem> smoke;
    public ParticleSystem smokeExplosion;
    public int placeSmoked;
    public int placedEngine;

    public AudioClip engineSound;

    public void Awake()
    {
        placeSmoked = 0;
        placedEngine = 0;
        for (int i = 0; i < smoke.Count; i++)
        {
            smoke[i].Stop();
        }

        botEngineAudioSource = new List<AudioSource>();
        for (int i = 0; i < 4; i++)
        {
            GameObject botAudioSource = new GameObject();
            AudioSource au = botAudioSource.AddComponent<AudioSource>();
            botEngineAudioSource.Add(au);
        }
    }

    public void PlaceEngine()
    {
        if(placedEngine < botEngineAudioSource.Count)
        {
            botEngineAudioSource[placedEngine].clip = engineSound;
            botEngineAudioSource[placedEngine].Play();
            placedEngine += 1;
        }
    }

    public void PlaceSmoke(Vector3 position)
    {
        if (placeSmoked < smoke.Count)
        {
            smoke[placeSmoked].transform.position = position;
            smoke[placeSmoked].Play();
            placeSmoked += 1;
        }
    }

    public void PlaySmokeExplosion(Vector3 position)
    {
        smokeExplosion.transform.position = position;
        smokeExplosion.Play();
    }

}
