using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSpawner : MonoBehaviour
{
    public GameObject SlotPrefab;
    public GameObject Destination;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnSlot", 0f, 3f);
    }

    private void SpawnSlot()
    {
        var slotInstance = GameObject.Instantiate(SlotPrefab, transform.position, Quaternion.identity);
        var partMoveComponent = slotInstance.GetComponent<PartMove>();
        partMoveComponent.destination = Destination;
    }
}
