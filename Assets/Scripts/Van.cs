using UnityEngine;

public class Van : MonoBehaviour, IInteractuable
{
    public GameObject interactIcon;
    

    public VictoryManager victorySO;

    public Transform locationToAppear;

    [SerializeField] private SoundCollection soundSO;
    private AudioClip vanAppear;

    private void Awake()
    {
        interactIcon.SetActive(false);
        vanAppear = soundSO.vanAppear;
    }

    public void OnInteractuableActivated()
    {
        interactIcon.SetActive(true);
    }

    public void OnInteractuableDeactivated()
    {
        interactIcon.SetActive(false);
    }

    public void Interact()                                                               // Invoca al método Victory del SO tipo VictoryManager, haciendo que el jugador supere el nivel. El jugador gana al interactuar con el camión.
    {
        victorySO.Victory();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void VanAppearing()                                                                   // Método para que la furgoneta aparezca en un determinado Transform que se coloca en el editor.
    {
        if(locationToAppear != null)
        {
            soundSO.PlaySound(vanAppear);
            transform.position = locationToAppear.position;
        }
    }

    private void OnEnable()                                                               // Suscripción y desuscripción a eventos correspondientes.
    {
        victorySO.OnVanAppear += VanAppearing;
    }

    private void OnDisable()
    {
        victorySO.OnVanAppear -= VanAppearing;
    }
}
