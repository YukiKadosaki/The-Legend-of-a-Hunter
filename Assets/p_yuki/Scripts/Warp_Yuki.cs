using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp_Yuki : MonoBehaviour
{
    private enum BossNameEnum
    {
        Bee,
        Dragon,
        Dhurahan,
        Robot,
        Other
    }

    [Header("移動先のシーン名")]
    [SerializeField] private string m_NextStage;
    [Header("ボス名")]
    [SerializeField] private BossNameEnum m_BossName;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //未撃破ならtrue
            if (GetCanWarp(m_BossName))
            {
                SceneManager.LoadScene(m_NextStage);
            }
        }
    }

    private bool GetCanWarp(BossNameEnum name)
    {
        int data = 0;

        switch (name)
        {
            case BossNameEnum.Bee:
                data = PlayerPrefs.GetInt("Bee");
                break;
            case BossNameEnum.Dragon:
                data = PlayerPrefs.GetInt("Dragon");
                break;
            case BossNameEnum.Dhurahan:
                data = PlayerPrefs.GetInt("Dhurahan");
                break;
            case BossNameEnum.Robot:
                data = PlayerPrefs.GetInt("Robot");
                break;
        }

        //1以外ならボス未撃破、1ならボス撃破済み
        return data != 1;
    }
}
