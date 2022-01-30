using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SlotSpawner : MonoBehaviour
{
    public GameObject SlotPrefab;
    public GameObject Destination;
    public List<Slot> slots;

    void Awake()
    {
        slots = new List<Slot>();
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnSlot", 0f, 3f);
    }

    private void SpawnSlot()
    {
        var slotInstance = GameObject.Instantiate(SlotPrefab, transform.position, this.transform.rotation);
        var partMoveComponent = slotInstance.GetComponent<PartMove>();
        partMoveComponent.destination = Destination;
        var slotComponent = slotInstance.GetComponent<Slot>();
        slots.Insert(0, slotComponent);
    }

    public void AddPartToSlot(GameObject part)
    {
        var slot = FindSlot();
        if(slot == null) return;
        
        slot.Occupied = true;

        part.transform.position = slot.transform.position;

        var partMoveComponent = part.GetComponent<PartMove>();
        partMoveComponent.destination = Destination;

        var partComponent = part.GetComponent<Part>();
        partComponent.CanSelect = false;
    }

    public void RemoveSlot(Slot slot)
    {
        slots.Remove(slot);
    }

    private Slot FindSlot()
    {
        if(!slots.Any(s => s.Occupied))
        {
            var middle = slots.Count / 2;
            return slots[middle];
        }
        else
        {
            var firstOccupied = slots.FirstOrDefault(s => s.Occupied == true);
            var index = slots.IndexOf(firstOccupied);

            if(index > 0) 
            {
                return slots[index - 1];
            }

            return null;
        }
    }
}
