using System;
using UnityEngine;

public class OpenCurtain : MonoBehaviour
{
    [SerializeField] private GameObject volume;
    [SerializeField] private GameObject curtain;
    
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E)) {
            curtain.GetComponent<Animator>().Play("OpenCurtain");
        }
    }

    public void DisableVolume()
    {
        volume.SetActive(false);
    }
}
