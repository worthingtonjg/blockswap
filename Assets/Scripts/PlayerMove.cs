using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public List<GameObject> NearParts;
    public GameObject NearestPart;
    public float rotationSpeed = 4f;
    public float moveSpeed = 25f;

    private CharacterController characterController;
    private float yaw = 0f;
    private bool isHoldingPart = false;
    private EnumPlayer playerName;

    void Awake()
    {
        NearParts = new List<GameObject>();
    }

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

        FindNearestPart();
        if (!isHoldingPart && (playerName == EnumPlayer.P1 && Input.GetButtonDown("P1Pickup") ||
            playerName == EnumPlayer.P2 && Input.GetButtonDown("P2Pickup")))
        {
            if (NearestPart != null)
            {
                NearestPart.transform.SetParent(gameObject.transform);
                NearestPart.GetComponent<PartMove>().destination = null;
                isHoldingPart = true;
            }
        }
        else if (isHoldingPart && (playerName == EnumPlayer.P1 && Input.GetButtonDown("P1Pickup") ||
            playerName == EnumPlayer.P2 && Input.GetButtonDown("P2Pickup")))
        {
            NearestPart.transform.SetParent(null);
            isHoldingPart = false;
        }
    }

    private void FindNearestPart()
    {
        NearestPart = null;
        float? closest = null;
        foreach(var part in NearParts)
        {
            float distanceToPart = Vector3.Distance(transform.position, part.transform.position);
            if(closest == null || distanceToPart < closest)
            {
                closest = distanceToPart;
                NearestPart = part;
                //print($"Nearest: {part.name}");
                playerName = NearestPart.GetComponent<Part>().Owner;
            }
        }

        foreach(var part in NearParts)
        {
            var partComponent = part.GetComponent<Part>();
            partComponent.ToggleSelected(false);
        }

        if(NearestPart != null)
        {
            var partComponent = NearestPart.GetComponent<Part>();
            partComponent.ToggleSelected(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var part = other.gameObject.GetComponent<Part>();
        if(part != null)
        {
            NearParts.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var part = other.gameObject.GetComponent<Part>();
        if(part != null)
        {
            part.ToggleSelected(false);
            NearParts.Remove(other.gameObject);
        }        
    }
}
