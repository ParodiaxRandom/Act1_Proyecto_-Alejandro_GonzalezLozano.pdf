using UnityEngine;

public class FSMController<T> : MonoBehaviour where T : FSMController<T>
{
    private State<T> currentState;
    // La clase FSMController cuenta con un genérico tipo "T", de tal forma que se puedan generar varias máquinas de estados en forma de scripts sustituyendo en cada una de ellas la T. 
    void Start()
    {
        
    }

    // Esta máquina se encarga de controlar los estados, haciéndolos actualizarse y cambiando entre ellos de ser necesario con el método "ChangeState", saliendo de un estado, cambiando de estado y haciendo que entre.
    protected virtual void Update()
    {
        if(currentState)
        {
            currentState.OnUpdate();
        }
    }

    public void ChangeState(State<T> newState)
    {
        if(currentState)
        {
            currentState.OnExit();
        }

        currentState = newState;
        currentState.OnEnter();
        
    }
}
