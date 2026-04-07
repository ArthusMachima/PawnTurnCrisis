using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource Music;
    public AudioSource SoundFX;
    public AudioSource Ambience;
    public AudioSource SubAmbience;

    [Header("Clips")]
    public AudioClip m_ChromaticRose;
    public AudioClip m_ChromaticRose_wapred;
    public AudioClip s_GunShot;
    public AudioClip s_CritGunShot;
    public AudioClip s_CylinderTurn;
    public AudioClip s_ReloadGun;
    public AudioClip s_OpenBorder;
    public AudioClip s_CloseBorder;
    public AudioClip s_GlassBreak;
    public AudioClip a_Static;
    public AudioClip a_VinylCrack;
    public AudioClip s_EnemyStep;
    public AudioClip s_EnemyBash;
    public AudioClip s_ParryAttempt;
    public AudioClip s_Parried;
    public AudioClip s_MenuAppear;
    public AudioClip s_MenuClick;
    public AudioClip s_MenuSelect;
    public AudioClip s_Explosion;
    public AudioClip s_ClearStage;
    public AudioClip s_FailedStage;
    public AudioClip s_NoBullets;
    public AudioClip s_Ult;
    public AudioClip s_UltExplosion;
    public AudioClip s_DisplayGoodMessage;
    public AudioClip s_DisplayBadMessage;
    public AudioClip s_UltShot;

    Coroutine PlayingMusic;

    public void PlaySound(AudioSource source, AudioClip clip)
    {
        GameObject soundGameObject = new GameObject("TempAudio");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();

        // Configure the AudioSource
        audioSource.clip = clip;
        audioSource.Play();
        if (source == Ambience || source == SubAmbience)
        {
            audioSource.loop = true;
        }

        Destroy(soundGameObject, clip.length);
    }

    public void DoPlayMusic(AudioClip startingclip, AudioClip loopingclip)
    {
        PlayingMusic = StartCoroutine(PlayMusic(startingclip, loopingclip));
    }

    public void StopMusic()
    {
        StopCoroutine(PlayingMusic);
        PlayingMusic = null;
        Music.Stop();
    }

    IEnumerator PlayMusic(AudioClip startingclip, AudioClip loopingclip)
    {
        Music.Stop();
        Music.clip = startingclip;
        Music.Play();
        while (Music.isPlaying)
        {
            yield return null;
        }
        Music.clip = loopingclip;
        Music.loop = true;
        Music.Play();
    }

    public void DoLerpPitch(AudioSource audioSource, float targetPitch, float duration)
    {
        StartCoroutine(LerpPitch(audioSource, targetPitch, duration));
    }

    private IEnumerator LerpPitch(AudioSource audioSource, float targetPitch, float duration)
    {
        float startPitch = audioSource.pitch;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (audioSource == null) yield break;

            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            audioSource.pitch = Mathf.Lerp(startPitch, targetPitch, t);
            yield return null;
        }

        if (audioSource != null)
        {
            audioSource.pitch = targetPitch;
        }
    }
}
