using UnityEngine;

public class EnemyController : FSMController<EnemyController>
{
    public PatrolState patrol { get; private set; }    // La T de FSMController<T> se sustituye por "EnemyController" para crear una máquina de estados para el enemigo.

    public Chase chase { get; private set; }           // Se pillan las instancias de los estados, los cuales heredan de la clase State<T>. Gracias a los genéricos, se asegura que los estados usen el EnemyController y no algo más genérico como FSMController.

    public Attack attack { get; private set; }

    public Death death { get; private set; }

    public SensorSystem Sensor;                        // Se colocan aquí los parámetros compartidos entre varios estados (Ej: target entre estado de patrulla y persecución)
    public UnityEngine.AI.NavMeshAgent Agent;
    public Transform Target;
    public Animator Animator;
    public float MaximumSpeed;
    [SerializeField] ParticleManager pManager;

    public float currentHealth;
    public float maxHealth;

    public RagdollSystem RagdollSystem;

    public bool dead;

    [SerializeField] public SoundCollection soundSO;
    public AudioClip grunt, painMonster, dieMonster;
   

    public VictoryConditions victoryConditions;

    void Awake()                                       // Se pillan todos los componentes y scripts que el EnemyController debe manejar.
    {
        Animator = GetComponent<Animator>();
        Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        RagdollSystem = GetComponent<RagdollSystem>();
        Target = FindAnyObjectByType<FirstPersonController>().transform;

        patrol = GetComponent<PatrolState>();
        chase = GetComponent<Chase>();
        attack = GetComponent<Attack>();
        death = GetComponent<Death>();

        patrol.InitController(this);        
        chase.InitController(this);     
        attack.InitController(this);
        death.InitController(this);

        ChangeState(patrol);

        currentHealth = maxHealth;

        grunt = soundSO.monsterPlayerDetected;
        painMonster = soundSO.painMonster;
        dieMonster = soundSO.dieMonster;

        pManager = GetComponentInChildren<ParticleManager>();
    }

    public void OnDamage(float damage)                // El enemigo recibe el daño en base a qué hueso ha recibido el daño. Si su salud llega a 0 o menos, este muere.
    {
        currentHealth -= damage;
        soundSO.PlaySound(painMonster);
        pManager.PlayParticle();

        if (currentHealth < 0)
        {
            currentHealth = 0;
            soundSO.PlaySound(dieMonster);
            Die();
        }
    }

    private void Die()                             // Al morir, se desactiva todos sus estados y el agente, y se cambia a la animación de muerte. Este reduce en 1 el contador de enemigos necesarios para que aparezca la furgoneta.
    {

        if (dead) return;

        Animator.SetBool("die", true);
        patrol.enabled = false;
        chase.enabled = false;
        attack.enabled = false;
        Animator.enabled = false;
        Agent.enabled = false;

        RagdollSystem.UpdateBones(false);
        victoryConditions.EnemiesCountDown();

        Destroy(gameObject, 4f);

        dead = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
