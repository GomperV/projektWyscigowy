using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DrivingScript : MonoBehaviour
{
    public WheelScript[] wheels; //tu bêd¹ wszystkie nasze ko³a
    public float torque = 200; //moment obrotowy
    public float maxSteerAngle = 30; //maksymalny k¹t wychylenia
    public float maxBrakeTorque = 500; //moment hamowania
    public float maxSpeed = 150; //max prêdkoœæ
    public Rigidbody rb;
    public float currentSpeed; //aktualna prêdkoœæ
    public GameObject backLights;
    public TMP_Text speedText;
    public AudioSource engineSound;
    float rpm;
    public int currentGear = 1;
    public float currentGearPerc;
    public int numGears = 5;
    public float gearLength = 5f;
    public GameObject cameraTarget;
    // Metoda odpowiedzialna za generowanie dŸwiêku silnika pojazdu w zale¿noœci od aktualnego biegu oraz prêdkoœci.
    public void EngineSound()
    {
        float gearPercentage = 1 / (float)numGears;

        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage *
        (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed));

        currentGearPerc = Mathf.Lerp(currentGearPerc, targetGearFactor, Time.deltaTime * 5f);

        var gearNumFactor = currentGear / (float)numGears;
        rpm = Mathf.Lerp(gearNumFactor, 1, currentGearPerc);

        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed);
        //obliczenie maksymalnej prêdkoœci do zmiany biegu w górê i w dó³
        float upperGearMax = (1 / (float)numGears * (currentGear + 1));
        float downGearMax = (1 / (float)numGears * currentGear);

        if(currentGear > 0 && speedPercentage < downGearMax)
        {
            currentGear--;
        }
        if (speedPercentage > upperGearMax && currentGear < (numGears - 1))
        {
            currentGear++;
        }

        float pitch = Mathf.Lerp(1, 6, rpm);

        engineSound.pitch = Mathf.Min(6, pitch) * 0.35f;
    }
    public void Drive(float accel, float brake, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 1) * maxBrakeTorque;

        if(brake != 0)
        {
            backLights.SetActive(true);
        } else
        {
            backLights.SetActive(false);
        }

        float thrustTorque = 0;
        currentSpeed = rb.velocity.magnitude * 5;
        //speedText.text = "SPEED: " + Mathf.Round(currentSpeed) + " km/h";
        if (currentSpeed < maxSpeed)
        {
            thrustTorque = accel * torque;
        }

        foreach(WheelScript wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = thrustTorque;
            if(wheel.frontWheel)
            {
                wheel.wheelCollider.steerAngle = steer;
            } else
            {
                wheel.wheelCollider.brakeTorque = brake;
            }
            Quaternion quat;
            Vector3 position;
            wheel.wheelCollider.GetWorldPose(out position, out quat);
            wheel.wheel.transform.position = position;
            wheel.wheel.transform.rotation = quat;
        }
    }
}
