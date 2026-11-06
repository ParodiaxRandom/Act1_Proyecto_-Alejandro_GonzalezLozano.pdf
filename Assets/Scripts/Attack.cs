using UnityEngine;

public class Attack : State<EnemyController>
{

    private static readonly int attack = Animator.StringToHash("attack");
    private static readonly int hAttack = Animator.StringToHash("hAttack");

    [SerializeField] private float attackDistance = 6f;                       // Parámetros del primer ataque del enemigo.
    [SerializeField] private float attackRadius = 50f;
    [SerializeField] private float attackDamage = 25f;

                                                                              
    [SerializeField] private float attackHRadius = 50f;                       // Parámetros del ataque pesado del enemigo.
    [SerializeField] private float attackHDamage = 50f;
    [SerializeField] private float probabilityattackHDamage = 25f;

    public float probability;
    

    public GameObject attackPoint;                                            // Se establece el attackpoint en el inspector.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
        
    }

    public override void InitController (EnemyController controller)                                          // Se identifica el controlador de la clase base (en este caso, EnemyController).
    {
        base.InitController(controller);
    }

    public override void OnEnter()
    {
        StopAllCoroutines();
        Debug.Log("ENTRANDO EN ATTACK");
        probability = Random.Range(0, 100);

        if (probability < probabilityattackHDamage)
        {
            controller.Animator.SetBool(hAttack, true);
            controller.Animator.SetBool(attack, false);
         
        }
        else if (probability >= probabilityattackHDamage)
        {
            controller.Animator.SetBool(attack, true);
            controller.Animator.SetBool(hAttack, false);
        }

    }

    public override void OnUpdate()
    {
        Debug.Log("ATTACKING");
        controller.Agent.isStopped = true;                                                                    // Paramos al agente, activamos la animación de ataque y aseguramos que vaya rotando hacie donde se mueva el jugador.
        

        Vector3 directionToTarget = (controller.Target.position - transform.position).normalized;
        directionToTarget.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = lookRotation;
    }

    public override void OnExit()
    {

    }

    private void OnAttack()
    {
        
       Debug.Log("LIGHT");
           
           
       Collider[] cols = Physics.OverlapSphere(attackPoint.transform.position, attackRadius);                // Se llama por evento de animación (en el frame en el que se ejecuta el ataque). Al atacar, se comprueba si ha golpeado al jugador (a través de HealthSystem), y se le aplica el daño correspondiente.

       foreach (Collider col in cols)
       {
        col.GetComponent<HealthSystem>()?.TakeDamage(attackDamage);
       }
        
       
    }

    private void OnHeavyAttack()
    {
        
      Debug.Log("HEAVY");

      Collider[] cols = Physics.OverlapSphere(attackPoint.transform.position, attackHRadius);                // Se llama por evento de animación (en el frame en el que se ejecuta el ataque). Al atacar, se comprueba si ha golpeado al jugador (a través de HealthSystem), y se le aplica el daño correspondiente.

      foreach (Collider col in cols)
      {
         col.GetComponent<HealthSystem>()?.TakeDamage(attackHDamage);
      }
       

    }

    private void OnAttackFinished()                                                                           // Se llama por evento de animación (cuando acaba el ataque). Al acabar, se desactiva la animación de ataque, el agente deja de detenerse y cambia al estado de Chase.
    {
        probability = Random.Range(0, 100);

        if (probability < probabilityattackHDamage)
        {
            controller.Animator.SetBool(hAttack, true);
            controller.Animator.SetBool(attack, false);
            
        }
        else if (probability >= probabilityattackHDamage)
        {
            controller.Animator.SetBool(attack, true);
            controller.Animator.SetBool(hAttack, false);
            
        }


        if (Vector3.Distance(transform.position, controller.Target.position) > attackDistance)
        {
            Debug.Log("SE ACABO");
            
            controller.Animator.SetBool(attack, false);
            controller.Animator.SetBool(hAttack, false);
            controller.Agent.isStopped = false;
            controller.ChangeState(controller.chase);
        }
    }

    private void OnHeavyAttackFinished()                                                                           // Se llama por evento de animación (cuando acaba el ataque). Al acabar, se desactiva la animación de ataque, el agente deja de detenerse y cambia al estado de Chase.
    {
        probability = Random.Range(0, 100);

        if (probability < probabilityattackHDamage)
        {
            controller.Animator.SetBool(hAttack, true);
            controller.Animator.SetBool(attack, false);
           
        }
        else if (probability >= probabilityattackHDamage)
        {
            controller.Animator.SetBool(attack, true);
            controller.Animator.SetBool(hAttack, false);
        }


        if (Vector3.Distance(transform.position, controller.Target.position) > attackDistance)
        {
            Debug.Log("SE ACABO");
         
            controller.Animator.SetBool(attack, false);
            controller.Animator.SetBool(hAttack, false);
            controller.Agent.isStopped = false;
            controller.ChangeState(controller.chase);
        }
    }
}
