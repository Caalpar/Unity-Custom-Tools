using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomActionGrab : MonoBehaviour
{
    [SerializeField]
    Grabbable grabbable;

    [SerializeField]
    UnityEvent<PointerEvent> Select, Move, Cancel, Unselect, Hover, Unhover;

    private void OnEnable()
    {
        grabbable.WhenPointerEventRaised += AllEventDebug;
    }

    private void OnDisable()
    {
        grabbable.WhenPointerEventRaised += AllEventDebug;

       
    }

    private void AllEventDebug(PointerEvent evt)
    {
        switch (evt.Type)
        {
            case PointerEventType.Select:
                Select.Invoke(evt);
                break;
            case PointerEventType.Move:
                Move.Invoke(evt);
                break;
            case PointerEventType.Cancel:
                Cancel.Invoke(evt);
                break;
            case PointerEventType.Unselect:
                Unselect.Invoke(evt);
                break;
            case PointerEventType.Hover:
                Hover.Invoke(evt);
                break;
            case PointerEventType.Unhover:
                Unhover.Invoke(evt);
                break;
        }
    }
}
