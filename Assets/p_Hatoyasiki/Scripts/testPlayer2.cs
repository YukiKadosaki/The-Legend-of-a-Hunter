using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class testPlayer2 : MonoBehaviour
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
    void Start()
    {
        m_Startpos = transform.position;
        m_Transform = this.transform;
        m_RigidBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 無効入力をスルー
        if (!((Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))))
            return;

        // 基準ベクトルの取得
        Vector2 playerVec2 = new Vector2(this.transform.position.x, this.transform.position.z);
        Vector2 cameraVec2 = new Vector2(camera.transform.position.x, camera.transform.position.z);
        Vector2 criteriaVec2 = playerVec2 - cameraVec2;

        // 入力角度の取得
        List<float> anglelist = new List<float>();
        bool v = false;
        bool h = false;
        if(Input.GetKey(KeyCode.W)){
            anglelist.Add(0.00001f);
            v = true;
        }
        if(Input.GetKey(KeyCode.S)){
            if(!v)
                anglelist.Add(180.00001f);
        }
        if(Input.GetKey(KeyCode.A)){
            anglelist.Add(270.0001f);
            h = true;
        }
        if(Input.GetKey(KeyCode.D)){
            if(!h)
                anglelist.Add(90.0001f);
        }
        float angle = anglelist.Average();

        // 進行ベクトルの取得
        float theta = Mathf.Atan2(criteriaVec2.x, criteriaVec2.y) * 180 / Mathf.PI;
        Vector2 moveVec2 =criteriaVec2.magnitude *  new Vector2(Mathf.Cos((theta+angle)*Mathf.PI/180), Mathf.Sin((theta+angle)*Mathf.PI/180));
        Debug.Log(moveVec2);

        // 原点の取得
        Vector2 nomVec2;
        theta = Mathf.Atan2(moveVec2.x, moveVec2.y) * 180 / Mathf.PI;
        if(angle > 180f){
            nomVec2 = criteriaVec2.magnitude * new Vector2(Mathf.Cos((theta-90)*Mathf.PI/180), Mathf.Sin((theta-90)*Mathf.PI/180));
        }else{
            nomVec2 = criteriaVec2.magnitude * new Vector2(Mathf.Cos((theta+90)*Mathf.PI/180), Mathf.Sin((theta+90)*Mathf.PI/180));
        }
        originPoint = playerVec2 + nomVec2.normalized * (criteriaVec2.magnitude / Mathf.Abs(Mathf.Sin(angle * Mathf.PI /180)));
        Debug.Log(theta);
        Debug.Log("PL");
        Debug.Log(playerVec2);
        Debug.Log("CM");
        Debug.Log(cameraVec2);
        Debug.Log("OP");
        Debug.Log(originPoint);

        // XZ軸とのズレた角度を取得
        float phi = Mathf.Atan2(criteriaVec2.x, criteriaVec2.y) * 180 / Mathf.PI;

        // 移動
        float plCircleR = Vector2.Distance(originPoint, playerVec2);
        float cmCircleR = Vector2.Distance(originPoint, cameraVec2);
        playerVec2 += new Vector2(plCircleR * (Mathf.Cos(-(phi + angle + rotateSpeed)) - Mathf.Cos(-(phi + angle))), plCircleR * (Mathf.Sin(-(phi + angle + rotateSpeed)) - Mathf.Sin(-(phi + angle))));
        cameraVec2 += new Vector2(cmCircleR * (Mathf.Cos(-(phi + rotateSpeed)) - Mathf.Cos(-phi)), cmCircleR * (Mathf.Sin(-(phi + rotateSpeed)) - Mathf.Sin(-phi)));

        // 座標代入
        this.transform.position = new Vector3(playerVec2.x, this.transform.position.y, playerVec2.y);
        camera.transform.position = new Vector3(cameraVec2.x, camera.transform.position.y, cameraVec2.y);

        Debug.Log("-------------------------------");
    }
}