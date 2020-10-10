using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float m_MoveSpeed;
    private Vector3 m_Startpos;
    private Rigidbody m_RigidBody;
    private Transform m_Transform;

    // Start is called before the first frame update
    void Start()
    {
        m_Startpos = transform.position;
        m_Transform = this.transform;
        m_RigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        //プレイヤーの移動
        TryRun();
        //リスタート
        TryRestart();
    }

    private void TryRun()
    {
        Vector3 moveDirection;

        //移動方向とアニメーションの決定
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = new Vector3(-m_MoveSpeed * Time.deltaTime, 0, 0);
            m_Animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = new Vector3(m_MoveSpeed * Time.deltaTime, 0, 0);
            m_Animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection = new Vector3(0, 0, m_MoveSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection = new Vector3(0, 0, -m_MoveSpeed * Time.deltaTime);
        }
        else
        {
            moveDirection = new Vector3(0, 0, 0);
            m_Animator.SetBool("Run", false);
        }
        //見る方向を変える
        m_Transform.LookAt(m_Transform.position + moveDirection);

        //前後には動かないように
        moveDirection.z = 0;

        //移動
        m_RigidBody.MovePosition(m_Transform.position + moveDirection);
        

    }

    private void TryRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = m_Startpos;
            GetComponent<Rigidbody>().velocity *= 0;
        }
    }
    
}
