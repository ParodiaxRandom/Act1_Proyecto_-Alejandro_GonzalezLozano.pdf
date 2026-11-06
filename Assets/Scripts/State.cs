using UnityEngine;

public abstract class State<T> : MonoBehaviour where T : FSMController<T>
{
    protected T controller;

    /*Cada estado reconocerá su controllador, que gracias a los genéricos, se sabe que es de tipo T. En el caso del enemigo creado, este es EnemyController*/

    public virtual void InitController(T controller)
    {
        this.controller = controller;
    }

    /*Esta es la base para los estados de la máquina de estados. Esta se basa en una clase genérica "T", 
     la cual sirve para que los estados generados con esta clase sean controlados por un FSMController génerico de tipo T, concretamente de tipo EnemyController.*/

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void OnEnter();

    public abstract void OnUpdate();

    public abstract void OnExit();
}
