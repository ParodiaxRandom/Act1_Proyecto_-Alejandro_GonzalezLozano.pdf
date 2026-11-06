using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // La clase abstracta Weapon contiene los métodos que todos los scripts que hereden de esta deben poseer.

    
    public abstract void Use();
    public abstract void SoundChange();
}
