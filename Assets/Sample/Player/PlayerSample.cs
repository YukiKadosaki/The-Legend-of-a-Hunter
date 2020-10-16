using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSample : MonoBehaviour
{
    //[SerializeField] private Animator m_Animator;
    [Header("移動速度")]
    [SerializeField] private float m_MoveSpeed = 10;
    [Header("ジャンプ力")]
    [SerializeField] private float m_JumpForce = 10;
    private Vector3 m_Startpos;
    private Rigidbody m_RigidBody;
    private Transform m_Transform;
    private bool m_OnLand
    {
        //RaycastNonAllocを使うので複雑になっている
        get
        {
            Ray ray = new Ray(this.transform.position + new Vector3(0, 0.5f), Vector3.down);
            RaycastHit[] raycastHits = new RaycastHit[1];
            int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 0.5f);
            return hitCount >= 1;
        }
    }

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
        Debug.DrawRay(this.transform.position + new Vector3(0, 0.5f)
            , Vector3.down, Color.red, 3, false);
        //プレイヤーの移動
        TryRun();
        TryJump();
        //リスタート
        TryRestart();
    }

    private void TryJump()
    {
        //着地時かつジャンプボタン押下時
        
        if(m_OnLand && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!!!");
            m_RigidBody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
        }
    }

    private void TryRun()
    {
        Vector3 moveDirection;

        //移動方向とアニメーションの決定
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = new Vector3(-m_MoveSpeed * Time.deltaTime, 0, 0);
            //m_Animator.SetBool("Run", true);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = new Vector3(m_MoveSpeed * Time.deltaTime, 0, 0);
            //m_Animator.SetBool("Run", true);
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
            //m_Animator.SetBool("Run", false);
        }
        //見る方向を変える
        m_Transform.LookAt(m_Transform.position + moveDirection);
        
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
