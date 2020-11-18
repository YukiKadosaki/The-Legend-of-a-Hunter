using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Yuki : MonoBehaviour
{
    [SerializeField] Transform m_camera;//Cinemachineのカメラ
    [SerializeField] float m_walkSpeed;//歩く速度
    [SerializeField] float m_runSpeed;//走る速度
    [SerializeField] float m_rotSpeed;//回転速度
    Rigidbody m_rigidBody;
    float m_MoveFront;//GetAxisで手に入れる前へ進む速度の割合(0~1）
    float m_MoveSide;//GetAxisで手に入れる横へ進む速度の割合(0~1)
    float m_rotRadius;//回転半径
    Vector3 m_PlayerPosition;//極座標でのプレイヤーの座標
    Vector3 m_rotCenter;//回転中心
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
            m_cameraToPlayer = this.transform.localPosition - m_camera.localPosition;
            m_cameraToPlayer.y = 0;

            //CmaeraToPlayerとMoveDirectionの内積からなす角を求める
            float theta = Vector3.Angle(m_cameraToPlayer.normalized, m_MoveDirection);//CmaeraToPlayerとMoveDirectionのなす角

            //回転半径、原点を求める
            m_rotRadius = m_cameraToPlayer.magnitude / Mathf.Sin(theta);
            m_rotCenter = Quaternion.Euler(0, 90, 0) * m_MoveDirection *  m_rotRadius;

            m_PlayerPosition = m_rotCenter + new Vector3(m_rotRadius * Mathf.Cos(Mathf.Deg2Rad * theta), 0, m_rotRadius * Mathf.Sin(Mathf.Deg2Rad * theta));
            Vector3 nextpos = m_rotCenter + new Vector3(m_rotRadius * Mathf.Cos(Mathf.Deg2Rad *(theta + m_rotSpeed)), 0, m_rotRadius * Mathf.Sin(Mathf.Deg2Rad * (theta + m_rotSpeed)));
            m_MoveDirectionAbs = nextpos - m_PlayerPosition;

            this.transform.localPosition = m_PlayerPosition + m_MoveDirectionAbs;

            //Debug.Log("m_PlayerPosition:" + m_PlayerPosition);
            Debug.Log("nextpos:" + nextpos);
            Debug.Log("m_rotCenter:" + m_rotCenter);

        }
    }

    private void FixedUpdate()
    {
        //m_rigidBody.AddForce(m_MoveDirectionAbs * m_walkSpeed);
    }

    
}
