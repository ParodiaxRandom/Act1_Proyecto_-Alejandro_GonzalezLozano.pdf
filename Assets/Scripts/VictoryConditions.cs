using UnityEngine;
using System;

public class VictoryConditions : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;                                          // Se encuentran todos los enemigos necesarios para su contador. Se setean el resto de parámetros.
    [SerializeField] private int enemyCount;
    public float timeToWin;
    

    [SerializeField] VictoryManager victorySO;

    void Awake()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyCount = enemies.Length;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()                                                                            // 2 wincons distintas: derrotar a todos los enemigos o sobrevivir un tiempo. Si se coloca el timeToWin mayor a 0, este tiempo bajará. Si no, se queda igual, y la furgoneta no debería aparecer.
    {
        if (timeToWin > 0)
        {
            TimeCountDown();
        }
           
    }

    void TimeCountDown()
    {
       timeToWin -= Time.deltaTime;

       if (timeToWin <= 0)
       {
           VanAppear();
       }
        
    }

    public void EnemiesCountDown()                                                          // Si hay más de un enemigo, se ejecuta el código del método. Si el contador llega a 0, aparece la furgoneta. 
    {
        if (enemies != null && enemyCount > 0)
        {
            enemyCount -= 1;
            if (enemyCount <= 0)
            {
                enemyCount = 0;
                VanAppear();
            }
        }
       
    }

    private void VanAppear()
    {
        victorySO.VanAppear();                                                              // Método a partir del cual el victorySO invoca un evento para comunicar a la furgoneta que puede aparecer.
    }

    
}
