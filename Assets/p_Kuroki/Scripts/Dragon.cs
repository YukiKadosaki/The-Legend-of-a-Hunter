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
    private float MoveTime;

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
        //Rigidbody rb = this.transform.GetComponent<Rigidbody>();
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

        Debug.Log("Move Mode");

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
        //for(currentTime = 0; currentTime <= 5; currentTime++)
        for(MoveTime = 0f; MoveTime <= 10f; MoveTime += Time.deltaTime)
        {
            transform.Rotate(new Vector3(0, 0, this.rotSpeed)); //回転させる
            //Debug.Log("Turn" + currentTime);
            Debug.Log("Turn" + MoveTime);
        }

        //currentTime = 0;
        MoveTime = 0;
        CancelInvoke();
        StartCoroutine("Stop");
        Attack();
        //Move();
        //}
    }

    void Attack()
    {
        Debug.Log("Attack Mode");
        this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる
        //for (currentTime = 0; currentTime <= 5; currentTime++)
        for (MoveTime = 0f; MoveTime <= 2f; MoveTime += Time.deltaTime)
        {
            //Vector3 velocity = this.transform.rotation * new Vector3(MoveSpeed, 0, 0);
            //this.transform.position += velocity * Time.deltaTime;
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }

        //currentTime = 0;
        MoveTime = 0;
        CancelInvoke();
        Move();
    }

    //void Fire()
    //{

    //}

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
