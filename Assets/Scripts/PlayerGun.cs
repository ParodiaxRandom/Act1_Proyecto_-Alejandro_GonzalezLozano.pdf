using UnityEngine;
using System;
using UnityEngine.Pool;                                                                          // Para usar las Pools.
using UnityEngine.InputSystem;

public class PlayerGun : Weapon
{
    [SerializeField] private Bullet bulletPrefab;                                                // Parámetros de spawneo, desactivación y recuperación de las balas, la pool (en este caso de clase Bullet), si se está recargando, etc.
    [SerializeField] private Transform spawnPoint;
  
    [SerializeField] public int bullets = 5;
    private int charger;
    [SerializeField] public int maxBullets = 10;
    [SerializeField] private float cooldown = 3f;
    private float cooldownP;
    public float cadence = 0.6f;
    public float cadenceP = 0.6f;

    private ObjectPool<Bullet> bulletPool;

    [SerializeField] private bool isRecharging;
    [SerializeField] private bool gunShoot;

    [SerializeField] private SoundCollection soundSO;
    [SerializeField] private AudioClip gunShot, gunReload, gunChange;

    public bool rechargingSound;

    [SerializeField] private ParticleManager pManager;
    [SerializeField] private ParametreEnemyManager enemyManagerSO;

    public event Action<int> OnBulletSpent, OnReload;
   

    private void Awake()                                                                         // Al empezar, crea una object pool, el cual necesita de al menos estos 3 métodos.
    {
        bulletPool = new ObjectPool<Bullet>(OnCreateBullet, OnGetBullet, OnReleaseBullet);
        gunShot = soundSO.shootGun;
        gunReload = soundSO.reloadGun;
        gunChange = soundSO.gunChange;
        rechargingSound = false;
        gunShoot = false;
        cadenceP = cadence;
        pManager = GetComponentInChildren<ParticleManager>();
        maxBullets = 10;
        charger = bullets;
        cooldownP = cooldown;
    }

    private Bullet OnCreateBullet()                                                              // Cuando se crea una bala, se instancia una copia en la punta del arma y se indica a dicha copia que pertenece a la bulletPool de este script.
    {
        Bullet bulletCopy = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        bulletCopy.myPool = bulletPool;
        return bulletCopy;
        Debug.Log("CREADO");
    }

    private void OnGetBullet(Bullet bulletCopy)                                                  // Al pillar una bala, se reinicia su velocidad y se spawnea en la punta del arma.
    {
        bulletCopy.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        bulletCopy.transform.position = spawnPoint.transform.position;
        bulletCopy.transform.rotation = spawnPoint.transform.rotation;
        bulletCopy.gameObject.SetActive(true);
        Debug.Log("COGIDO");
    }

    private void OnReleaseBullet(Bullet bulletCopy)                                              // Cuando la bala termine de cumplir su función, esta se desactiva y pasa a estar "reciclada".
    {
        bulletCopy.gameObject.SetActive(false);
        Debug.Log("RECICLADO");
    }

    public override void Use()                                           // Si tiene menos de 0 balas, el jugador recarga la pistola y usa un return para parar el código. De lo contrario, dispara.
    {

        
       

        if(bullets <= 0)
        {
            bullets = 0;
            isRecharging = true;

            if(!rechargingSound)
            {
                soundSO.PlaySound(gunReload);
                rechargingSound = true;
            }
           

        }

        if (isRecharging) { Debug.Log("RECARGANDO"); }

        if (isRecharging) return;

        Debug.Log("PIU");

        if (!isRecharging && !gunShoot)
        {
            bullets -= 1;
            bulletPool.Get();                                            // El método Get() se encarga de pillar una bala recogida o instanciar una en el caso de que no hayan.
            soundSO.PlaySound(gunShot);
            pManager.PlayParticle();
            enemyManagerSO.PlayerShoot();
            gunShoot = true;
            OnBulletSpent?.Invoke(bullets);
        }
        
    }

    public override void SoundChange()
    {
        soundSO.PlaySound(gunChange);
    }

    void Update()                                                        // Recarga de la pistola.
    {

        
        if (isRecharging)
        {
            cooldown -= Time.deltaTime;
            

            if (cooldown <= 0)
            {
                cooldown = cooldownP;

                if (maxBullets >= charger)
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

        if (gunShoot)
        {
            cadence -= Time.deltaTime;
            if (cadence <= 0)
            {
                cadence = cadenceP;
                gunShoot = false;
                enemyManagerSO.PlayerCrouch(false);
            }

        }
    }
}
