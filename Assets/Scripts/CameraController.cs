using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position-player.transform.position;
    }

    void Update ()
    {
        var appliedPos = player.transform.position + _offset;
        appliedPos.x = 0;
        transform.position = appliedPos;
    }
}
