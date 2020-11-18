using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Yuki : MonoBehaviour
{
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_camera;
    private Vector3 m_CameraOffset;
    private Vector3 m_CameraToPlayer;


    // Start is called before the first frame update
    void Start()
    {
        m_CameraOffset = m_camera.localPosition - m_player.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        m_CameraToPlayer = (m_player.localPosition - this.transform.localPosition);
        this.transform.position = m_player.localPosition + m_CameraOffset;
        this.transform.LookAt(m_player);
        Debug.Log("this.transform.position:" + this.transform.localPosition);
    }
}
