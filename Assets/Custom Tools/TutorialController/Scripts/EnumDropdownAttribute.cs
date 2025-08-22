using UnityEngine;
using System;

public class EnumDropdownAttribute : PropertyAttribute
{
    public Type EnumType { get; private set; }

    public EnumDropdownAttribute(Type enumType)
    {
        EnumType = enumType;
    }
}
