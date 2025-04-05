using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu1 : MonoBehaviour
{
   public void PlayGame()
{
    Debug.Log("Button clicked");
    SceneManager.LoadSceneAsync(1);
}

}
