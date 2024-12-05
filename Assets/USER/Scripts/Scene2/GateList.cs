using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateList : MonoBehaviour
{
    public List<Transform> entryGatePos = new List<Transform>();
    public List<Transform> exitGatePos = new List<Transform>();

    private void OnValidate()
    {
        foreach(Transform child in this.transform)
        {
            if(child.transform.CompareTag("EntryGate") && !entryGatePos.Contains(child))
            {
                entryGatePos.Add(child.transform);
            }
        }

        foreach (Transform child in this.transform)
        {
            if (child.transform.CompareTag("ExitGate") && !exitGatePos.Contains(child))
            {
                exitGatePos.Add(child.transform);
            }
        }
    }
}
