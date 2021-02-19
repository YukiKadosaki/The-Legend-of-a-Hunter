//音を鳴らすスクリプト
//デュラハンは音を聞いて行動を変える

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Speaker : MonoBehaviour
{
    [Header("SoundRangeのコライダーを子供に付ける")]
    [SerializeField] private Collider m_SoundRange;
    AudioSource m_Audio;

    private void Start()
    {
        m_SoundRange.enabled = false;
        m_Audio = this.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_Audio.Play();
            //音の範囲を設定
            m_SoundRange.enabled = true;
        }
    }




}
