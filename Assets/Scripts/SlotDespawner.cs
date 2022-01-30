using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotDespawner : MonoBehaviour
{
    private SlotSpawner spawner;

    private void Start()
    {
        spawner = FindObjectOfType<SlotSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Slot") return;

        var slotComponent = other.gameObject.GetComponent<Slot>();
        spawner.RemoveSlot(slotComponent);
        GameObject.Destroy(other.gameObject);
    }

}
