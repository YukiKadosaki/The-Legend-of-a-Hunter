using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Yuki : MonoBehaviour
{
    [SerializeField] Camera m_camera;//Cinemachineのカメラ
    [SerializeField] float m_walkSpeed;//歩く速度
    [SerializeField] float m_runSpeed;//走る速度
    Rigidbody m_rigidBody;
    float m_MoveFront;//GetAxisで手に入れる前へ進む速度の割合(0~1）
    float m_MoveSide;//GetAxisで手に入れる横へ進む速度の割合(0~1)
    float theta;//CmaeraToPlayerとMoveDirectionのなす角
    Vector3 m_MoveDirection;//相対的な（カメラからプレイヤーへのベクトルを基準とした)移動方向
    Vector3 m_MoveDirectionAbs;//絶対座標での移動方向
    Vector3 m_cameraToPlayer;//カメラからプレイヤーへの方向を表すベクトルの単位ベクトル

    // Start is called before the first frame update
    void Start()
    {
        m_rigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動方向の単位ベクトルをを計算する
        m_MoveFront = Input.GetAxis("Vertical");
        m_MoveSide = Input.GetAxis("Horizontal");
        m_MoveDirection = new Vector3(m_MoveSide, 0, m_MoveFront).normalized;

        m_MoveDirectionAbs = Vector3.zero;
        //ボタンを押していない場合はスキップ
        if (m_MoveDirection.sqrMagnitude != 0)
        {
            //カメラからプレイヤーまでのベクトルを計算
            m_cameraToPlayer = this.transform.localPosition - m_camera.transform.localPosition;
            m_cameraToPlayer.y = 0;
            m_cameraToPlayer = m_cameraToPlayer.normalized;

            //CmaeraToPlayerとMoveDirectionの内積からなす角を求める
            theta = Vector3.Angle(m_cameraToPlayer, m_MoveDirection);

            m_MoveDirectionAbs = Quaternion.Euler(0, theta, 0) * m_cameraToPlayer;

            Debug.Log("m_MoveDirection:" + m_MoveDirection);
            Debug.Log("m_cameraToPlayer:" + m_cameraToPlayer);
            Debug.Log("m_MoveDirectionAbs:" + m_MoveDirectionAbs);
            Debug.Log("theta:" + theta);
        }
    }

    private void FixedUpdate()
    {
        m_rigidBody.AddForce(m_MoveDirectionAbs * m_walkSpeed);
    }

    
}
