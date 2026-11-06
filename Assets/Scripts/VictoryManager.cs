using UnityEngine;
using System;

[CreateAssetMenu(fileName = "VictoryManager", menuName = "Scriptable Objects/VictoryManager")]
public class VictoryManager : ScriptableObject
{
    public event Action OnVanAppear, OnPlayerVictory;                                       // Eventos para avisar a la furgoneta que aparezca y para avisar al jugador que ha ganado.

    

    public void VanAppear()                                                                 // Invocación de eventos.
    {
        OnVanAppear?.Invoke();
    }

    public void Victory()
    {
        OnPlayerVictory?.Invoke();
    }
}
