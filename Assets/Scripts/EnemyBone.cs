using UnityEngine;

public class EnemyBone : MonoBehaviour
{

    public EnemyController enemyController;                                                         // Se ajusta el multiplicador de daño del hueso correspondiente
    public float damageMult;

    public bool hasTakenDamage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)                                                            // El enemigo recibe daño sobre ese multiplicador. El daño resultante se pasa al enemycontroller a través de su método OnDamage.
    {
        damage *= damageMult;
        Debug.Log(damage);
        enemyController.OnDamage(damage);
    }
}
