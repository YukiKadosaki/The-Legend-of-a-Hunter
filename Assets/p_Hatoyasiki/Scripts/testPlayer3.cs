using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class testPlayer3 : MonoBehaviour
{
    //[SerializeField] private Animator m_Animator;
    public GameObject camera;
    public float rotateSpeed;
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
    void Start(){
        m_Startpos = transform.position;
        m_Transform = this.transform;
        m_RigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        // 無効入力をスルー
        if (!((Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))))
            return;

        // 基準ベクトルの取得
        Vector2 playerVec2 = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 cameraVec2 = new Vector2(camera.transform.position.x, camera.transform.position.z);
        Vector2 criteriaVec2 = playerVec2 - cameraVec2;

        Vector2 velocityVec2 = m_MoveSpeed * criteriaVec2.normalized;
        var vec = velocityVec2;
        Vector2 lotateVec2 = Quaternion.Euler( 0, 0, 90 ) * vec / 2;

        List<Vector2> vec2list = new List<Vector2>();

        bool go_forward     = Input.GetKey(KeyCode.W);
        bool go_backward    = Input.GetKey(KeyCode.S);
        bool go_left        = Input.GetKey(KeyCode.A);
        bool go_right       = Input.GetKey(KeyCode.D);

        if(go_forward){
            playerVec2 += velocityVec2;
            cameraVec2 += velocityVec2;
            vec2list.Add(velocityVec2);
        }
        if(go_backward){
            playerVec2 -= velocityVec2;
            cameraVec2 -= velocityVec2;
            vec2list.Add(-velocityVec2);
        }
        if((go_left && !go_backward) || (go_right && go_backward))
            playerVec2 += lotateVec2;
        if((go_left && go_backward) || (go_right && !go_backward))
            playerVec2 -= lotateVec2;
        if(go_left)
            vec2list.Add(lotateVec2);
        if(go_right)
            vec2list.Add(-lotateVec2);


        Vector2 moveVec2 = Vector2.zero;
        foreach (Vector2 v in vec2list){
            moveVec2 += v;
        }
        Vector3 moveVec3 = new Vector3(moveVec2.x, 0, moveVec2.y);

        Vector2 new_criteriaVec2 = playerVec2 - cameraVec2;
        float difference = new_criteriaVec2.magnitude - criteriaVec2.magnitude;
        Vector2 diffeVec2 = difference * new_criteriaVec2.normalized;
        playerVec2 -= diffeVec2;

        this.transform.position = new Vector3(playerVec2.x, this.transform.position.y, playerVec2.y);
        camera.transform.position = new Vector3(cameraVec2.x, camera.transform.position.y, cameraVec2.y);



        camera.transform.LookAt(this.transform);
        this.transform.LookAt(this.transform.position + moveVec3);


    }
}
