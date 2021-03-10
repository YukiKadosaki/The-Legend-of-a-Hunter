using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Robot : Boss {
    void Start() {
        MoveSpeed = 10f;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update() {
        StartCoroutine("MoveToDestination");
    }
}
