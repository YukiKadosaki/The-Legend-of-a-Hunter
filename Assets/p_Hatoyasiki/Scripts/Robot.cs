using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Robot : Boss {
    private GameObject player;
    void Start() {
        MoveSpeed = 5f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        StartCoroutine("MoveToDestination", player.transform.position);
    }
}
