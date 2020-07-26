using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

#pragma warning disable
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float heightOffset;
#pragma warning restore

    private float offsetDistance;

    void Start() {
        offsetDistance = (transform.position - player.transform.position).magnitude;
    }

    void Update() {
        transform.position = player.transform.position + player.transform.forward * -1 * offsetDistance + player.transform.up * heightOffset;
        transform.LookAt(player.transform.position);
    }
}
