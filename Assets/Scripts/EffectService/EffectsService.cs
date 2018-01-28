using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsService : MonoBehaviour {

    public List<AudioSource> botEngineAudioSource;
    public List<ParticleSystem> smoke;
    public ParticleSystem smokeExplosion;
    public int placeSmoked;
    public int placedEngine;

    public AudioClip engineSound;
    public AudioClip explosion;
    public AudioClip move;
    public AudioSource effectsPlayer;

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
            GameObject botAudioSource = new GameObject("Source");
            AudioSource au = botAudioSource.AddComponent<AudioSource>();
            au.playOnAwake = false;
            au.loop = true;
            au.volume = 0.1f;
            au.transform.SetParent(transform);

            botEngineAudioSource.Add(au);
        }
        effectsPlayer = new GameObject("Effects Player").AddComponent<AudioSource>();
        effectsPlayer.playOnAwake = false;
        effectsPlayer.loop = false;
        effectsPlayer.transform.SetParent(transform);
    }

    public void UpdateAudioVolumeThreshold(int playerNumbers)
    {
        for (int i = 0; i < 4; i++)
        {
            botEngineAudioSource[i].volume = 0.5f *( (1f / playerNumbers) * 0.5f);
        }
    }

    [ContextMenu("Play Music")]
    public void PlaceEngine()
    {
        if(placedEngine < botEngineAudioSource.Count)
        {
            botEngineAudioSource[placedEngine].clip = engineSound;
            botEngineAudioSource[placedEngine].Play();
            placedEngine += 1;
        }
    }

    [ContextMenu("Explode")]
    public void PlayExplosionSound()
    {
        effectsPlayer.PlayOneShot(explosion);
    }

    [ContextMenu("Move")]
    public void PlayMoveSound()
    {
        effectsPlayer.PlayOneShot(move);
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

    [ContextMenu("Place Smoke")]
    public void PlaceSmokeSample()
    {
        Vector3 vos = new Vector3(20 * placeSmoked, 20, 20);
        PlaceSmoke(vos);
    }

    public void PlaySmokeExplosion(Vector3 position)
    {
        smokeExplosion.transform.position = position;
        smokeExplosion.Play();
    }

    [ContextMenu("Play Smoke Sample")]
    public void PlaySmokeExplosionSample()
    {
        Vector3 vos = new Vector3(20 * placeSmoked, 20, 20);
        PlaySmokeExplosion(vos);
    }
}
