using UnityEngine;
using UnityEngine.Pool;
using System;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public ObjectPool<Bullet> myPool { get; set; }                                        // Preparamos parámetros (entre ellos, el tiempo que tarda en desactivarse, el daño); se coge la pool en la que va a estar.
    public float impulseForce = 150f;

    public float cooldown = 3f;

    public float damage = 50f;

    public GameObject BulletPrefab;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)                                                       // Al chocar con un obstáculo, se desactiva la bala a través del release de su object pool.
    {
        if (other.CompareTag("Obstacle"))
        {
            myPool.Release(this);
        }
    }

    private void OnCollisionEnter(Collision other)                                                    // Si choca contra el enemigo, recibe el daño correspondiente a la parte en donde le ha dado.
    {
        if(other.transform.TryGetComponent(out EnemyBone bone))
        {
            bone.TakeDamage(damage);
            myPool.Release(this);
            Debug.Log("DISPARO PISTOLA");
        }
    }

    private void OnEnable()                                                                           // Al activarse, se lanza la bala hacia adelante.
    {
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        GetComponent<Rigidbody>().AddForce(cam.transform.forward * impulseForce, ForceMode.Impulse);
        StartCoroutine(Cooldown());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Cooldown()                                                                     // Se desactiva la bala al pasar un tiempo determinado desde que ha sido lanzada.
    {
        yield return new WaitForSeconds(cooldown);
        myPool.Release(this);
    }
}
