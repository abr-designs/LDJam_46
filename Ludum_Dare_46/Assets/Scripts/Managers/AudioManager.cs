using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public enum EFFECT
    {
        STEP = 0,
        SPLASH,
        FIRE,
        GOT_FIRE,
        PUT_OUT,
        THROW,
        TO_ASH,
        BEACON,
        PUSH,
        
    }

    [Serializable]
    public enum MUSIC
    {
        NONE,
        MENU,
        JUNGLE,
        DESERT
    }

    [Serializable]
    public struct EffectProfile
    {
        public string name;
        public Vector2 pitchRange;
    }
    
    //================================================================================================================//

    private MUSIC currentMusic = MUSIC.NONE;
    
    [SerializeField]
    private AudioSource clipsAudioSource;
    [SerializeField]
    private AudioClip[] _audioClips;
    [SerializeField]
    private EffectProfile[] _effectProfiles;

    [Space(10f), SerializeField]
    private AudioMixer masterMixer;

    [SerializeField]
    private AudioMixerSnapshot[] musicSnapshots;

    

    //================================================================================================================//
    
    private void Start()
    {
    }
    
    //================================================================================================================//

    public void PlayMusic(MUSIC music)
    {
        if (currentMusic == music)
            return;

        switch (music)
        {
            case MUSIC.NONE:
                masterMixer.TransitionToSnapshots(musicSnapshots, new []{0f, 0f, 0f, 1f}, 2f);
                break;
            case MUSIC.JUNGLE:
                masterMixer.TransitionToSnapshots(musicSnapshots, new []{1f,0f,0f, 0f}, 2f);
                break;
            case MUSIC.DESERT:
                masterMixer.TransitionToSnapshots(musicSnapshots, new []{0f,1f,0f, 0f}, 2f);
                break;
            case MUSIC.MENU:
                masterMixer.TransitionToSnapshots(musicSnapshots, new []{0f,0f,1f, 0f}, 2f);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(music), music, null);
        }

        currentMusic = music;

    }

    //================================================================================================================//

    [ContextMenu("Walk Sound")]
    public void playTestWalk()
    {
        PlaySoundEffect(EFFECT.STEP);
    }

    public void PlaySoundEffect(EFFECT effect, float volumeScale = 1f)
    {
        var index = (int) effect;

        var pitchRange = _effectProfiles[index].pitchRange;

        masterMixer.SetFloat("EffectPitch", Random.Range(pitchRange.x, pitchRange.y));
        clipsAudioSource.PlayOneShot(_audioClips[index], volumeScale);
    }
    
    //================================================================================================================//


    /// <summary>
    /// Sets the master volume
    /// </summary>
    /// <param name="volume"></param>
    public void SetMasterVolume(float volume)
    {
        SetVolume("MasterVolume", volume);
    }
    public void SetMusicVolume(float volume)
    {
        SetVolume("MusicVolume", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        SetVolume("EffectVolume", volume);
    }

    private void SetVolume(string parameterName, float volume)
    {
        volume = Mathf.Clamp01(volume);
        masterMixer.SetFloat(parameterName, Mathf.Log10(volume) * 40);
    }
    
    //================================================================================================================//
}
