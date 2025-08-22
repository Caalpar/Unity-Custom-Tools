using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;

[CustomPropertyDrawer(typeof(Actor))]
public class ActorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty enumTypeNameProp = property.FindPropertyRelative("enumTypeName");
        string enumTypeName = enumTypeNameProp.stringValue;

        Type enumType = null;
        if (!string.IsNullOrEmpty(enumTypeName))
        {
            enumType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => t.IsEnum && t.Name == enumTypeName);
        }

        // Si el tipo de enum no se encuentra, muestra un mensaje y no dibujes el popup
        if (enumType == null)
        {
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label.text, "Enum no seleccionado o no encontrado.");

            // Dibuja el resto de las propiedades aunque no haya un enum seleccionado
            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("targetPosition"), true);
            position.y += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetPosition")) + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(position, property.FindPropertyRelative("targetRotation"), true);

            EditorGUI.EndProperty();
            return;
        }

        // --- El resto del código de tu ActorDrawer es el mismo ---
        // (El que dibuja el popup con los valores del enum y el resto de los campos)

        // Código para dibujar la lista desplegable
        SerializedProperty objectIDProp = property.FindPropertyRelative("objectID");
        string[] enumValues = Enum.GetNames(enumType);
        int currentIndex = Array.IndexOf(enumValues, objectIDProp.stringValue);
        if (currentIndex < 0)
        {
            currentIndex = 0;
            if (enumValues.Length > 0)
            {
                objectIDProp.stringValue = enumValues[0];
            }
        }

        int newIndex = EditorGUI.Popup(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), label.text, currentIndex, enumValues);
        if (newIndex != currentIndex)
        {
            objectIDProp.stringValue = enumValues[newIndex];
        }

        // Dibuja el resto de las propiedades de la clase Actor
        position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        SerializedProperty posProp = property.FindPropertyRelative("targetPosition");
        EditorGUI.PropertyField(position, posProp, true);
        position.y += EditorGUI.GetPropertyHeight(posProp, true) + EditorGUIUtility.standardVerticalSpacing;

        SerializedProperty rotProp = property.FindPropertyRelative("targetRotation");
        EditorGUI.PropertyField(position, rotProp, true);

        EditorGUI.EndProperty();
    }

    // Y el GetPropertyHeight que ya tenías
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetPosition"), true) + EditorGUIUtility.standardVerticalSpacing;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("targetRotation"), true) + EditorGUIUtility.standardVerticalSpacing;
        return height;
    }
}