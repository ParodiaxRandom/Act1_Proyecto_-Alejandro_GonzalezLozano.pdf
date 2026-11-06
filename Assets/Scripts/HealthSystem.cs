using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{

    public float health;                                                                      // Se ajustan los parámetros sobre la vida del jugador, se acceden a los sonidos correspondientes del SO clase SoundCollection, y se pasa la salud mínima y máxima al script de la barra de vida para que pueda modificarla a través de un evento que pase 2 float.
    public float maxHealth;

    public FirstPersonController player;

    public event Action<float, float> OnPlayerLifeModified;

    [SerializeField] private SoundCollection soundSO;
    private AudioClip pain, die;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        health = maxHealth;
        player = GetComponent<FirstPersonController>();
        die = soundSO.diePlayer;
        pain = soundSO.painPlayer;
    }

    public void TakeDamage(float damage)                           // Si el jugador recibe daño, su vida se resta en base al daño pasado por el objeto que ha causado el daño. Si llega a 0 o menos, esta se pone en 0 y el jugador muere.
    {
        health -= damage;
        soundSO.PlaySound(pain);

        if (health <= 0)
        {
            health = 0;
            Debug.Log("MUELTO");
            soundSO.PlaySound(die);
            player.gameOverID = true;

            if(player.gameOverID)
            {
                player.GameOver();
            }
            
        }

        OnPlayerLifeModified?.Invoke(health, maxHealth);          // Actualización barra de vida a través de evento.
    }

    public void HealDamage(float damage)
    {
        health += damage;
       

        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        OnPlayerLifeModified?.Invoke(health, maxHealth);          // Actualización barra de vida a través de evento.
    }
}
