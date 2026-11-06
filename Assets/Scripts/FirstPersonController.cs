using Unity;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour                     // Parámetros necesarios para movimiento, detección de suelo, cámara, armas, elementos de HUD, sonidos, etc.
{
    [Header("Movement")] 
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementSpeedP;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravityScale;
    [SerializeField] private float movementSmoothTime;

    [Header("Ground Detection")]
    [SerializeField] private Transform feet;
    [SerializeField] private float detectionRadius;
    [SerializeField] private LayerMask whatIsGround;

    [Header("Croucher")]
    [SerializeField] private Transform eyes;
    [SerializeField] private Vector3 eyesM;

    private CharacterController controller;
    private Camera cam;

    public bool isGrounded;
    private Vector2 inputVector;
    private Vector3 verticalMovement;
    private Vector3 horizontalVelocity, horizontalVelocityRef;

    public PlayerInput PlayerInput;

    [SerializeField] private CinemachineInputAxisController cInput;

    public Animator animator;

    public PlayerGun gun;

    public event Action OnUseWeapon;
    public event Action<int> OnNewWeapon;
    public event Action OnNewWeaponButton;

    public event Action OnPlayerCrouched;

    public event Action OnPlayerInteract;

    public event Action OnPlayerShoot, OnPlayerCancelShoot;

    public event Action<bool> OnPlayerAim;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject victoryMenu;

    public bool pauseGameID;
    public bool gameOverID;
    

    public bool crouch;
    public bool run;
    public bool aim;

    [SerializeField] private VictoryManager victorySO;
    [SerializeField] private ParametreEnemyManager enemyManagerSO;
    [SerializeField] private SoundCollection soundSO;

    private AudioClip steps, jump, die;

    private void Awake()                                                                 // Se colocan todos los parámetros
    {

        movementSpeedP = movementSpeed;
        crouchSpeed = movementSpeed / 2;
        runSpeed = movementSpeed * 2;
        pauseGameID = false;
        controller = GetComponent<CharacterController>();
        PlayerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
        gun = GetComponentInChildren<PlayerGun>();
        pauseMenu = GameObject.Find("PauseMenu");
        gameOverMenu = GameObject.Find("GameOverMenu");
        victoryMenu = GameObject.Find("VictoryMenu");
        animator = gameObject.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        PlayerInput = GameObject.Find("InputController").GetComponent<PlayerInput>();
        cInput = GameObject.Find("CinemachineCamera").GetComponent<CinemachineInputAxisController>();
        
        eyesM = new Vector3(0, 2, 0);
       

        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(false);

        DontDestroyOnLoad(this);
     

        crouch = false;

        steps = soundSO.steps;
        die = soundSO.diePlayer;
        jump = soundSO.jump;
    }

    private void OnEnable()                                                 // Jugador se suscribe a los eventos del PlayerInput y del SO de sonido, vinculándolos a sus respectivos métodos.
    {
        PlayerInput.actions["Move"].canceled += UpdateMovement;
        PlayerInput.actions["Move"].performed += UpdateMovement;
        PlayerInput.actions["Jump"].started += Jump;
       

        PlayerInput.actions["Weapon 1"].started += ChangeWeapon;
        PlayerInput.actions["Weapon 2"].started += ChangeWeapon2;
        PlayerInput.actions["ChangeWeapon"].started += ChangeWeaponButton;

        PlayerInput.actions["Shoot"].started += UseWeapon;
        PlayerInput.actions["Shoot"].performed += UseWeapon;

        PlayerInput.actions["Shoot"].started += PlayerShoot;
        PlayerInput.actions["Shoot"].performed += PlayerShoot;
        PlayerInput.actions["Shoot"].canceled += PlayerShootCancel;

        PlayerInput.actions["Aim"].performed += PlayerAim;
        PlayerInput.actions["Aim"].canceled += PlayerAim;

        PlayerInput.actions["Pause"].started += PauseGame;

        PlayerInput.actions["Interact"].started += Interact;

        PlayerInput.actions["Run"].started += Run;
        PlayerInput.actions["Run"].canceled += Run;

        PlayerInput.actions["Crouch"].started += Crouch;

        victorySO.OnPlayerVictory += Victory;

    }

    private void OnDisable()                                               // Jugador se desuscribe a los eventos del PlayerInput y del SO de sonido, vinculándolos de sus respectivos métodos.
    {
        PlayerInput.actions["Jump"].started -= Jump;
        PlayerInput.actions["Move"].canceled -= UpdateMovement;
        PlayerInput.actions["Move"].performed -= UpdateMovement;

        PlayerInput.actions["Weapon 1"].started -= ChangeWeapon;
        PlayerInput.actions["Weapon 2"].started -= ChangeWeapon2;
        PlayerInput.actions["ChangeWeapon"].started -= ChangeWeaponButton;

        PlayerInput.actions["Shoot"].started -= UseWeapon;
        PlayerInput.actions["Shoot"].performed -= UseWeapon;

        PlayerInput.actions["Shoot"].started -= PlayerShoot;
        PlayerInput.actions["Shoot"].performed -= PlayerShoot;
        PlayerInput.actions["Shoot"].canceled -= PlayerShootCancel;

        PlayerInput.actions["Aim"].performed -= PlayerAim;
        PlayerInput.actions["Aim"].canceled -= PlayerAim;

        PlayerInput.actions["Pause"].started -= PauseGame;

        PlayerInput.actions["Interact"].started -= Interact;

        PlayerInput.actions["Run"].started -= Run;
        PlayerInput.actions["Move"].canceled -= Run;

        PlayerInput.actions["Crouch"].started -= Crouch;

        victorySO.OnPlayerVictory -= Victory;
    }

    private void UpdateMovement(InputAction.CallbackContext ctx)           // Pasar valores leídos de los inputs al animator para que el Blend Tree pueda cambiar de animaciones en base a dichos valores.
    {
        inputVector = ctx.ReadValue<Vector2>();
        animator.SetFloat("hInput", inputVector.x);
        animator.SetFloat("vInput", inputVector.y);
    }

    private void UseWeapon(InputAction.CallbackContext ctx)                // Invocar evento para que el Weapon Changer invoque el método "Use" del arma actual clase Weapon a través de su método "UseWeapon".
    {
        
        OnUseWeapon?.Invoke();
    }
    
    private void Jump(InputAction.CallbackContext ctx)                     // Salto (usa isGrounded del charactercontroller).
    {
        if (isGrounded && !crouch)
        {
            soundSO.PlaySound(jump);
            animator.SetBool("jump", true);
            verticalMovement.y = Mathf.Sqrt(-2 * gravityScale * jumpHeight);
        }
    }

    public void ChangeWeapon(InputAction.CallbackContext ctx)              // El jugador invoca evento para pasar valor del index del arma correspondiente al Weapon Changer.
    {
        OnNewWeapon?.Invoke(0);
        Debug.Log("GUN");
    }


    public void ChangeWeapon2(InputAction.CallbackContext ctx)
    {
        OnNewWeapon?.Invoke(1);
        Debug.Log("RIFLE");
    }

    public void ChangeWeaponButton(InputAction.CallbackContext ctx)
    {
        OnNewWeaponButton?.Invoke();
        Debug.Log("RIFLE");
    }

    public void Interact(InputAction.CallbackContext ctx)                   // El jugador invoca el evento OnPlayerInteract para que el PlayerInteractionSystem pueda invocar el método Interact() del GameObject actual con clase IInteractuable.
    {
        OnPlayerInteract?.Invoke();
    }

    public void PauseGame(InputAction.CallbackContext ctx)                  // Se bloquea/desbloquea el cursor, y se usa Time.timeScale para modificar la velocidad del juego.
    {
        if (!pauseGameID)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            // PlayerInput.SwitchCurrentActionMap("UI");
            Time.timeScale = 0;
            pauseGameID = true;
        }

        else
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            // PlayerInput.SwitchCurrentActionMap("Gameplay");
            Time.timeScale = 1;
            pauseGameID = false;
        }
      
    }

    public void Crouch(InputAction.CallbackContext ctx)                     // Cuando el jugador se agacha, notifica a los enemigos que deben reducir su radio y distancia de visión a través de un EnemyManager, el cual es un SO.
    {
        if(!crouch && isGrounded)
        {
            crouch = true;
            animator.SetBool("crouch", true);
            movementSpeed = crouchSpeed;
            enemyManagerSO.PlayerCrouch(true);
            eyes.position -= eyesM;
        }
        else if (crouch && isGrounded)
        {
            crouch = false;
            animator.SetBool("crouch", false);
            movementSpeed = movementSpeedP;
            enemyManagerSO.PlayerCrouch(false);
            eyes.position += eyesM;
        }

        OnPlayerCrouched?.Invoke();
        
    }

    public void Run(InputAction.CallbackContext ctx)                        // Al correr/dejar de correr, su velocidad se multiplica/divide entre 2.
    {
        
        if(!run && !crouch /*&& isGrounded*/)
        {
            run = true;
            movementSpeed = runSpeed;
            Debug.Log("CORRE");
        }
        else if (run && !crouch/*&& isGrounded*/)
        {
            run = false;
            movementSpeed = movementSpeedP;
            Debug.Log("NO CORRAS");
        }
       
        
    }

    private void PlayerAim(InputAction.CallbackContext ctx)
    {
        if (!aim)
        {
            aim = true;
            cam.fieldOfView = 40;
        }

        else if (aim)
        {
            aim = false;
            cam.fieldOfView = 60;
        }
       
        OnPlayerAim?.Invoke(aim);
    }

    public void GameOver()                                                   // Se pasa al animator el trigger "die" al morir.
    {
        if (gameOverID)
        {
            animator.SetTrigger("die");
            this.enabled = false;
            PlayerInput.enabled = false;
            cInput.enabled = false;
            eyes.GetComponent<Rigidbody>().isKinematic = false;
            eyes.GetComponent<BoxCollider>().isTrigger = false;
            StartCoroutine("GameOverM");

           
        }
    }

    private void Victory()                                                   // Al completar un nivel, aparece el menú de victoria.
    {
        this.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        victoryMenu.SetActive(true);
        Time.timeScale = 0;
    }

    private IEnumerator GameOverM()                                          // Pasados 3 segundos desde la muerte del jugador, se pasa al menú del GameOver.
    {
        yield return new WaitForSeconds(3f);
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        PlayerInput.enabled = true;
        Time.timeScale = 0;
        gameOverID = false;
    }

    public void PauseButton()                                                // Este método es usado para el botón de "Continue" del menú de pausa, pues el método PauseGame se llama a través de un InputAction, por lo que no es accesible desde el botón de la UI.
    {
        if (!pauseGameID)
        {
            pauseMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            // PlayerInput.SwitchCurrentActionMap("UI");
            Time.timeScale = 0;
            pauseGameID = true;
        }

        else
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            // PlayerInput.SwitchCurrentActionMap("Gameplay");
            Time.timeScale = 1;
            pauseGameID = false;
        }
    }

    private void OnStep()                                                    // Evento de animación que hace sonar pasos cuando la animación del personaje lo hace tocar el suelo.
    {
        soundSO.PlaySound(steps);
    }

    private void PlayerShoot(InputAction.CallbackContext ctx)
    {
        OnPlayerShoot?.Invoke();
    }

    private void PlayerShootCancel(InputAction.CallbackContext ctx)
    {
        OnPlayerCancelShoot?.Invoke();
    }

    void Update()                                                            // Los scripts de movimiento, rotación, salto y comprobación de suelo siempre estarán ejecutándose.
    {
        GroundCheck();
        ApplyGravity();
        MoveAndRotate();

        if(Input.GetMouseButtonDown(0))
        {
            OnUseWeapon?.Invoke();
        }

        if (Input.GetMouseButton(1))
        {

            aim = true;
            cam.fieldOfView = 40;
            OnPlayerAim?.Invoke(aim);
        }


        else if (Input.GetMouseButtonUp(1))
        {
            aim = false;
            cam.fieldOfView = 60;
            OnPlayerAim?.Invoke(aim);
        }

            
        

        if (Input.GetMouseButton(0))
        {
            OnPlayerShoot?.Invoke();
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnPlayerCancelShoot?.Invoke();
        }

       

    }

    private void MoveAndRotate()                                             // Debido a que usamos CharacterController, este usa el método controller.Move para moverse.
    {
        transform.rotation = Quaternion.Euler(0, cam.transform.eulerAngles.y, 0);

        Vector3 targetVelocity = transform.TransformDirection(inputVector.x, 0, inputVector.y) * movementSpeed;
        horizontalVelocity = Vector3.SmoothDamp(horizontalVelocity, targetVelocity,
            ref horizontalVelocityRef, movementSmoothTime);

        controller.Move((horizontalVelocity + verticalMovement) * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (isGrounded && verticalMovement.y < 0)
        {
            verticalMovement.y = -2f;
            animator.SetBool("jump", false);
        }
        else 
        {   
           verticalMovement.y += gravityScale * Time.deltaTime;    
        }
    }

    private void GroundCheck()                                                               // GroundCheck() usa una esfera creada por Physics.CheckSphere, la cual se basa en un Transform que se coloca a la altura de los pies, en un radio de detección y en un LayerMask que se coloca en el suelo. El resultado del chequeo determina el isGrounded
    {
        isGrounded = Physics.CheckSphere(feet.position, detectionRadius, whatIsGround);
    }

    private void OnDrawGizmos()                                                              // Permite comprobar la esfera que detecta el suelo.
    {
        if (feet != null)
        {
            Gizmos.DrawSphere(feet.position, detectionRadius);
        }
    }
}
