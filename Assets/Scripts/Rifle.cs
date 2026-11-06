using UnityEngine;
using System;

public class Rifle : Weapon
{
    [SerializeField] private float attackDistance = 500f;                                 // Parámetros del rifle (alcance, daño, número de balas, cooldown), etc).
    [SerializeField] private float attackDamage = 20f;
    [SerializeField] public int bullets = 30;
    [SerializeField] public int maxBullets = 100;
    [SerializeField] private float cooldown = 1.5f;
    [SerializeField] private float cooldownP;
    [SerializeField] private bool isRecharging;
    [SerializeField] private ParticleManager pManager;
    [SerializeField] private ParametreEnemyManager enemyManagerSO;
    [SerializeField] private FirstPersonController fPController;
    private int charger;
    public float cadence = 0.14f;
    public float cadenceP = 0.14f;

    [SerializeField] private bool shooting;

    private Camera cam;

    [SerializeField] private SoundCollection soundSO;
    private AudioClip rifleShot, rifleReload, rifleChange;

    public event Action<int> OnBulletSpent, OnReload;
   

    private bool rifleRecharging;
    private bool rifleShoot;

    void Awake()
    {
        DontDestroyOnLoad(this);
        cam = Camera.main;
        cooldownP = cooldown;
        cadenceP = cadence;
        rifleShot = soundSO.shootRifle;
        rifleReload = soundSO.rechargeRifle;
        rifleChange = soundSO.rifleChange;
        pManager = GetComponentInChildren<ParticleManager>();
        fPController = GetComponentInParent<FirstPersonController>();
        rifleShoot = false;
        shooting = false;
        charger = bullets;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()                                                           // Recarga del fusil va por bool (isRecharging).
    {

       

        if (isRecharging)
        {
            cooldown -= Time.deltaTime;

            if(cooldown <= 0)
            {
                cooldown = cooldownP;

                if(maxBullets >= charger)
                {
                    maxBullets -= charger;
                    bullets += charger;
                }
                else
                {
                    bullets += maxBullets;
                    maxBullets -= maxBullets;
                }

               
                isRecharging = false;
                OnBulletSpent?.Invoke(bullets);
                OnReload?.Invoke(maxBullets);
            }
        }

        if(rifleShoot)
        {
            cadence -= Time.deltaTime;
            if (cadence <= 0)
            {
                cadence = cadenceP;
                rifleShoot = false;
                enemyManagerSO.PlayerCrouch(false);
               
            }

        }

        if(shooting)
        {
            Use();
        }

        if (isActiveAndEnabled && !shooting)
        {
            shooting = false;
        }
    }

    void OnEnable()
    {
        fPController.OnPlayerShoot += Shooting;
        fPController.OnPlayerCancelShoot += Unshooting;
        shooting = false;
    }

    void OnDisable()
    {
        fPController.OnPlayerShoot += Shooting;
        fPController.OnPlayerCancelShoot -= Unshooting;
    }

    void Shooting()
    {
        shooting = true;
    }

    void Unshooting()
    {
        shooting = false;
    }
    
    public override void SoundChange()
    {
        soundSO.PlaySound(rifleChange);
    }

    public override void Use()                                                                 // Se dispara el rifle. Si no le quedan balas, recarga, y el método se detiene por return. De lo contrario, se invoca un raycast.
    {

                                  

        if (bullets <= 0)
        {
            bullets = 0;

            isRecharging = true;

            if(!rifleRecharging)
            {
                soundSO.PlaySound(rifleReload);
                rifleRecharging = true;
            }
        }

        if (isRecharging) { Debug.Log("RECARGANDO"); } if (isRecharging) return;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance) && !isRecharging && !rifleShoot)
        {
            bullets -= 1;
            soundSO.PlaySound(rifleShot);
            pManager.PlayParticle();
            enemyManagerSO.PlayerShoot();
            rifleShoot = true;
            OnBulletSpent?.Invoke(bullets);

            if(hit.transform.TryGetComponent(out EnemyBone bone))                           // Se comprueba si el objeto golpeado tiene el componente EnemyBone, y le pasa a ese componente el daño establecido en este script.
            {
                Debug.Log("Disparado");
                bone.TakeDamage(attackDamage);
                Debug.Log("DAÑACOBEIBI");
            }
        }
    }
}
