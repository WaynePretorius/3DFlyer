using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

    [SerializeField] private float thrustSpeed = 100f;
    [SerializeField] private float rotationSpeed = 100f;

    private Rigidbody myBody;
    private AudioSource shipAudio;

    private bool audioIsPlaying = false;

    private void Awake()
    {
        AwakeCacheFunctions();
    }

    private void AwakeCacheFunctions()
    {
        myBody = GetComponent<Rigidbody>();
        shipAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        RotateShip();
        PlayAudio();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float thrustBoost = thrustSpeed * Time.deltaTime;

            myBody.AddRelativeForce(Vector3.up * thrustBoost);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioIsPlaying = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioIsPlaying = false;
        }
    }

    private void RotateShip()
    {
        myBody.freezeRotation = true;

        float rotationThrust = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThrust);
        }

        myBody.freezeRotation = false;
    }

    private void PlayAudio()
    {
        if (audioIsPlaying)
        {
            if (!shipAudio.isPlaying)
            {
                shipAudio.Play();
            }
        }
        else
        {
            shipAudio.Stop();
        }
    }
}
