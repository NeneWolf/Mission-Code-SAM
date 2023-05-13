using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [SerializeField] private GameObject endPortal;

    public GameObject ReturnEndPortal()
    {
        return endPortal;
    }
}
