using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_PlayerConn : MonoBehaviour
{
    public int Speed;
    private Vector3 dir;

    private CharacterController _controller;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
        transform.position += dir * Speed * Time.deltaTime;
    }
}
