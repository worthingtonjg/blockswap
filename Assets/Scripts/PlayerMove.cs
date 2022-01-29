using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float rotationSpeed = 4f;
    public float moveSpeed = 25f;

    private CharacterController characterController;
    private float yaw = 0f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        Vector3 moveDir = transform.forward * (Input.GetAxis(tag+"Vertical")+Input.GetAxis(tag+"VerticalJoystick")) * moveSpeed;

        //transform.eulerAngles = new Vector3(0f, yaw, 0f);
        moveDir += transform.right * (Input.GetAxis(tag+"Horizontal")+Input.GetAxis(tag+"HorizontalJoystick")) * moveSpeed;        

        characterController.SimpleMove(moveDir);

        //this.transform.Translate((Input.GetAxis(tag+"Horizontal")+Input.GetAxis(tag+"HorizontalJoystick"))*speed*Time.deltaTime, 0, (Input.GetAxis(tag+"Vertical")+Input.GetAxis(tag+"VerticalJoystick"))*speed*Time.deltaTime);
    }
}
