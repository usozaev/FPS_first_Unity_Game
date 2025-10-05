using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;

    public AudioClip bg_music;
    public AudioSource main_channel;


    string newGameScene = "SampleScene";
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        main_channel.PlayOneShot(bg_music);
        // Set high score text
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave  Survived: {highScore}";

    }

    public void StartNewGame()
    {
        main_channel.Stop();
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;


#else 
    Application.Quit();

#endif
    }
}
