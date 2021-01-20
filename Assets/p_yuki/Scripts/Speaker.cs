using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    AudioSource m_Audio;

    private void Start()
    {
        m_Audio = this.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touch");
        if (other.CompareTag("Player"))
        {
            m_Audio.Play();
            Debug.Log("Play!");
        }
    }
}
