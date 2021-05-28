using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExibitCamera : MonoBehaviour
{
    [SerializeField]private GameObject obj;
    private float distance;
    private float theta = Mathf.Deg2Rad * -90;
    private float movespeed = 5;


    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Distance(transform.position, obj.transform.position);
        this.transform.position = obj.transform.position + distance * new Vector3(Mathf.Cos(theta), 0.2f, Mathf.Sin(theta));
    }

    // Update is called once per frame
    void Update()
    {
        movespeed = 5;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movespeed = 15;
        }

        if (Input.GetKey(KeyCode.A))
        {
            theta += Time.deltaTime * movespeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            theta -= Time.deltaTime * movespeed;
        }

        if (Input.GetKey(KeyCode.W) && distance >= 0.1f)
        {
            distance -= Time.deltaTime * movespeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            distance += Time.deltaTime * movespeed;
        }

        transform.LookAt(obj.transform);
        this.transform.position = obj.transform.position + distance * new Vector3(Mathf.Cos(theta), 0.2f, Mathf.Sin(theta));
    }
}
