using UnityEngine;
using System;

[Serializable]
public class ObjectID
{
    // Campo serializado para el Inspector
    public string id;

    // Métodos de utilidad para convertir a enum
    public T ToEnum<T>() where T : Enum
    {
        return (T)Enum.Parse(typeof(T), id);
    }
}