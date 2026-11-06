using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    [SerializeField] private ParticleSystem particula;                            // Componente que se coloca a cada sistema de partículas dentro de sus respectivos objetos. 

    void Awake()
    {
        particula = gameObject.GetComponent<ParticleSystem>();
        particula.Stop();
    }

    public void PlayParticle()
    {
        particula.Play();
        Debug.Log("PARTICULA");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
