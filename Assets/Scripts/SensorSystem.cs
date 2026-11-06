using Unity;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SensorSystem : MonoBehaviour
{

    public float sensorDistance = 7f; // Parámetros del sensor, eventos, LayerMask, etc.
    public float sensorDistanceCrouched;
    public float sensorDistanceShoot;
    public float sensorDistanceP;

    public float sensorAngle = 360;
    public float sensorAngleCrouched;
    public float sensorAngleP;

    public LayerMask obstacle;
    private SphereCollider sensorTrigger;

    public event Action<Transform> OnPlayerDetected, OnPlayerLost;

    [SerializeField] private ParametreEnemyManager enemyManagerSO;


    private void Awake()                                                                                  // Radio del SphereCollider del sensor igual a la distancia para que no hayan problemas.
    {
        sensorDistanceCrouched = sensorDistance / 2;
        sensorDistanceShoot = sensorDistance * 2;
        sensorDistanceP = sensorDistance;

        sensorAngleCrouched = sensorAngle / 2;
        sensorAngleP = sensorAngle;

        sensorTrigger = GetComponent<SphereCollider>();
        sensorTrigger.radius = sensorDistance;
    }

    private void OnTriggerEnter(Collider other)                                                          // Cuando el jugador entre en el trigger, comprueba la distancia entre este y el enemigo. 
    {
        if (other.CompareTag("Player"))
        {
            CheckDetection(other);
        }
    }

    // private void OnTriggerStay (Collider other)                                                         
    // {
    //     if(other.CompareTag("Player"))
    //     {
    //         CheckDetection(other);
    //     }
    //     
    // }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("PERDIDO");
            OnPlayerLost?.Invoke(other.transform);
        }

    }

    private void CheckDetection(Collider other)                                                          // Si el Raycast desde el enemigo hasya el jugador se cruza con un objeto de LayerMask obstacle, se hace return.
    {
        Vector3 directionToTarget = other.transform.position - transform.position;

        if (Physics.Raycast(transform.position, directionToTarget, directionToTarget.magnitude, obstacle)) return;

        if (Vector3.Angle(directionToTarget, transform.forward) < sensorAngle / 2f)                       // De lo contrario, se comprueba el ángulo y se pasa a los estados de Patrol y Chase el Transform (posición) del jugador a través de dos eventos.
        {
            Debug.Log("VISTO");
            OnPlayerDetected?.Invoke(other.transform);
        }
        else
        {
            Debug.Log("PERDIDO");
            OnPlayerLost?.Invoke(other.transform);
        }
    }

    private void CrouchModifier()                                                                        // Si se agacha el jugador, y el enemigo no está muerto, disminuye la distancia y ángulo de detección. Si no está agachado, vuelve a la normalidad.
    {
        if (sensorTrigger != null)
        {
            sensorDistance = sensorDistanceCrouched;
            sensorAngle = sensorAngleCrouched;
            sensorTrigger.radius = sensorDistance;
        }

    }

    private void CancelCrouchModifier()
    {

        if (sensorTrigger != null)
        {
            sensorDistance = sensorDistanceP;
            sensorAngle = sensorAngleP;
            sensorTrigger.radius = sensorDistance;
        }


    }

    private void ShootModifier()
    {
        sensorDistance = sensorDistanceShoot;
        sensorTrigger.radius = sensorDistance;
    }

    private void OnEnable()                                                                              // Suscripción a los eventos correspondientes del enemyManagerSO que le permiten saber si el jugador se ha agachado o no.
    {
        enemyManagerSO.OnPlayerCrouched += CrouchModifier;
        enemyManagerSO.OnPlayerUnCrouched += CancelCrouchModifier;
        enemyManagerSO.OnPlayerShoot += ShootModifier;
    }

    private void OnDisable()                                                                              // Suscripción a los eventos correspondientes del enemyManagerSO que le permiten saber si el jugador se ha agachado o no.
    {
        enemyManagerSO.OnPlayerCrouched -= CrouchModifier;
        enemyManagerSO.OnPlayerUnCrouched -= CancelCrouchModifier;
        enemyManagerSO.OnPlayerShoot -= ShootModifier;
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
