using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsVR : MonoBehaviour
{
    [SerializeField] CarController carController;
    
    [Header("Steering Wheel")]

        [SerializeField] Transform steering_wheel;
        [SerializeField] float swSpeedRotation = 50;
        float swAccumulatedRotationZ = 0f;
        float swLastRotationZ;
        float swCurrentRotationZ;
        float swDeltaRotation;
        bool grabSteeringWheel = false;
        float steeringAngle;
        bool tryOn = false;
        Quaternion swInitialRot;

    [Header("Steering Lever")]

        [SerializeField] Transform lever;
        float leverAccumulatedRotationZ = 0f;
        float leverCurrentRotationZ=0;
        float leverDeltaRotation=0;
        float leverLastRotationZ=0;
        int leverAngle=0;

    [Header("Brake")]

        [SerializeField] Transform brake;
        public float brakeAccumulatedRotationZ = 0f;
        float brakeCurrentRotationZ;
        float brakeDeltaRotation;
        float brakeLastRotationZ;
        public float brakeAngle;

    float acelerate = 0;

    private void Awake()
    {

        swInitialRot = steering_wheel.localRotation;
        swLastRotationZ = steering_wheel.eulerAngles.z;

        leverLastRotationZ = lever.eulerAngles.x;

        brakeLastRotationZ = brake.eulerAngles.x;

    }

    private void Start()
    {
        leverAngle = 0;
    }

    private void FixedUpdate()
    {

        if (tryOn)
            carController.onOffController.TurningOnCar(tryOn);
        
        SterringWheelInput();
      //  LeverInput();
        BrakeInput();

        carController.UpdateAxel(acelerate, -leverAngle, -steeringAngle, brakeAngle);
    }
    public void TryOn(bool value)
    {
        tryOn = value;
    }

    #region SterromgWheel
    public void SterringWheelInput()
    {
        // Obtiene la rotación actual en Z
        swCurrentRotationZ = steering_wheel.eulerAngles.z;

        // Calcula el cambio en la rotación desde el último frame
        swDeltaRotation = swCurrentRotationZ - swLastRotationZ;

        // Detecta el "salto" de 360 a 0 grados
        if (swDeltaRotation > 180f)
        {
            swDeltaRotation -= 360f;
        }
        // Detecta el "salto" de 0 a 360 grados
        else if (swDeltaRotation < -180f)
        {
            swDeltaRotation += 360f;
        }

        // Acumula la rotación
        swAccumulatedRotationZ += swDeltaRotation;

        // Actualiza la última rotación
        swLastRotationZ = swCurrentRotationZ;

        if (!grabSteeringWheel && Mathf.RoundToInt(swAccumulatedRotationZ) != 0)
        {
            if (swAccumulatedRotationZ > 0)
            {
                RotateSteeringWheel(-swSpeedRotation);

            }
            else if (swAccumulatedRotationZ < 0)
            {
                RotateSteeringWheel(swSpeedRotation);
            }
            else
            {
                steering_wheel.eulerAngles = new Vector3(steering_wheel.eulerAngles.x, steering_wheel.eulerAngles.y, swInitialRot.z);
            }
        }
        steeringAngle = Mathf.Clamp(Map(swAccumulatedRotationZ, -360, 360, -1, 1), -1, 1);
    }
    public void Select(PointerEvent evt)
    {
        grabSteeringWheel = true;
    }

    public void Unselect(PointerEvent evt)
    {
        grabSteeringWheel = false;
    }

    public void RotateSteeringWheel(float speed)
    {
        steering_wheel.Rotate(0, 0, speed * Time.deltaTime);
    }
    #endregion
   
    #region Torque
    public void Acelerate()
    {
        if (acelerate >= 1) return;

        acelerate += 0.01f;
        acelerate = Mathf.Clamp(acelerate,-1,1);
    }

    public void Desacelerate()
    {
        if (acelerate <= 0) return;

        acelerate -= 0.01f;
        acelerate = Mathf.Clamp(acelerate, -1, 1);
    }
    #endregion

    public void LeverInput()
    {
        // Obtiene la rotación actual en Z
        leverCurrentRotationZ = lever.eulerAngles.x;

        // Calcula el cambio en la rotación desde el último frame
        leverDeltaRotation = leverCurrentRotationZ - leverLastRotationZ;
       
        // Detecta el "salto" de 360 a 0 grados
        if (leverDeltaRotation > 180f)
        {
            leverDeltaRotation -= 360f;
        }
        // Detecta el "salto" de 0 a 360 grados
        else if (leverDeltaRotation < -180f)
        {
            leverDeltaRotation += 360f;
        }

        // Acumula la rotación
        leverAccumulatedRotationZ += leverDeltaRotation;

        // Actualiza la última rotación
        leverLastRotationZ = leverCurrentRotationZ;

        leverAngle = Mathf.RoundToInt(Mathf.Clamp(Map(leverAccumulatedRotationZ, -35, 10, -1, 1), -1, 1));

    }
    public void BrakeInput()
    {
        // Obtiene la rotación actual en Z
        brakeCurrentRotationZ = brake.eulerAngles.x;

        // Calcula el cambio en la rotación desde el último frame
        brakeDeltaRotation = brakeCurrentRotationZ - brakeLastRotationZ;

        // Detecta el "salto" de 360 a 0 grados
        if (brakeDeltaRotation > 180f)
        {
            brakeDeltaRotation -= 360f;
        }
        // Detecta el "salto" de 0 a 360 grados
        else if (brakeDeltaRotation < -180f)
        {
            brakeDeltaRotation += 360f;
        }

        // Acumula la rotación
        brakeAccumulatedRotationZ += brakeDeltaRotation;

        // Actualiza la última rotación
        brakeLastRotationZ = brakeCurrentRotationZ;

        brakeAngle =  Mathf.Clamp(Map(brakeAccumulatedRotationZ, 12, 40, 0, 1), 0, 1);
    }

    public float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        // La fórmula de mapeo:
        // (valor - rango_entrada_min) * (rango_salida_max - rango_salida_min) / (rango_entrada_max - rango_entrada_min) + rango_salida_min
        return (value - fromMin) * (toMax - toMin) / (fromMax - fromMin) + toMin;
    }


}
