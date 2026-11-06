using UnityEngine;
using UnityEngine.InputSystem;

public class BotiquinBomb : MonoBehaviour, IInteractuable
{

    public float interactID;                                                                               // Parámetros del botiquín bomba (icono de interacción, probabilidad de que explote o cure (50/50), sonido, etc).
    public FirstPersonController player;
    public GameObject interactIcon;

    public float bombProbability;
    public float healProbability;

    public float BHP;

    public float explosionRadius;
    public float explosionDamage;

    public float healDamage;

    [SerializeField] private SoundCollection soundSO;
    private AudioClip bomb, heal;
    
    void Awake()                                                                                           // Se setean parámetros.
    {
        player = GameObject.Find("PlayerBody").GetComponent<FirstPersonController>();
        bombProbability = 50;
        healProbability = 50;

        explosionRadius = 50f;
        explosionDamage = 50f;

        healDamage = 50f;

        bomb = soundSO.bomb;
        heal = soundSO.heal;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteractuableActivated()                                                                  // Este método se activa cuando el jugador se acerca al objeto, gracias a la interfaz "IInteractuable".
    {
        interactIcon.SetActive(true);
    }

    public void OnInteractuableDeactivated()                                                                // Este se activa cuando se aleja del objeto.
    {
        interactIcon.SetActive(false);
    }

    public void Interact()                                                                                  // Al interactuar con el objeto, este daña a los enemigos y al jugador, o cura al jugador.
    {
        BHP = Random.Range(0, 100);

        if (BHP < bombProbability)                                                                
        {
            soundSO.PlaySound(bomb);
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider col in cols)
            {
                if (col.TryGetComponent(out HealthSystem health))
                {
                    health.TakeDamage(explosionDamage);
                }

                else if (col.TryGetComponent(out EnemyBone eBone))                                         // EnemyBone es un componente que se coloca a cada hueso del enemigo, cuya función es proporcionar un multiplicador de daño depenediendo de por dónde se golpee.
                {
                    eBone.TakeDamage(explosionDamage);

                    if(eBone.hasTakenDamage)
                    {
                        return;
                    }
                }
            }
        }
        else
        {
            soundSO.PlaySound(heal);
            Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider col in cols)
            {
                col.GetComponent<HealthSystem>()?.HealDamage(healDamage);
            }
        }

        Destroy(gameObject);
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }
}
