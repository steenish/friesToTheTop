﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;
    public float heightOffset;

    private float offsetDistance;

    // Start is called before the first frame update
    void Start()
    {
        offsetDistance = (transform.position - player.transform.position).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + player.transform.forward * -1 * offsetDistance + player.transform.up * heightOffset;
        transform.LookAt(player.transform.position);
    }
}
