using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chase : State<EnemyController>
{

    public float chaseSpeed = 3.5f;                                                                                                 // Parámetros principales (velocidad de caza, distancia a la que se va a detener el agente).
    public float chaseStoppingDistance = 6f;

    private bool soundPlayed;

    private static readonly int Speed = Animator.StringToHash("Speed");
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*controller.Animator.SetFloat(Speed, controller.Agent.velocity.magnitude / controller.MaximumSpeed);       // El agente persigue al jugador, y se ajusta el float "Speed" del animator para que el blend tree pueda ajustar las animaciones. 

        if (Vector3.Distance(transform.position, controller.Target.position) > chaseStoppingDistance)
        {
            controller.Agent.SetDestination(controller.Target.position);
        }
        else
        {
            controller.ChangeState(controller.attack);                                                             // Si el enemigo está lo suficientemente cerca del jugador, este pasará a su estado de ataque.
        }*/
    }

    public override void InitController(EnemyController controller)
    {
        base.InitController(controller);
        controller.MaximumSpeed = chaseSpeed;
    }

    public override void OnEnter()                                         // Al entrar en este estado, deja de detenerse al agente y se ajustan los parámetros de velocidad y distancia de parada del agente.
    {
        Debug.Log("ENTRANDO EN CHASE");
        StopAllCoroutines();
        controller.Agent.isStopped = false;
        controller.Agent.speed = chaseSpeed;
        controller.Agent.stoppingDistance = chaseStoppingDistance;
        soundPlayed = false;
        controller.soundSO.PlaySound(controller.grunt);
        if (!soundPlayed) { soundPlayed = true; }
    }

    public void UpdateTarget(Transform newTarget)                          // Al detectar al jugador, el sensor le pasa el transform del objeto golpeado (en este caso, el jugador) al enemigo, para que lo persiga.
    {
        controller.Target = newTarget;
    }

    public override void OnUpdate()
    {
        Debug.Log("CHASEANDO");
        controller.Animator.SetFloat(Speed, controller.Agent.velocity.magnitude / controller.MaximumSpeed);       // El agente persigue al jugador, y se ajusta el float "Speed" del animator para que el blend tree pueda ajustar las animaciones. 

        if (Vector3.Distance(transform.position, controller.Target.position) > chaseStoppingDistance)
        {
            controller.Agent.SetDestination(controller.Target.position);
        }
        else
        {
            controller.ChangeState(controller.attack);                                                             // Si el enemigo está lo suficientemente cerca del jugador, este pasará a su estado de ataque.
        }
            
    }

    public override void OnExit()
    {
        soundPlayed = false;
    }

    private void PlayerHasBeenLost(Transform newTarget)
    {
        StartCoroutine("PlayerLost");
        controller.Agent.speed = 2f;
    }

    public void OnEnable()                                                                                        // Suscripción y desuscripción a los eventos del sensor. Esta es la manera de saber cuándo el jugador ha sido detectado, y cuándo no.
    {
        controller.Sensor.OnPlayerDetected += UpdateTarget;
        controller.Sensor.OnPlayerLost += PlayerHasBeenLost;
    }

    public void OnDisable()
    {
        controller.Sensor.OnPlayerDetected -= UpdateTarget;
        controller.Sensor.OnPlayerLost -= PlayerHasBeenLost;                                                    
    }

    private IEnumerator PlayerLost()                                                                               // Al irse el jugador, después de 5 segundos, el enemigo volverá a su estado de patrulla.
    {
        yield return new WaitForSeconds(5f);
        controller.ChangeState(controller.patrol);
    }

   
}
