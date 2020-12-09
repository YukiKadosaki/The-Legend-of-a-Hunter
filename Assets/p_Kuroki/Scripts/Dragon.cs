using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : Boss
{
    [SerializeField] private Vector3 dest;

    private GameObject player; //プレイヤーオブジェクト
    private Vector3 PlayerPosition; //プレイヤーの位置情報
    private Vector3 EnemyPosition; //敵の位置情報
    private float distance;  //プレイヤーと敵の距離
    private float targetTime = 1.0f;
    private float currentTime = 0;

    void Start()
    {
        //Debug.Log("Start");   
        //StartCoroutine(MoveLiner(dest));  //コルーチン(MoveLiner)を実行
        //Debug.Log("End");
        
        //自機のオブジェクトを見つける
        player = GameObject.Find("PlayerSample");
        Debug.Log("target = " + player.name);

        PlayerPosition = player.transform.position;
        EnemyPosition = this.transform.position;       
    }

    void Update()
    {
        //while(Hp > 0)
        //{

        //InvokeRepeating("Move", 2f, 10f);   //2秒後に関数Moveを実行し、3秒間隔で続ける
        distance = Vector3.Distance(PlayerPosition, transform.position);
        Invoke("Move", 2f);
        //}
        //yield break;  //コルーチン終了
    }

    void Move()
    {
        this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる
        StartCoroutine("Stop");  //コルーチン"Stop"を起動

        if (distance > 2.0) 
        {
            float step = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, step);
        }      
        else{
            Invoke("Turn", 2f);
        }
        
    }

    void Turn()
    {
        //自機のオブジェクトを見つける
        
        ///PlayerPosition = player.transform.position;
        //EnemyPosition = this.transform.position;
       
        Invoke("Move", 10f);
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(2);  //2秒間処理を止める
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
