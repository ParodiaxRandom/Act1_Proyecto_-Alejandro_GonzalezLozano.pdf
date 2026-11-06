using UnityEngine;

public interface IInteractuable
{
    // Una interfaz contiene métodos que los scripts que la incorporan debe usar. Este uso es parecido al de la herencia, pero permite que los scripts sean más independientes. Son como una expansión a lo que los scripts normales son capaces de hacer.

    void OnInteractuableActivated();

    void OnInteractuableDeactivated();

    void Interact();
}
