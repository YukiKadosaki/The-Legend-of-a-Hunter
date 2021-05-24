﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveManager : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha0))
        {
            SceneManager.LoadScene("Title");
        }
        else if (Input.GetKey(KeyCode.Alpha1))
        {
            SceneManager.LoadScene("Hub");
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SceneManager.LoadScene("Scene_Dan");
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SceneManager.LoadScene("Scene_Hato");
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            SceneManager.LoadScene("Scene_Kuroki");
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            SceneManager.LoadScene("TrueDullahanScene");
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            SceneManager.LoadScene("Scene_Dan");
        }

    }
}
