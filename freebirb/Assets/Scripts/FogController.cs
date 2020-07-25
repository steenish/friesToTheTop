using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour {

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float fogPlayerOffset;

    private float playerMaxAltitude;

    void Start() {
        
    }

    void Update() {
        playerMaxAltitude = (playerTransform.position.y > playerMaxAltitude) ? playerTransform.position.y : playerMaxAltitude;
        transform.position = new Vector3(playerTransform.position.x, playerMaxAltitude - fogPlayerOffset, playerTransform.position.z);
    }
}
