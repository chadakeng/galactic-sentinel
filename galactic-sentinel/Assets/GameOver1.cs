using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {

    public void Setup() {
        gameObject.SetActive(true);
    }

    public void RestartButton() {
        SceneManager.LoadScene(1);  // Load scene with index 1
    }

    public void ExitButton() {
        SceneManager.LoadScene(0);  // Load scene with index 0 (main menu)
    }
}