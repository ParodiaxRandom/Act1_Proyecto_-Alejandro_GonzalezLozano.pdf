using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "SceneManagerSO", menuName = "Scriptable Objects/SceneManagerSO")]
public class SceneManagerSO : ScriptableObject
{

    public void RestartScene()                                             // ScriptableObject que se encarga de reiniciar el nivel o salir del juego a través de los botones de la UI. Por eso los métodos en public.
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
