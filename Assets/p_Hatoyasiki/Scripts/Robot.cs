using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Robot : Boss {
    public float firstHpBorder;
    public float secondHpBorder;
    private GameObject player;
    private GameObject camera;
    private enum RobotStatus{
        FullBody,
        LostReg,
        OnlyBody
    }
    private RobotStatus nowStatus;
    private StateEnum tmp_state = StateEnum.Normal;
    private float AnimeTime = -1f;

    void Start() {
        base.Start();
        nowStatus = RobotStatus.FullBody;
        MoveSpeed = 5f;
        if(Hp < firstHpBorder){
            firstHpBorder = Hp;
        }
        if(firstHpBorder < secondHpBorder){
            secondHpBorder = firstHpBorder;
        }
        player = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update() {
        if (AnimeTime > 0f) {
            AnimeTime -= Time.deltaTime;
            if (AnimeTime <= 0f){
                _state = tmp_state;
            }
        }
        switch (nowStatus) {
            case RobotStatus.FullBody:
                if(Hp < firstHpBorder){
                    Hp = firstHpBorder;
                    BecomeLostReg();
                }
                break;
            case RobotStatus.LostReg:
                if(Hp < secondHpBorder){
                    Hp = secondHpBorder;
                    BecomeOnlyBody();
                }
                break;
            case RobotStatus.OnlyBody:
                break;
        }
        if(Hp <= 0){
            BecomeDie();
        }
        Vector3 destination = transform.position - player.transform.position;
        destination.y = 0f;
        if(destination.magnitude > 7f){
            _state = StateEnum.Normal;
        }else{
            StopRunning();
            _state = StateEnum.Attack;
        }
        if(_state == StateEnum.Normal){
            _animator.SetBool("isWalk", true);
            StartCoroutine("MoveToDestination", player.transform.position);
        }
        if(_state == StateEnum.Attack){
            _animator.SetBool("isWalk", false);
            switch (nowStatus) {
                case RobotStatus.FullBody:
                    break;
                case RobotStatus.LostReg:
                    break;
                case RobotStatus.OnlyBody:
                    break;
            }
        }
    }

    void BecomeLostReg(){
        StopRunning();
        tmp_state = _state;
        _state = StateEnum.Animation;
        AnimeTime = 5f;
        SetCammeraPosition();
        GameObject[] foots = GameObject.FindGameObjectsWithTag("Foot");
        foreach (GameObject foot in foots) {
            GameObject.Destroy(foot);
        }
        nowStatus = RobotStatus.LostReg;
    }

    void BecomeOnlyBody(){
        StopRunning();
        tmp_state = _state;
        _state = StateEnum.Animation;
        AnimeTime = 5f;
        SetCammeraPosition();
        GameObject[] arms = GameObject.FindGameObjectsWithTag("Arm");
        foreach (GameObject arm in arms) {
            GameObject.Destroy(arm);
        }
        nowStatus = RobotStatus.OnlyBody;
    }

    void BecomeDie(){
        Debug.Log("Destroy");
        // GameObject.Destroy(GameObject.FindGameObjectWithTag("Robot"));
        OnDie();
    }

    void OnCollisionEnter(Collision col) {
        // if (col.gameObject.tag == "Player"){
        //     Hp -= 5;
        //     Debug.Log(Hp);
        // }
    }

    void SetCammeraPosition(){
        // Vector3 toPlayerVec3 = player.transform.position - this.transform.position;
        // Vector2 toPlayerNomVec2 = new Vector2(toPlayerVec3.x, toPlayerVec3.z).normalized;
        // Vector2 camVec2 = new Vector2(transform.position.x, transform.position.z) + toPlayerNomVec2 * 10f;
        // camera.transform.position = new Vector3(camVec2.x, camera.transform.position.y, camVec2.y);
        // camera.transform.LookAt( new Vector3(transform.position.x, camera.transform.position.y, transform.position.z) );
    }
}
