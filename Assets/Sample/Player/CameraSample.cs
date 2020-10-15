using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSample : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector3 target_to_camera;

    // Start is called before the first frame update
    void Start()
    {
        target_to_camera = this.transform.localPosition - target.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = target.transform.localPosition + target_to_camera;
    }
}
