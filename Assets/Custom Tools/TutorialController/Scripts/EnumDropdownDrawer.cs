using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CustomPropertyDrawer(typeof(EnumDropdownAttribute))]
public class EnumDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Solo funciona con campos de tipo string
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Solo se puede usar el atributo EnumDropdown en campos de tipo string.");
            return;
        }

        EnumDropdownAttribute enumAttribute = attribute as EnumDropdownAttribute;
        Type enumType = enumAttribute.EnumType;

        // Obtener los nombres de los enums
        string[] enumNames = Enum.GetNames(enumType);

        // Obtener el valor actual del string
        string currentName = property.stringValue;

        // Encontrar el índice del nombre actual
        int currentIndex = Array.IndexOf(enumNames, currentName);

        // Si no se encuentra, establecer el índice a 0
        if (currentIndex < 0)
        {
            currentIndex = 0;
            // Opcional: Asignar el primer valor como predeterminado si el string está vacío
            if (string.IsNullOrEmpty(currentName) && enumNames.Length > 0)
            {
                property.stringValue = enumNames[0];
            }
        }

        // Dibujar el Popup
        int newIndex = EditorGUI.Popup(position, label.text, currentIndex, enumNames);

        // Si el índice ha cambiado, actualizar el valor del string
        if (newIndex != currentIndex)
        {
            property.stringValue = enumNames[newIndex];
        }
    }
}