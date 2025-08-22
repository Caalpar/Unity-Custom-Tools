using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectManager : MonoBehaviour
{

    string pathFolder = "GeneratedEnums";
    [HideInInspector]
    public  GameObject[] listObjects;


    public int listObjectsAmount { get { return listObjects.Length; } }

    public void Active(Enum value)
    {
        int index = Convert.ToInt32(value);

        if (index < 0) return;
        if (index >= listObjects.Length) return;

        listObjects[index].SetActive(true);
    }



    public void Active(int index)
    {
        if (index < 0) return;
        if (index >= listObjects.Length) return;

        listObjects[index].SetActive(true);
    }

    public void Desactive(int index)
    {
        if (index < 0) return;
        if (index >= listObjects.Length) return;

        listObjects[index].SetActive(false);
    }

    public void Desactive(Enum value)
    {
        int index = Convert.ToInt32(value);
        if (index < 0) return;
        if (index >= listObjects.Length) return;

        listObjects[index].SetActive(false);
    }

    public void Toggle(int index)
    {
        if (index < 0) return;
        if (index >= listObjects.Length) return;

        listObjects[index].SetActive(!listObjects[index].activeSelf);
    }



    public void Toggle(Enum value)
    {
        int index = Convert.ToInt32(value);
        if (index < 0) return;
        if (index >= listObjects.Length) return;

        listObjects[index].SetActive(!listObjects[index].activeSelf);
    }


    public GameObject this[int index]
    {
        get
        {
            if (index < 0 || index >= listObjects.Length) return null;
            return listObjects[index];
        }
    }

    public GameObject this[Enum valor]
    {
        get
        {
            int index = Convert.ToInt32(valor);
            if (index < 0 || index >= listObjects.Length) return null;
            return listObjects[index];

        }
    }

    public T GetComponentFrom<T>(Enum valor)
    {
       return this[valor].GetComponent<T>();
    }

    public T GetComponentFrom<T>(int valor)
    {
        return this[valor].GetComponent<T>();
    }


    public void CrearEnumDesdeObjetos()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        string ItemName = scene.name.Replace(" ", "_");
        ItemName = ItemName.Replace("-", "");
        ItemName = ItemName.ToUpper();
        ItemName = EnumCreator.EliminarCaracteresDuplicadosConsecutivos(ItemName, '_');

        EnumCreator.CrearEnum("ITEM_" + ItemName, listObjects, pathFolder);
    }


}
