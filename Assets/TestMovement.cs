using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMovement : MonoBehaviour
{

    public float speed = 10f;
    private Vector2 dir = Vector2.zero;

    public void OnMove(InputValue value)
    {
        Debug.Log("Moving" + value.Get<Vector2>());
        dir = value.Get<Vector2>().normalized;
    }

    private void FixedUpdate()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }
}
