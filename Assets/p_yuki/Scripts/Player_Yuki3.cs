using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player_Yuki3 : MonoBehaviour
{
    //[SerializeField] private Animator m_Animator;
    [SerializeField] private Camera m_Camera;
    [Header("カメラ位置")]
    [SerializeField] Vector3 m_CameraOffset;//プレイヤーとの相対座標
    [Header("移動速度")]
    [SerializeField] private float m_MoveSpeed = 10;
    [Header("ジャンプ力")]
    [SerializeField] private float m_JumpForce = 10;
    private float moveSpeed;
    private Vector2 originPoint;
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
        //Transformをキャッシュ
        m_Transform = this.transform;
        //カメラ位置を決定
        m_Camera.transform.localPosition = m_Transform.localPosition + m_CameraOffset;
        //開始位置を記録
        m_Startpos = m_Transform.localPosition;
        //RigidBodyをキャッシュ
        m_RigidBody = this.GetComponent<Rigidbody>();

        Debug.Log(Vector3.right);
        Debug.Log(Quaternion.Euler(0, 0, 90) * Vector3.right);

    }

    // Update is called once per frame
    void Update()
    {
        m_Camera.transform.LookAt(m_Transform);

        // 無効入力をスルー
        if (!((Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))))
            return;

        // 基準ベクトルの取得
        Vector2 playerVec2 = new Vector2(m_Transform.position.x, m_Transform.position.z);
        Vector2 cameraVec2 = new Vector2(m_Camera.transform.localPosition.x, m_Camera.transform.localPosition.z);
        Vector2 criteriaVec2 = playerVec2 - cameraVec2;

        Vector2 velocityVec2 = m_MoveSpeed * criteriaVec2.normalized;
        //velocityVec2はx, z成分を持っている（つもり）だが、Vector2クラスなので、z軸に90°回転させることでy軸回転させたことになる？
        Vector2 lotateVec2 = Quaternion.Euler(0, 0, 90) * velocityVec2;

        List<Vector2> vec2list = new List<Vector2>();

        bool go_forward = Input.GetKey(KeyCode.W);
        bool go_backward = Input.GetKey(KeyCode.S);
        bool go_left = Input.GetKey(KeyCode.A);
        bool go_right = Input.GetKey(KeyCode.D);

        if (go_forward)
        {
            playerVec2 += velocityVec2;
            cameraVec2 += velocityVec2;
            vec2list.Add(velocityVec2);
        }
        if (go_backward)
        {
            playerVec2 -= velocityVec2;
            cameraVec2 -= velocityVec2;
            vec2list.Add(-velocityVec2);
        }
        if (go_left)
        {
            playerVec2 += lotateVec2;
            vec2list.Add(lotateVec2);
        }
        if (go_right)
        {
            playerVec2 -= lotateVec2;
            vec2list.Add(-lotateVec2);
        }


        Vector2 moveVec2 = Vector2.zero;
        foreach (Vector2 v in vec2list)
        {
            moveVec2 += v * Time.deltaTime;
        }
        Vector3 moveVec3 = new Vector3(moveVec2.x, 0, moveVec2.y);

        Vector2 new_criteriaVec2 = playerVec2 - cameraVec2;
        float difference = new_criteriaVec2.magnitude - criteriaVec2.magnitude;
        Vector2 diffeVec2 = difference * new_criteriaVec2.normalized;
        playerVec2 -= diffeVec2;

        m_Transform.localPosition = new Vector3(playerVec2.x, this.transform.position.y, playerVec2.y);
        m_Camera.transform.position = new Vector3(cameraVec2.x, m_Camera.transform.position.y, cameraVec2.y);

        m_Camera.transform.LookAt(this.transform);
        m_Transform.LookAt(this.transform.position + moveVec3);

        
    }
}
