using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    PlayerInput pInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pInput = GetComponent<PlayerInput>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
