//プレイヤーの子オブジェクトに付ける
//壁に触れているかどうかを判定する

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WallChecker : MonoBehaviour
{
    [SerializeField]
    private bool IsLeft;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (IsLeft)
            {
                player.TouchingWallLeft = true;
            }
            else
            {
                player.TouchingWallRight = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            if (IsLeft)
            {
                player.TouchingWallLeft = false;
            }
            else
            {
                player.TouchingWallRight = false;
            }
        }
    }
}
