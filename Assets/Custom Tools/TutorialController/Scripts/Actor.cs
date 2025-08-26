using UnityEngine;
using System;

[Serializable]
public class Actor 
{
    // Campo para que el editor sepa qué enum mostrar
    [HideInInspector] // Oculta este campo del Inspector normal
    public string enumTypeName;

    // Aquí ya tienes el atributo, que es el que usaremos en el PropertyDrawer
    public string objectID;

    public bool alwaysActive;

    public Vector3 targetPosition;
    public Quaternion targetRotation;
}
