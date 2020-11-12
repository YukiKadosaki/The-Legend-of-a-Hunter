using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Boss
{
    [SerializeField] private Vector3 dest;

    private GameObject player; //プレイヤーオブジェクト
    private Vector3 PlayerPosition; //プレイヤーの位置情報
    private Vector3 EnemyPosition; //敵の位置情報
    private float targetTime = 1.0f;
    private float currentTime = 0;

    void Start()
    {
        //Debug.Log("Start");   
        //StartCoroutine(MoveLiner(dest));  //コルーチン(MoveLiner)を実行
        //Debug.Log("End");
        
        //自機のオブジェクトを見つける
        player = GameObject.Find("Player");

        PlayerPosition = player.transform.position;
        EnemyPosition = transform.position;
    }

    void Update()
    {
        while(Hp > 0)
        {
            currentTime += Time.deltaTime;
            if (targetTime < currentTime)
            {
                currentTime = 0;
            }
        }
        //yield break;  //コルーチン終了
    }

    //目的地(destination）に直線移動する
    public override IEnumerator MoveLiner(Vector3 destination)
    {
        yield return null;
    }

    //目的地（destination）に障害物などを避けながら移動する 
    public override IEnumerator MoveToDestination (Vector3 destination)
    {
        yield return null; //1フレーム停止
    }
}
