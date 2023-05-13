using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject barPortal;
    [SerializeField] private GameObject endPortal;

    public GameObject ReturnEndPortal()
    {
        return endPortal;
    }

    public GameObject ReturnBarPortal()
    {
        return barPortal;
    }
}
