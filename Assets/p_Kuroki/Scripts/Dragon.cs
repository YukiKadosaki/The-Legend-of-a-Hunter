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
    private float rotSpeed = 20;  //回転攻撃の速度

    private float currentTime = 0;
    private float MoveTime;
    private bool Switch, Switch1, Switch2, Switch3;
    private ParticleSystem FlameStream;

    void Start()
    {             
        //自機のオブジェクトを見つける
        player = GameObject.Find("PlayerSample");
        Debug.Log("target = " + player.name);

        PlayerPosition = player.transform.position;
        EnemyPosition = this.transform.position;

        Switch = true;
        Switch1 = false;
        Switch2 = false;
        Switch3 = false;

        FlameStream = GetComponent<ParticleSystem>();

        //Invoke("Turn", 2f);
        StartCoroutine("Stop");  //コルーチン(Move)を実行
    }

    void Update()
    {
        //InvokeRepeating("Move", 2f, 10f);   //2秒後に関数Moveを実行し、3秒間隔で続ける
        //Rigidbody rb = this.transform.GetComponent<Rigidbody>();
        //Invoke("Turn", 2f);

        //if(Switch == true)
        //{
            //EnemyPosition += new Vector3(0.1f, 0f, 0f);
        //}

        if (Switch == true && Switch1 == false)
        {
            Debug.Log("Move Mode");
            PlayerPosition = player.transform.position;
            EnemyPosition = this.transform.position;
            distance = Vector3.Distance(PlayerPosition, transform.position);

            this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる

            if (distance > 2.0)
            {
                float step = MoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, step);
            }
            else
            {
                Switch = false;
                Switch1 = false;
                Switch2 = false;
                Switch3 = true;
                //Invoke("Fire", 2f);
            }                
        }

        if(Switch == false && Switch1 == true)
        {
            //Debug.Log("Turn Mode");
            //distance = 0;
            //StartCoroutine("Stop");  //コルーチン"Stop"を起動

            //Invoke("Turn");
            //StartCoroutine("Turn");
            //currentTime = 0;
            if(currentTime <= 10.0)
            {
                Debug.Log("Time = " + currentTime);
                transform.Rotate(new Vector3(0, this.rotSpeed, 0)); //回転させる
                currentTime++;
            }
            else
            {
                currentTime = 0;
                Switch = true;
                Switch1 = false;
            }

        }

        if (Switch == false && Switch1 == false && Switch2 == true)
        {
            Debug.Log("Attack Mode");
            PlayerPosition = player.transform.position;
            EnemyPosition = this.transform.position;
            distance = Vector3.Distance(PlayerPosition, transform.position);

            //this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる

            this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

            /*if (distance > 2.0)
            {
                float step = MoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, step);
            }
            else
            {
                Switch = true;
                Switch1 = false;
                Switch2 = false;
                //Invoke("Fire", 2f);
            }*/
        }

        if (Switch == false && Switch1 == false && Switch2 == false && Switch3 == true)
        {
            Debug.Log("Fire Mode");
            //PlayerPosition = player.transform.position;
            //EnemyPosition = this.transform.position;
            //distance = Vector3.Distance(PlayerPosition, transform.position);

            //this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる

            this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

            /*if (distance > 2.0)
            {
                float step = MoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, step);
            }
            else
            {
                Switch = true;
                Switch1 = false;
                Switch2 = false;
                //Invoke("Fire", 2f);
            }*/
        }
        //StartCoroutine("Move");
        //}
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
       
        //yield return new WaitForSeconds(2.0f);
        StartCoroutine("Stop");  //コルーチン"Stop"を起動
        
        if (distance > 2.0) 
        {
            float step = MoveSpeed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, step);
            //Switch1 = true;

            if (distance <= 2.0)
            //if (Switch1 == false)
            {
                //Switch1 = false;

                CancelInvoke(); //実行しているInvoke(ここでは、Move)を停止              
                Invoke("Turn", 1f);
                //Turn();
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


    IEnumerator Turn()
    //void Turn()
    {
        Debug.Log("Turn Mode");
        distance = 0;     
        StartCoroutine("Stop");  //コルーチン"Stop"を起動
        //for(currentTime = 0; currentTime <= 5; currentTime++)
        //for(MoveTime = 0; MoveTime <= 10; MoveTime += Time.deltaTime)
        
        MoveTime = 0;

        if(MoveTime <= 100)
        {
            transform.Rotate(new Vector3(0, this.rotSpeed, 0)); //回転させる
            MoveTime++;
            //transform.Rotate(new Vector3(0, this.rotSpeed*Time.deltaTime, 0));
        }
        else
        {
            MoveTime = 0;
            Switch = true;
            Switch1 = false;
            //Invoke("Update");
            yield break;
        }
        //currentTime = 0;
        //MoveTime = 0;
        //Switch = true;
        //Switch1 = false;
        //Invoke("Update");
        //CancelInvoke();
        //StartCoroutine("Stop");
        //StartCoroutine("Attack");
        //yield break;
        //Attack();
        //}
    }

    //IEnumerator Attack()
    void Attack()
    {
        Debug.Log("Attack Mode");
        this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる
        //for (currentTime = 0; currentTime <= 5; currentTime++)
        for (MoveTime = 0f; MoveTime <= 5f; MoveTime += Time.deltaTime)
        {
            Switch = true;
            //Vector3 velocity = this.transform.rotation * new Vector3(MoveSpeed, 0, 0);
            //this.transform.position += velocity * Time.deltaTime;
            //transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            //transform.position += new Vector3(0.1f, 0f, 0f);
        }

        //currentTime = 0;
        MoveTime = 0;
        Switch = false;
        //StartCoroutine("Move");
        //yield break;
        CancelInvoke();
        Move();
    }

    void Fire()
    {
        this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる

        for (MoveTime = 0f; MoveTime <= 10f; MoveTime += Time.deltaTime)
        {
            FlameStream.Play(); //パーティクル(FlameStream)を実行
        }

        FlameStream.Stop(); //パーティクル(FlameStream)を停止
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
