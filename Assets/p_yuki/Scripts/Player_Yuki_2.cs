/*Cinemachineを使いましょう
 * CinemachineのBodyをTransorserに
 * BodyのBinding ModeをSimple Follow With World Upにしましょう。（これでカメラが良い感じに動く）
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Yuki_2 : MonoBehaviour
{
    [SerializeField] Transform m_camera;//Cinemachineのカメラ
    [SerializeField] float m_walkSpeed;//歩く速度
    [SerializeField] float m_runSpeed;//走る速度
    [SerializeField] float m_rotSpeed;//回転速度
    Rigidbody m_rigidBody;
    Transform m_transform;
    float m_Theta;//カメラからプレイヤーのベクトルとプレイヤーの移動方向とのベクトルのなす角
    float m_ThetaBefore;//theta計算用
    float m_ThetaAfter;//theta計算用
    Vector3 m_cameraToPlayer;//カメラからプレイヤーへの方向を表すベクトルの単位ベクトル

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = this.GetComponent<Rigidbody>();
        m_transform = this.transform;

        //移動に必要な変数を計算
        MovePrepare();
    }

    // Update is called once per frame
    void Update()
    {
        //方向キーを押せば動く
        TryMove();
    }

    private void FixedUpdate()
    {
    }

    //方向キーを押せば動く
    private void TryMove()
    {

        //移動方向の単位ベクトルをを計算する
        Vector3 moveDirection = Vector3.zero; //移動方向
        bool pushLeft = false;
        bool pushRight = false;

        //前
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection += Vector3.forward;
        }
        //右
        if (Input.GetKey(KeyCode.RightArrow))
        {
            pushRight = true;
            moveDirection += Vector3.right;
        }
        //左
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            pushLeft = true;
            moveDirection += Vector3.left;
        }
        //下
        if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection += Vector3.back;
        }
        //移動方向を単位ベクトルへ
        moveDirection = moveDirection.normalized;


        //ボタンを押していない場合はスキップ
        if (moveDirection.sqrMagnitude != 0)
        {
            //カメラからプレイヤーまでのベクトルを計算
            m_cameraToPlayer = this.transform.localPosition - m_camera.localPosition;
            m_cameraToPlayer.y = 0;
            m_cameraToPlayer = m_cameraToPlayer.normalized;


            m_ThetaAfter = Vector3.Angle(Vector3.forward, m_cameraToPlayer);
            float thetaSub;//前フレームと今フレームのthetaの差
            thetaSub = Mathf.Abs(m_ThetaBefore - m_ThetaAfter);
            //誤差を除去
            if (thetaSub < 0.05)
            {
                thetaSub = 0;
            }
            m_ThetaBefore = m_ThetaAfter;//今のafterは次のbefore

            if (pushLeft)
            {
                m_Theta -= thetaSub;//カメラからプレイヤーへのベクトルと移動方向とのなす角

            }
            if (pushRight)
            {
                m_Theta += thetaSub;//カメラからプレイヤーへのベクトルと移動方向とのなす角
            }


            Quaternion rotator;//進行方向のベクトルを回転させる
            rotator = Quaternion.Euler(0, m_Theta, 0);


            //進行方向を向く
            m_transform.LookAt(m_transform.localPosition + rotator * moveDirection);

            //動く
            m_transform.localPosition += rotator * moveDirection * m_walkSpeed * Time.deltaTime;


            //m_Thetaのオーバーフロー対策
            if (m_Theta > 360)
            {
                m_Theta -= 360;
            }
            if (m_Theta < -360)
            {
                m_Theta -= 360;
            }
        }
    }
    //移動に必要な変数の計算
    private void MovePrepare()
    {
        //カメラからプレイヤーまでのベクトルを計算
        m_cameraToPlayer = this.transform.localPosition - m_camera.localPosition;
        m_cameraToPlayer.y = 0;
        m_cameraToPlayer = m_cameraToPlayer.normalized;
        m_ThetaBefore = Vector3.Angle(Vector3.forward, m_cameraToPlayer);//プレイヤーの移動に使用
        m_Theta = m_ThetaBefore;
    }

}
