using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : MobStatus
{
    //[SerializeField] private Animator m_Animator;
    public GameObject camera;
    public int cameraMoveTime = 0;
    [Header("ジャンプ力")]
    [SerializeField] private float m_JumpForce = 10;
    [SerializeField] private GameObject Canvas;
    private Transform m_Transform;
    private Vector2 playerVec2;
    private Vector2 cameraVec2;
    private Vector2 criteriaVec2;
    private Vector2 moveVec2;
    private Vector3 moveVec3;
    private Vector3 dummyCameraVec3;
    private float const_distance;
    private bool isMovingCamera = false;
    private Slider slider;

    protected override void Start(){
        base.Start();
        m_Transform = this.transform;

        dummyCameraVec3 = camera.transform.position;
        playerVec2 = new Vector2(m_Transform.position.x, m_Transform.position.z);
        cameraVec2 = new Vector2(dummyCameraVec3.x, dummyCameraVec3.z);
        criteriaVec2 = playerVec2 - cameraVec2;
        const_distance = criteriaVec2.magnitude;
        moveVec2 = Vector2.zero;
        camera.transform.LookAt(m_Transform);

        GameObject canvasObject = Instantiate(Canvas, Vector3.zero, Quaternion.identity);

        slider = canvasObject.transform.Find("Slider").gameObject.GetComponent<Slider>();
        slider.maxValue = Hp;
    }

    void Update(){
        slider.value = Hp;

        MoveLikeZelda();

        if(Input.GetKeyDown(KeyCode.K)){
            if(!isMovingCamera){
                StartCoroutine(MoveCamera());
                isMovingCamera = true;
            }
        }
    }

    void MoveLikeZelda(){
        // 無効入力をスルー
        if ((Input.GetKey(KeyCode.W) ^ Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.A) ^ Input.GetKey(KeyCode.D))){
            // 基準ベクトルの取得
            playerVec2 = new Vector2(m_Transform.position.x, m_Transform.position.z);
            cameraVec2 = new Vector2(dummyCameraVec3.x, dummyCameraVec3.z);
            criteriaVec2 = playerVec2 - cameraVec2;

            // 前方向と横方向のベクトル取得
            Vector2 velocityVec2 = MoveSpeed * criteriaVec2.normalized * Time.deltaTime;
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
            m_Transform.position = new Vector3(playerVec2.x, m_Transform.position.y, playerVec2.y);
            dummyCameraVec3 = new Vector3(cameraVec2.x, dummyCameraVec3.y, cameraVec2.y);

            // 向きの修正
            camera.transform.LookAt(m_Transform);
            m_Transform.LookAt(m_Transform.position + moveVec3);

            // 障害物の捜査
            Vector3 playerVec3 = m_Transform.position;
            RaycastHit hit;
            int layerMask = ~(1 << 9);
            if(Physics.Raycast(playerVec3, dummyCameraVec3-playerVec3, out hit, (dummyCameraVec3-playerVec3).magnitude, layerMask)){
                camera.transform.position = new Vector3(hit.point.x, camera.transform.position.y, hit.point.z);
            }else{
                camera.transform.position = dummyCameraVec3;
            }
        }
    }

    private IEnumerator MoveCamera(){
        Vector3 newCameraVector3 = m_Transform.position - moveVec3.normalized * const_distance;
        Vector3 MoveCamVec3 = newCameraVector3 - camera.transform.position;
        for(int i = 0; i < cameraMoveTime; i++){
            camera.transform.position += new Vector3(MoveCamVec3.x, 0, MoveCamVec3.z) / cameraMoveTime;
            camera.transform.LookAt(m_Transform); 
            dummyCameraVec3 = camera.transform.position;
            // 障害物の捜査
            Vector3 playerVec3 = m_Transform.position;
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

