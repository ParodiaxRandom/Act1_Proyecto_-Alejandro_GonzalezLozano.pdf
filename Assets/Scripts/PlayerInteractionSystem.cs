using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    public FirstPersonController fPController;
    private IInteractuable currentInteractuable;                                       // El objeto a interactuar debe heredar la interfaz IInteractuable.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        fPController.OnPlayerInteract += LaunchInteraction;                            // Se encarga de ejecutar el método Interact del objeto que el jugador tenga en frente cuando pulsa la tecla de interactuar a través del método LaunchInteraction.
    }

    void OnDisable()
    {
        fPController.OnPlayerInteract -= LaunchInteraction;
    }

    private void LaunchInteraction()
    {
        currentInteractuable?.Interact();
        currentInteractuable?.OnInteractuableDeactivated();
        currentInteractuable = null;
    }

    private void OnTriggerEnter(Collider other)                                        // Al entrar en el trigger de un objeto interactuable, este primero desactiva el icono del interactuable anterior, y luego hace que el interactuable actual pase a ser el usado por el PlayerInteractionSystem, activando su icono.
    {
        if(other.TryGetComponent(out IInteractuable interactuable))
        {
            if(interactuable != currentInteractuable)
            {
                currentInteractuable?.OnInteractuableDeactivated();
            }
            currentInteractuable = interactuable;
            interactuable.OnInteractuableActivated();
        }
    }

    private void OnTriggerExit(Collider other)                                         // Al salir del trigger de un interactuable, este desactiva su icono (OnInteractuableDeactivated).
    {
        if (other.TryGetComponent(out IInteractuable interactuable))
        {
            interactuable.OnInteractuableDeactivated();

            if (interactuable == currentInteractuable)
            {
                currentInteractuable = null;
            }
           
           
        }
    }
}
