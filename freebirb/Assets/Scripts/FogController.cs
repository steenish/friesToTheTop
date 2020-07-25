using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FogController : MonoBehaviour {

    [SerializeField]
    private float _fogPlayerOffset;
    public float fogPlayerOffset { get => _fogPlayerOffset; private set => _fogPlayerOffset = value; }

#pragma warning disable
    [SerializeField]
    private Transform playerTransform;
#pragma warning restore

    public float playerMaxAltitude { get; private set; }

    void Update() {
        playerMaxAltitude = (playerTransform.position.y > playerMaxAltitude) ? playerTransform.position.y : playerMaxAltitude;
        transform.position = new Vector3(playerTransform.position.x, playerMaxAltitude - fogPlayerOffset, playerTransform.position.z);
    }
}
