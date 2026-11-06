using UnityEngine;

public class RagdollSystem : MonoBehaviour
{
    public Rigidbody[] bones;                                                   // Se establece el array de Rigidbody bones.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bones = GetComponentsInChildren<Rigidbody>();                           // Se recogen todos los componentes hijos que tengan Rigidbody (el componente se pone justo en el padre de los huesos).
        UpdateBones(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateBones(bool state)                                         // Método que pone a los huesos en kinematic o no dependiendo de un estado que se pasará por bool.
    {
        foreach (var bone in bones)
        {
            bone.isKinematic = state;
        }
    }
}
