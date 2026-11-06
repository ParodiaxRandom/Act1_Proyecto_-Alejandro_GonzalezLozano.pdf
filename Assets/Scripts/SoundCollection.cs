using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SoundCollection", menuName = "Scriptable Objects/SoundCollection")]
public class SoundCollection : ScriptableObject
{
    public AudioClip steps;                                                                 // Este SO contiene los SFX usados para el juego, y este pasa los que tiene que usar el SoundManager a través de un evento con AudioClip.
    public AudioClip jump;
    public AudioClip shootGun;
    public AudioClip reloadGun;
    public AudioClip shootRifle;
    public AudioClip rechargeRifle;
    public AudioClip sandCastle;

    public AudioClip diePlayer;
    public AudioClip painPlayer;

    public AudioClip painMonster;
    public AudioClip dieMonster;
    public AudioClip attackMonster;
    public AudioClip monsterPlayerDetected;

    public AudioClip bomb;
    public AudioClip heal;

    public AudioClip vanAppear;

    public AudioClip gunChange;
    public AudioClip rifleChange;


    public event Action<AudioClip> OnPlaySound;

    public void PlaySound(AudioClip clip)
    {
        OnPlaySound?.Invoke(clip);
    }
}
