using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {
    void Update() {
        Quaternion rotation = transform.rotation;
            Vector3 eulerRotation = rotation.eulerAngles;
            eulerRotation.z = 0f;
            rotation.eulerAngles = eulerRotation;
            transform.rotation = rotation;
    }
}
