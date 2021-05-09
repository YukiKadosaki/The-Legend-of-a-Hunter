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
    private float rotSpeed = 5;  //回転攻撃の速度

    private float currentTime = 0;
    private float MoveTime;
    private bool Switch, Switch1, Switch2, Switch3;
    private ParticleSystem FlameStream;
    [SerializeField] ParticleSystem particle, particle1;
    private Animator animator; 
    int index;
    float time, yzahyo;

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

        animator = GetComponent<Animator>();
        FlameStream = GetComponent<ParticleSystem>();

        //Invoke("Turn", 2f);
        StartCoroutine("Stop");  //コルーチン(Move)を実行
    }

    void Update()
    {
        //InvokeRepeating("Move", 2f, 10f);   //2秒後に関数Moveを実行し、3秒間隔で続ける
       
        //ドラゴンの座標調節
        yzahyo = EnemyPosition.y;
        //Debug.Log("zahyo = " + EnemyPosition.y);
        if(yzahyo < 1.0f)
        {
            EnemyPosition.y = 1.0f;
        }

        if (Switch == true && Switch1 == false && Switch2 == false && Switch3 == false)
        {
            //Debug.Log("Move Mode");
            PlayerPosition = player.transform.position;
            EnemyPosition = this.transform.position;
            distance = Vector3.Distance(PlayerPosition, transform.position);

            this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる
            animator.SetBool("walk", true);

            if (distance > 5.0)
            {
                float step = MoveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, PlayerPosition, step);
            }
            else
            {
                animator.SetBool("walk", false);
                Switch = false;
                Switch1 = true;
                Switch2 = false;
                Switch3 = false;
                //Invoke("Fire", 2f);
            }                
        }

        if(Switch == false && Switch1 == true && Switch2 == false && Switch3 == false)
        {
            //Debug.Log("Turn Mode");
            //StartCoroutine("Stop");  //コルーチン"Stop"を起動

            //Invoke("Turn");
            //StartCoroutine("Turn");

            index = Random.Range(0, 2); //0か1の乱数を生成
            time += Time.deltaTime;
            if (time >= 2 && time < 4)
            {
                transform.Rotate(new Vector3(0, this.rotSpeed, 0)); //回転させる
            }

            if (time >= 4)
            {
                time = 0;

                if (index == 0)
                {
                    Switch = false;
                    Switch1 = false;
                    Switch2 = true;
                    Switch3 = false;
                }
                else
                {
                    Switch = false;
                    Switch1 = false;
                    Switch2 = false;
                    Switch3 = true;
                }
                            
                
            }
        }

        if (Switch == false && Switch1 == false && Switch2 == true && Switch3 == false)
        {
            Debug.Log("Attack Mode");
            PlayerPosition = player.transform.position;
            EnemyPosition = this.transform.position;
            distance = Vector3.Distance(PlayerPosition, transform.position);

            //this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる

            //this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);

            time += Time.deltaTime;
            if (time >= 0 && time < 3)
            {
                this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる
                animator.SetBool("taiki", true);
            }

            if(time >= 3 && time < 5)
            {
                animator.SetBool("taiki", false);
                animator.SetBool("walk", true);
                this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
            }

            if (time >= 6)
            {
                animator.SetBool("walk", false);
                time = 0;
                Switch = true;
                Switch1 = false;
                Switch2 = false;
                Switch3 = false;
            }
        }

        if (Switch == false && Switch1 == false && Switch2 == false && Switch3 == true)
        {
            Debug.Log("Fire Mode");
            
            time += Time.deltaTime;
            if(time >= 0 && time < 3)
            {
                particle.Play();
                this.transform.LookAt(player.transform);  //オブジェクトをプレイヤーの位置に向かせる
                animator.SetBool("bless", true);
            }

            if(time >=3 && time < 8)
            {
                particle.Stop();                
                particle1.Play();
            }

            if (time >= 9)
            {
                time = 0;
                particle1.Stop();
                animator.SetBool("bless", false);
                Switch = true;
                Switch1 = false;
                Switch2 = false;
                Switch3 = false;
            }            
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

            if (distance <= 2.0)
            //if (Switch1 == false)
            {
                //Switch1 = false;

                CancelInvoke(); //実行しているInvoke(ここでは、Move)を停止              
                Invoke("Turn", 1f);
            }
        }      
        else{
            CancelInvoke(); //実行しているInvoke(ここでは、Move)を停止        
            Invoke("Turn", 2f);
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
