using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class PatrolState : State<EnemyController>
{
    public Transform patrolPath;                                                             // Setear parámetros del PatrolState.
    public List<Vector3> patrolPoints = new ();                                              // Se puede usar List<Vector3> para sacar todos los patrolpoints del patrolPath añadiendo sus posiciones a la lista.
    public float patrolSpeed = 2.0f;
    

    private float waitingTime = 1.5f;
    public int currentIndex;

    private static readonly int Speed = Animator.StringToHash("Speed");                      // Se setea el hash de Speed para que el animator pueda leer el valor pasado por este script.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }   

    public override void InitController(EnemyController controller)                          // InitController inicializa el controller basado en EnemyController (estados y EnemyController se colocan en el mismo objeto de enemigo). También permite añadir código que se ejecutará al empezar el nivel.
    {
        base.InitController(controller);

        foreach (Transform point in patrolPath)                                              // Se añaden todos los Transform a la lista de patrolPoints a través de su método Add() y un Foreach.
        {
            patrolPoints.Add(point.position);
        }

    }

    public override void OnEnter()                                                           // Código que se ejecuta al entrar en Patrol.
    {
        Debug.Log("ENTRANDO EN PATROL");
        StopAllCoroutines();
        controller.Agent.isStopped = false;
        controller.Agent.speed = patrolSpeed;
        controller.Agent.stoppingDistance = 0f;
        controller.soundSO.PlaySound(controller.grunt);
        StartCoroutine(PatrolAndWait());
    }

    
    private IEnumerator PatrolAndWait()                                                      // Corrutina que se ejecuta siempre (por while(true)).
    {
        while(true)
        {
            controller.Agent.SetDestination(patrolPoints[currentIndex]);

            yield return new WaitUntil(() => !controller.Agent.pathPending && controller.Agent.remainingDistance <= controller.Agent.stoppingDistance);

            yield return new WaitForSeconds(waitingTime);

            CalculateNewPoint();
        }
    }

    private void CalculateNewPoint()                                                         // Se calcula nuevo punto a patrullar.
    {
        currentIndex = (currentIndex + 1) % patrolPoints.Count;
        //controller.soundSO.PlaySound(controller.grunt);
    }

    public override void OnUpdate()                                                          // Se pasa a Speed la velocidad del controlador dividida entre su velocidad máxima, de modo que dependiendo de esta, el animator muestre una animación de Idle, Caminar o Correr.
    {
        controller.Animator.SetFloat(Speed, controller.Agent.velocity.magnitude / controller.MaximumSpeed);
        Debug.Log("PATROLLING");
    }

    public override void OnExit()
    {

    }

    private void StopPatrolling(Transform newTarget)                                         // El SensorSystem pasa en su evento OnPlayerDetected un Transform para indicar al enemigo dónde está el objetivo (el jugador) y a dónde tiene que ir en su estado de Chase. Por eso pasa un Transform.
    {
        StopAllCoroutines();
        controller.Agent.isStopped = true;
        controller.ChangeState(controller.chase);
    }

    private void OnEnable()                                                                  // Suscripción y desuscripción a eventos.
    {
       controller.Sensor.OnPlayerDetected += StopPatrolling;
    }

    private void OnDisable()
    {
        controller.Sensor.OnPlayerDetected -= StopPatrolling;
    }
}
