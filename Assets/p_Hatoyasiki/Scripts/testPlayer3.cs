using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class testPlayer3 : MobStatus
{
    //[SerializeField] private Animator m_Animator;
    public GameObject camera;
    public float rotateSpeed;
    public int cameraMoveTime = 0;
    [Header("移動速度")]
    [SerializeField] private float m_MoveSpeed = 10;
    [Header("ジャンプ力")]
    [SerializeField] private float m_JumpForce = 10;
    private float moveSpeed;
    private Vector2 originPoint;
    private Vector3 m_Startpos;
    private Rigidbody m_RigidBody;
    private Transform m_Transform;
    private Vector2 playerVec2;
    private Vector2 cameraVec2;
    private Vector2 criteriaVec2;
    private Vector2 moveVec2;
    private Vector3 moveVec3;
    private Vector3 dummyCameraVec3;
    private float const_distance;
    private bool isMovingCamera = false;
    public GameObject PlayerWP;
    private float WPReloadTime = 0;
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

    protected override void Start(){
        base.Start();
        m_Startpos  = transform.position;
        m_Transform = this.transform;
        m_RigidBody = this.GetComponent<Rigidbody>();

        dummyCameraVec3 = camera.transform.position;
        playerVec2 = new Vector2(this.transform.position.x, this.transform.position.z);
        cameraVec2 = new Vector2(dummyCameraVec3.x, dummyCameraVec3.z);
        criteriaVec2 = playerVec2 - cameraVec2;
        const_distance = criteriaVec2.magnitude;
        moveVec2 = Vector2.zero;
        camera.transform.LookAt(this.transform);
        if(serchTag(gameObject, "WP"))
            PlayerWP = serchTag(gameObject, "WP");
    }

    void Update(){
        Debug.Log("Life:" + Hp);
        WPReloadTime += Time.deltaTime;
        if(WPReloadTime >= 0.5f){
            if(serchTag(gameObject, "WP"))
                PlayerWP = serchTag(gameObject, "WP");
            WPReloadTime = 0f;
        }

        // 無効入力をスルー
        if ((Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))){
            // 基準ベクトルの取得
            playerVec2 = new Vector2(this.transform.position.x, this.transform.position.z);
            cameraVec2 = new Vector2(dummyCameraVec3.x, dummyCameraVec3.z);
            criteriaVec2 = playerVec2 - cameraVec2;

            // 前方向と横方向のベクトル取得
            Vector2 velocityVec2 = m_MoveSpeed * criteriaVec2.normalized * Time.deltaTime;
            Vector2 lotateVec2 = new Vector2(-velocityVec2.y, velocityVec2.x);

            // 入力した方向のベクトルを格納するリスト
            List<Vector2> vec2list = new List<Vector2>();

            // プレイヤーとカメラの移動とリストへの格納
            if(Input.GetKey(KeyCode.W)){
                playerVec2 += velocityVec2;
                cameraVec2 += velocityVec2;
                vec2list.Add(velocityVec2);
            }
            if(Input.GetKey(KeyCode.S)){
                playerVec2 -= velocityVec2;
                cameraVec2 -= velocityVec2;
                vec2list.Add(-velocityVec2);
            }
            if(Input.GetKey(KeyCode.A)){
                playerVec2 += lotateVec2;
                vec2list.Add(lotateVec2);
            }
            if(Input.GetKey(KeyCode.D)){
                playerVec2 -= lotateVec2;
                vec2list.Add(-lotateVec2);
            }

            // 進行方向のベクトルの取得
            moveVec2 = Vector2.zero;
            foreach (Vector2 v in vec2list){
                moveVec2 += v;
            }
            moveVec3 = new Vector3(moveVec2.x, 0, moveVec2.y);

            // 円運動によるズレの修正(カメラとの距離の固定のため)
            Vector2 new_criteriaVec2 = playerVec2 - cameraVec2;
            float difference = new_criteriaVec2.magnitude - criteriaVec2.magnitude;
            Vector2 diffeVec2 = difference * new_criteriaVec2.normalized * Time.deltaTime;
            playerVec2 -= diffeVec2;

            // オブジェクトにぶつかった際のズレの修正
            Vector2 neo_criteriaVec2 = playerVec2 - cameraVec2;
            float distance_difference = neo_criteriaVec2.magnitude - const_distance;
            cameraVec2 += distance_difference * neo_criteriaVec2.normalized;

            // 移動した2次元ベクトルをプレイヤーとカメラの3次元座標に代入
            this.transform.position = new Vector3(playerVec2.x, this.transform.position.y, playerVec2.y);
            dummyCameraVec3 = new Vector3(cameraVec2.x, dummyCameraVec3.y, cameraVec2.y);

            // 向きの修正
            camera.transform.LookAt(this.transform);
            this.transform.LookAt(this.transform.position + moveVec3);

            // 障害物の捜査
            Vector3 playerVec3 = this.transform.position;
            RaycastHit hit;
            int layerMask = ~(1 << 9);
            if(Physics.Raycast(playerVec3, dummyCameraVec3-playerVec3, out hit, (dummyCameraVec3-playerVec3).magnitude, layerMask)){
                camera.transform.position = new Vector3(hit.point.x, camera.transform.position.y, hit.point.z);
            }else{
                camera.transform.position = dummyCameraVec3;
            }
        }

        if(Input.GetKeyDown(KeyCode.K)){
            if(!isMovingCamera){
                StartCoroutine(MoveCamera());
                isMovingCamera = true;
            }
            // Vector3 newCameraVector3 = this.transform.position - moveVec3.normalized * const_distance;
            // camera.transform.position = new Vector3(newCameraVector3.x, camera.transform.position.y, newCameraVector3.z);
            // camera.transform.LookAt(this.transform);
        }

        IEnumerator MoveCamera(){
            Vector3 newCameraVector3 = this.transform.position - moveVec3.normalized * const_distance;
            Vector3 MoveCamVec3 = newCameraVector3 - camera.transform.position;
            for(int i = 0; i < cameraMoveTime; i++){
                camera.transform.position += new Vector3(MoveCamVec3.x, 0, MoveCamVec3.z) / cameraMoveTime;
                camera.transform.LookAt(this.transform); 
                dummyCameraVec3 = camera.transform.position;
                // 障害物の捜査
                Vector3 playerVec3 = this.transform.position;
                RaycastHit hit;
                int layerMask = ~(1 << 9);
                if(Physics.Raycast(playerVec3, dummyCameraVec3-playerVec3, out hit, (dummyCameraVec3-playerVec3).magnitude, layerMask)){
                    camera.transform.position = new Vector3(hit.point.x, camera.transform.position.y, hit.point.z);
                }else{
                    camera.transform.position = dummyCameraVec3;
                }
                yield return null;
            }
            isMovingCamera = false;
            yield break;
        }
    }

    GameObject serchTag(GameObject nowObj,string tagName){
        float tmpDis = 0;           //距離用一時変数
        float nearDis = 0;          //最も近いオブジェクトの距離
        GameObject targetObj = null; //オブジェクト

        //タグ指定されたオブジェクトを配列で取得する
        foreach (GameObject obs in  GameObject.FindGameObjectsWithTag(tagName)){
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得
            //一時変数に距離を格納
            if (nearDis == 0 || nearDis > tmpDis){
                nearDis = tmpDis;
                targetObj = obs;
            }

        }
        //最も近かったオブジェクトを返す
        return targetObj;
    }

}
