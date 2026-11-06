using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HUDManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hpText;                                    // Información sobre el texto de salud, la barra de vida y el healthsystem.
    [SerializeField] private Image healthBar;
    [SerializeField] private Image bulletGunIcon;
    [SerializeField] private Image bulletRifleIcon;
    [SerializeField] private Image aimIcon;

    [SerializeField] private TextMeshProUGUI gunbulletText;
    [SerializeField] private TextMeshProUGUI maxgunbulletText;
    [SerializeField] private TextMeshProUGUI riflebulletText;
    [SerializeField] private TextMeshProUGUI maxriflebulletText;
    [SerializeField] private TextMeshProUGUI victoryText;


    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private Rifle rifle;
    [SerializeField] private PlayerGun gun;
    [SerializeField] private FirstPersonController fPController;
    [SerializeField] private VictoryConditions vConditions;

    private bool aim;


    void Awake()
    {
        fPController = gameObject.GetComponentInParent<FirstPersonController>();
        hpText.text = healthSystem.maxHealth.ToString();
        riflebulletText.text = rifle.bullets.ToString();
        gunbulletText.text = gun.bullets.ToString();
        maxgunbulletText.text = gun.maxBullets.ToString();
        maxriflebulletText.text = rifle.maxBullets.ToString();
        aimIcon.gameObject.SetActive(false);
        vConditions = GameObject.Find("VictoryConditions").GetComponent<VictoryConditions>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(vConditions.timeToWin > 0)
        {
         victoryText.text = vConditions.timeToWin.ToString();
        }
        else
        {
            victoryText.gameObject.SetActive(false);
        }

        if(rifle.isActiveAndEnabled)
        {
            riflebulletText.gameObject.SetActive(true);
            maxriflebulletText.gameObject.SetActive(true);
            gunbulletText.gameObject.SetActive(false);
            maxgunbulletText.gameObject.SetActive(false);
            bulletGunIcon.gameObject.SetActive(false);
            bulletRifleIcon.gameObject.SetActive(true);

        }

        else if (gun.isActiveAndEnabled)
        {
            riflebulletText.gameObject.SetActive(false);
            maxriflebulletText.gameObject.SetActive(false);
            gunbulletText.gameObject.SetActive(true);
            maxgunbulletText.gameObject.SetActive(true);
            bulletGunIcon.gameObject.SetActive(true);
            bulletRifleIcon.gameObject.SetActive(false);
        }
    }
     
    void UpdateHealthBar(float health, float maxHealth)                                  // Actualización de la barra de vida a través del método y los float pasados por el evento del HealthSystem.
    {
        healthBar.fillAmount = health / maxHealth;
        hpText.text = healthSystem.health.ToString();
       
    }

    void UpdateRifleAmmo(int rifleAmmo)
    {
        riflebulletText.text = rifleAmmo.ToString();
        Debug.Log("GASTABALA");
    }

    void UpdateGunAmmo(int gunAmmo)
    {
        gunbulletText.text = gunAmmo.ToString();
        Debug.Log("GASTABALA");
    }

    void UpdateRifleMaxAmmo(int rifleMaxAmmo)
    {
        maxriflebulletText.text = rifleMaxAmmo.ToString();
    }

    void UpdateGunMaxAmmo(int gunMaxAmmo)
    {
        maxgunbulletText.text = gunMaxAmmo.ToString();
    }

    void PlayerAim(bool aim)
    {
        if (aim)
        {
            aimIcon.gameObject.SetActive(true);
            aim = true;
            
        }
        else if (!aim)
        {
            aimIcon.gameObject.SetActive(false);
            aim = false;
            
        }
    }

    private void OnEnable()                                                              // Suscripción y desuscripción al evento del healthsystem (OnEnable/OnDisable).
    {
        healthSystem.OnPlayerLifeModified += UpdateHealthBar;
        rifle.OnBulletSpent += UpdateRifleAmmo;
        gun.OnBulletSpent += UpdateGunAmmo;
        rifle.OnReload += UpdateRifleMaxAmmo;
        gun.OnReload += UpdateGunMaxAmmo;
        fPController.OnPlayerAim += PlayerAim;

    }

    private void OnDisable()
    {
        healthSystem.OnPlayerLifeModified -= UpdateHealthBar;
        rifle.OnBulletSpent -= UpdateRifleAmmo;
        gun.OnBulletSpent -= UpdateGunAmmo;
        rifle.OnReload -= UpdateRifleMaxAmmo;
        gun.OnReload -= UpdateGunMaxAmmo;
        fPController.OnPlayerAim -= PlayerAim;

    }
}
