using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // handle buttons
    public void Begin()
    {
       SceneManager.LoadScene("Scenes/_Scene_0"); 
    }

    public void Return()
    {
        SceneManager.LoadScene("Scenes/Main Menu");
    }

    public void ExitGame()
    {
      Debug.Log("Game is exiting");
      Application.Quit();
    }
}
