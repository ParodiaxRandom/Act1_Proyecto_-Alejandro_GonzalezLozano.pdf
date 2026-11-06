using UnityEngine;
using System;
using System.Collections;

[CreateAssetMenu(fileName = "ParametreEnemyManager", menuName = "Scriptable Objects/ParametreEnemyManager")]
public class ParametreEnemyManager : ScriptableObject
{

   
    public event Action OnPlayerCrouched, OnPlayerUnCrouched, OnPlayerShoot;
   

    public void PlayerCrouch(bool crouchID)
    {
        if (crouchID) { OnPlayerCrouched?.Invoke(); } else { OnPlayerUnCrouched?.Invoke(); }            // ParametreEnemyManager avisa a los enemigos para que reduzcan su rádio y ángulo de visión cuando el jugador se agacha a través de estos eventos.
    }

    public void PlayerShoot()
    {
        OnPlayerShoot?.Invoke();
    }
}
