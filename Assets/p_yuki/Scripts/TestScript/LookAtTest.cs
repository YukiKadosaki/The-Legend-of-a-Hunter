using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTest : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var direction = target.transform.position - this.transform.position;
        direction.y = 0;
        transform.LookAt(this.transform.position + direction);
    }
}
