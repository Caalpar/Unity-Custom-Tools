using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public GameObject visualLeftWheel;
    public GameObject visualRightWheel;
    public bool motor;
    public bool steering;
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;

    [Header("Parameters")]
    [SerializeField] Rigidbody rbForkLift;
    [SerializeField] float maxVelocity = 10;

    [Header("Motor")]
    [Range(-1, 1)] public float torque;
    public float maxTorque;
    [Range(-1, 1)] public int transmisión;
    public OnOffController onOffController;
    [SerializeField] AudioSource audioEngine;
    [SerializeField] float minPitch = 0.05f;
    [SerializeField] float pitchFormCar;
    [SerializeField] float frictionalBrake = 20;

    [Header("Steering Angle")]
    public float maxAngle;
    [Range(-1, 1)] public float angle;

    [Header("Break")]
    public float maxMotorBreak;
    [Range(0, 1)] public float mBreak;
    float inputBrak;

    [Header("Debug")]
    public bool debug = false;

    // TODO: separar logica de aceleracion , frenos y angulo de las ruedas por que se puede mover las ruedas cuando esta apagado y se puede apretar el freno tambien

    private void Start()
    {
        audioEngine.pitch = minPitch;
        rbForkLift.centerOfMass = new Vector3(0, -1, 0);
    }

    private void Update()
    {
        if(debug)
            UpdateAxel(torque, transmisión, angle, mBreak);
    }

    public void UpdateAxel(float motorTorque, int march, float steeringAngle, float motorBreak)
    {
        if (!onOffController.IsOn) return;   

        for (int i = 0; i < axleInfos.Count; i++)
        {
            AxleInfo axel = axleInfos[i];



            if (axel.motor && CurrentVelocityKPH() <= maxVelocity)
            {
  
                // manejar el control de velocidad desde aca
                axel.leftWheel.motorTorque = maxTorque * motorTorque * march;
                axel.rightWheel.motorTorque = maxTorque * motorTorque * march;

            }
            else
            {
                axel.leftWheel.motorTorque = 0;
                axel.rightWheel.motorTorque = 0;
            }

            if (axel.steering)
            {
                axel.leftWheel.steerAngle = maxAngle * steeringAngle;
                axel.rightWheel.steerAngle = maxAngle * steeringAngle;
            }

            inputBrak = maxMotorBreak * motorBreak;

            if (motorTorque <= 0)
            {
                axel.leftWheel.brakeTorque = inputBrak + frictionalBrake;
                axel.rightWheel.brakeTorque = inputBrak + frictionalBrake;
            }  
            else
            {
                axel.leftWheel.brakeTorque = inputBrak;
                axel.rightWheel.brakeTorque = inputBrak;
            }



            UpdateVisualAxel(axel.leftWheel, axel.visualLeftWheel.transform);
            UpdateVisualAxel(axel.rightWheel, axel.visualRightWheel.transform);
        }
        AudioEngine(motorTorque);
    }

    public void AudioEngine(float value) {

        pitchFormCar = (MapPercentageToRange(value, 1, 2.5f) + MapPercentageToRange(CurrentVelocityKPH() / maxVelocity,1,2.5f)) * 0.5f;// (CurrentVelocityKPH() / maxVelocity) + (maxPitch * Time.deltaTime);  
        if (pitchFormCar < minPitch)
            audioEngine.pitch = minPitch; 
        else
            audioEngine.pitch = pitchFormCar;
    }


    public float CurrentVelocityKPH() { return (float)Math.Round(rbForkLift.velocity.magnitude * 3.6f, 0); }

    private void UpdateVisualAxel(WheelCollider collider, Transform visualWheel)
    {
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
        visualWheel.position = position;
        visualWheel.rotation = rotation;
    }


    public float MapPercentageToRange(float percentage, float minValue, float maxValue)
    {
        // Clampeamos el porcentaje para asegurarnos de que esté entre 0 y 1
        percentage = Mathf.Clamp(percentage, 0f, 1f);

        // Convertimos el porcentaje al rango deseado
        return minValue + percentage * (maxValue - minValue);
    }

}
