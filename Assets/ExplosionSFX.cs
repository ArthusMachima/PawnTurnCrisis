using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSFX : MonoBehaviour
{
    [SerializeField] private AudioManager aud;

    private void Start()
    {
        aud = FindAnyObjectByType(typeof(AudioManager)) as AudioManager;
        aud.PlaySound(aud.SoundFX, aud.s_Explosion);
    }
}
