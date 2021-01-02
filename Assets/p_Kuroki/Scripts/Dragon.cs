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
    private float rotSpeed = 60;  //回転攻撃の速度
    //private float targetTime = 1.0f;
    private float currentTime;

    void Start()
    {
         
        //StartCoroutine(MoveLiner(dest));  //コルーチン(MoveLiner)を実行
               
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
        //distance = Vector3.Distance(PlayerPosition, transform.position);
        Invoke("Move", 2f);
        //StartCoroutine("Move");
        //}
        //yield break;  //コルーチン終了
    }

    //IEnumerator Move()
    void Move()
    {
        PlayerPosition = player.transform.position;
        EnemyPosition = this.transform.position;
        distance = Vector3.Distance(PlayerPosition, transform.position);
        //Debug.Log("distance = " + distance);
        this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる
        StartCoroutine("Stop");  //コルーチン"Stop"を起動
        
        if (distance > 2.0) 
        {
            float step = MoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, step);
            if(distance <= 2.0)
            {
                CancelInvoke(); //実行しているInvoke(ここでは、Move)を停止              
                Invoke("Turn", 1f);
                //StartCoroutine("Turn");
                //yield break;
            }
        }      
        else{
            CancelInvoke(); //実行しているInvoke(ここでは、Move)を停止        
            Invoke("Turn", 2f);
            //StartCoroutine("Turn");
            //yield break;
        }

    }


    //IEnumerator Turn()
    void Turn()
    {
        Debug.Log("Turn Mode");
        distance = 0;     
        //StartCoroutine("Stop");  //コルーチン"Stop"を起動
        for(currentTime = 0; currentTime <= 5; currentTime++)
        {
            transform.Rotate(new Vector3(0, 0, this.rotSpeed)); //回転させる
            //currentTime += Time.deltaTime;
            Debug.Log("Turn" + currentTime);
        }
        //if(currentTime <= 10)
        //{
            
        //}
        //else
        //{
        currentTime = 0;
        CancelInvoke();
        StartCoroutine("Stop");
        Move();
        //}
        
        //CancelInvoke();
        //Invoke("Move", 10f);
        //StartCoroutine("Move");
        //yield break;
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(2);  //2秒間処理を止める
        yield break; 
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
