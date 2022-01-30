using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public EnumPlayer playerName;
    public List<GameObject> NearParts;
    public GameObject NearestPart;
    public GameObject SelectedPart;
    public float rotationSpeed = 4f;
    public float moveSpeed = 25f;
    public float dropRange = 3f;

    private CharacterController characterController;
    private float yaw = 0f;
    private bool isHoldingPart = false;
    private SlotSpawner slotSpawner;
    private GameObject discardPartsConveyor;
    
    private Dictionary<EnumPlayer, PartSpawner> spawners;

    void Awake()
    {
        NearParts = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        slotSpawner = FindObjectOfType<SlotSpawner>();
        discardPartsConveyor = GameObject.FindGameObjectWithTag("DiscardPartsConveyor");

        var spawnerList = GameObject.FindObjectsOfType<PartSpawner>();
        spawners = spawnerList.ToDictionary(k => k.Owner, v => v);
    }

    // Update is called once per frame
    void Update()
    {
        //yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        Vector3 moveDir = transform.forward * (Input.GetAxis(tag+"Vertical")+Input.GetAxis(tag+"VerticalJoystick")) * moveSpeed;
        
        //transform.eulerAngles = new Vector3(0f, yaw, 0f);
        moveDir += transform.right * (Input.GetAxis(tag+"Horizontal")+Input.GetAxis(tag+"HorizontalJoystick")) * moveSpeed;        

        yaw += (Input.GetAxis(tag+"Horizontal")+Input.GetAxis(tag+"HorizontalJoystick")) * rotationSpeed;
        //transform.eulerAngles = new Vector3(0f, yaw, 0f);

        //Model.transform.Rotate(Vector3.up, yaw, Space.World);

        characterController.SimpleMove(moveDir);

        //this.transform.Translate((Input.GetAxis(tag+"Horizontal")+Input.GetAxis(tag+"HorizontalJoystick"))*speed*Time.deltaTime, 0, (Input.GetAxis(tag+"Vertical")+Input.GetAxis(tag+"VerticalJoystick"))*speed*Time.deltaTime);

        FindNearestPart();

        if (!isHoldingPart && (playerName == EnumPlayer.P1 && Input.GetButtonDown("P1Pickup") ||
            playerName == EnumPlayer.P2 && Input.GetButtonDown("P2Pickup")))
        {
            if (NearestPart != null)
            {
                SelectedPart = NearestPart;
                NearestPart = null;

                SelectedPart.transform.SetParent(gameObject.transform);
                SelectedPart.GetComponent<PartMove>().destination = null;
                NearParts.Remove(SelectedPart);

                SelectedPart.GetComponent<Part>().ToggleSelected(false);
                spawners[playerName].TakePartFromConveyer(SelectedPart);
                isHoldingPart = true;
            }
        }
        else if (isHoldingPart && (playerName == EnumPlayer.P1 && Input.GetButtonDown("P1Pickup") ||
            playerName == EnumPlayer.P2 && Input.GetButtonDown("P2Pickup")))
        {
            SelectedPart.transform.SetParent(null);
            isHoldingPart = false;

            float distanceToConveyor = Mathf.Abs(transform.position.x - discardPartsConveyor.transform.position.x);
            print($"distanceToConveyor: {distanceToConveyor}");
            if(distanceToConveyor < dropRange)
            {
                slotSpawner.AddPartToSlot(SelectedPart);
            }
            else
            {
                spawners[playerName].AddPartToMachine(SelectedPart);
            }

            SelectedPart = null;
        }
    }

    private void FindNearestPart()
    {
        if(isHoldingPart) 
        {
            NearParts.Clear();
            return;
        }

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
        if(part != null && part.CanSelect)
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
