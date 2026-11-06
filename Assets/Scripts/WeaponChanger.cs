using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponChanger : MonoBehaviour
{
    private int currentIndex;
    public Weapon[] weapons;
    public Weapon currentWeapon;

    public FirstPersonController fPController;

    private void Awake()
    {
        currentIndex = 0;
        currentWeapon = weapons[currentIndex];

        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
        
        weapons[0].gameObject.SetActive(true);
        
    }

    private void OnEnable()                                  // Suscripción y desuscripción de eventos de armas del FirstPersonController (el jugador).
    {
        fPController.OnNewWeapon += ChangeWeapon;
        fPController.OnUseWeapon += UseWeapon;
        fPController.OnNewWeaponButton += ChangeWeaponButton;
       
    }

    private void OnDisable()
    {
        fPController.OnNewWeapon -= ChangeWeapon;
        fPController.OnUseWeapon -= UseWeapon;
        fPController.OnNewWeaponButton -= ChangeWeaponButton;
       
    }

    private void UseWeapon()                                  // UseWeapon se encarga de invocar el método Use() del arma que el jugador lleva actualmente.
    {
        currentWeapon.Use();
    }

    private void ChangeWeapon(int index)                      // ChangeWeapon se encarga de cambiar el arma del jugador según un index que el jugador le pasa a través del evento "OnNewWeapon".
    {
        currentWeapon.gameObject.SetActive(false);

        currentIndex = index;
        currentWeapon = weapons[currentIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.SoundChange();
    }

    private void ChangeWeaponButton()                      // ChangeWeapon se encarga de cambiar el arma del jugador según un index que el jugador le pasa a través del evento "OnNewWeapon".
    {
        currentWeapon.gameObject.SetActive(false);

        if(currentIndex == weapons.Length - 1)
        {
            currentIndex = 0;
        }

        else
        {
            currentIndex = currentIndex + 1;
        }
        
        currentWeapon = weapons[currentIndex];
        currentWeapon.gameObject.SetActive(true);
        currentWeapon.SoundChange();
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
