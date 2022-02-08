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
    public GameObject Button;
    public Camera MainCamera;

    private CharacterController characterController;
    private float yaw = 0f;
    private bool isHoldingPart = false;
    private bool canPressButton = false;
    private SlotSpawner slotSpawner;
    private GameObject discardPartsConveyor;
    private Dictionary<EnumPlayer, PartSpawner> spawners;
    private float P1CameraPosition = -13f;
    private float P2CamerPosition = -4f;

    private GameObject DropZone;
    private bool CanDropOnMainConveyor = false;
    private int PlayerCount;

    void Awake()
    {
        NearParts = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerCount = PlayerPrefs.GetInt("PlayerCount");
        if (PlayerCount == 1)
        {
            MainCamera.transform.position = new Vector3(P1CameraPosition, MainCamera.transform.position.y, MainCamera.transform.position.z);
            if (playerName == EnumPlayer.P2)
            {
                return;
            }
        }
        else
        {
            MainCamera.transform.position = new Vector3(P2CamerPosition, MainCamera.transform.position.y, MainCamera.transform.position.z);
        }

        characterController = GetComponent<CharacterController>();
        slotSpawner = FindObjectOfType<SlotSpawner>();
        discardPartsConveyor = GameObject.FindGameObjectWithTag("DiscardPartsConveyor");

        var spawnerList = GameObject.FindObjectsOfType<PartSpawner>();
        spawners = spawnerList.ToDictionary(k => k.Owner, v => v);

        DropZone = GameObject.FindWithTag("DropZone");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCount == 2 || playerName == EnumPlayer.P1)
        {
            MoveCharacter();
            FindNearestPart();

            if(Input.GetButtonDown(tag+"Pickup"))
            {
                if (!isHoldingPart)
                {
                    if(canPressButton)
                    {
                        PressNewPartButton();
                    } 
                    else 
                    {
                        if (NearestPart != null)
                        {
                            PickupPartFromConveyor();
                        }
                    }
                }
                else
                {
                    DropPart();
                }
            }
        }
    }

    private void MoveCharacter()
    {
        if(!PartsManager.Instance.GameStarted) return;

        Vector3 moveDir = transform.forward * (Input.GetAxis(tag+"Vertical")+Input.GetAxis(tag+"VerticalJoystick")) * moveSpeed;
        
        moveDir += transform.right * (Input.GetAxis(tag+"Horizontal")+Input.GetAxis(tag+"HorizontalJoystick")) * moveSpeed;        

        yaw += (Input.GetAxis(tag+"Horizontal")+Input.GetAxis(tag+"HorizontalJoystick")) * rotationSpeed;

        characterController.SimpleMove(moveDir);
    }

    private void PressNewPartButton()
    {
        var part = PartsManager.Instance.TakePart();
        spawners[playerName].AddPartFromButton(part);
        DeactivateButton();
        SetSelectedPart(part);
    }

    private void PickupPartFromConveyor()
    {
        spawners[playerName].TakePartFromConveyer(NearestPart);
        SetSelectedPart(NearestPart);
    }

    private void SetSelectedPart(GameObject part)
    {
        SoundEffectsManager.Instance.PlayPickup();

        SelectedPart = part;

        NearestPart = null;
        NearParts.Remove(SelectedPart);

        SelectedPart.GetComponent<Part>().ToggleSelected(false);
        SelectedPart.transform.SetParent(gameObject.transform);
        SelectedPart.GetComponent<PartMove>().destination = null;
        SelectedPart.transform.position = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);

        isHoldingPart = true;
    }

    private void DropPart()
    {
        if(CanDropOnMainConveyor)
        {
            bool success = slotSpawner.AddPartToSlot(SelectedPart);
            if(success)
            {
                SoundEffectsManager.Instance.PlayDrop();
                isHoldingPart = false;
                SelectedPart = null;
            }
        }
        else
        {
            SoundEffectsManager.Instance.PlayDrop();
            isHoldingPart = false;
            spawners[playerName].AddPartToMachine(SelectedPart,"Part returned to Inventory");
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

        if(other.tag == "Button")
        {
            ActivateButton();
        }

        if(other.tag == "DropZone")
        {
            CanDropOnMainConveyor = true;
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
    
        if(other.tag == "Button")
        {
            DeactivateButton();
        }    

        if(other.tag == "DropZone")
        {
            CanDropOnMainConveyor = false;
        }
    }

    private void ActivateButton()
    {
        if(isHoldingPart) return;

        Button.GetComponent<Renderer>().material.color = Color.red;
        canPressButton = true;
    }

    private void DeactivateButton()
    {
        Button.GetComponent<Renderer>().material.color = Color.white;
        canPressButton = false;
    }
}
