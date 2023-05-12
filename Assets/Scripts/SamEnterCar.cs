using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamEnterCar : MonoBehaviour
{
    [SerializeField] private Transform samSeatPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sam")
        {
            print("Here");
            other.gameObject.transform.SetParent(samSeatPosition);
            other.gameObject.transform.position = samSeatPosition.transform.position;
            other.gameObject.GetComponent<SamBehaviour>().DisableSamMovement();
        }
    }
}
