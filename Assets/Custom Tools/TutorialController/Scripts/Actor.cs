using UnityEngine;
using System;

[Serializable]
public class Actor 
{
    [EnumDropdown(typeof(ITEM_OBJECTMANAGER))]
    public string objectID;
    public Vector3 targetPosition;
    public Quaternion targetRotation;
}
