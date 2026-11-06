using UnityEngine;

public class SandCastle : MonoBehaviour, IInteractuable
{
    public GameObject interactIcon;                                                                   // Parámetros necesarios.
    public GameObject botiquinBombaPrefab;

    [SerializeField] private SoundCollection soundSO;
    private AudioClip sandCastle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        sandCastle = soundSO.sandCastle;
    }

    public void OnInteractuableActivated()
    {
        interactIcon.SetActive(true);
    }

    public void OnInteractuableDeactivated()
    {
        interactIcon.SetActive(false);
    }

    public void Interact()                                                                            // Al interactuar, spawnea un botiquín bomba.
    {
        Debug.Log("DESTRUIDO");
        soundSO.PlaySound(sandCastle);
        Instantiate(botiquinBombaPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
