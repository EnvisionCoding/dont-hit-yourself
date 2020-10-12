using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    
    [Header("Player Settings")]
    public float movementSpeed;
    public Rigidbody2D playerRigidbody;
    private Vector3 movementInput;
    private Vector3 moveVelocity;

    void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);
        moveVelocity = movementInput * movementSpeed;
    }

    void FixedUpdate()
    {
        playerRigidbody.velocity = moveVelocity;
    }
    
}
