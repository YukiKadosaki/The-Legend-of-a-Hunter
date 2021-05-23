using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameYuki : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }
    public void NewGame()
    {
        PlayerPrefs.SetInt("Bee", 0);
        PlayerPrefs.SetInt("Dragon", 0);
        PlayerPrefs.SetInt("Dhurahan", 0);
        PlayerPrefs.SetInt("Robot", 0);

        PlayerPrefs.Save();

        SceneManager.LoadScene("Hub");
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Hub");
    }
}
