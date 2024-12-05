//Credits to MirrorFish (2020) for original idea

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerExit : MonoBehaviour
{
    public float delay = 0.5f;

    public delegate void ExitAction();
    public static event ExitAction OnChunkExited;

    private bool playerExited = false;

    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerTag playerTagObj = other.GetComponent<PlayerTag>();
        if(playerTagObj != null)
        {
            if(!playerExited)
            {
                playerExited = true;
                //Debug.Log("Exit trigger of " + transform.root.gameObject.name);
                OnChunkExited();
                StartCoroutine(WaitAndDeactivate());
            }
        }
    }

    IEnumerator WaitAndDeactivate()
    {
        yield return new WaitForSeconds(delay);

        //Debug.Log("Destroying " + transform.root.gameObject.name);
        //transform.root.gameObject.SetActive(false);
        Destroy(transform.root.gameObject);
    }
}
