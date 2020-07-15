using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    enum State { Alive, Dying, Transcending}


    [SerializeField] private float thrustSpeed = 100f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float timeToNextScene = 1f;
    [SerializeField] private AudioClip mainEngine;
    [SerializeField] private AudioClip levelFinish;
    [SerializeField] private AudioClip playerDeath;
    [SerializeField] private ParticleSystem engineThrusters;
    [SerializeField] private ParticleSystem deathExplosion;
    [SerializeField] private ParticleSystem levelSuccess;

    private Rigidbody myBody;
    private AudioSource shipAudio;

    private bool audioIsPlaying = false;
    [SerializeField]private bool isColliding = true;
    private State state = State.Alive;

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
        CheckIfAlive();
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
    }

    private void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetAndRunNextScene();
        }

        if (Input.GetKey(KeyCode.C))
        {
            isColliding = !isColliding;
        }
    }

    private void CheckIfAlive()
    {
        if (state == State.Alive)
        {
            Thrust();
            RotateShip();
            PlayAudio();
        }
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
            engineThrusters.Play();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            audioIsPlaying = false;
            engineThrusters.Stop();
        }
    }

    private void RotateShip()
    {
        myBody.angularVelocity = Vector3.zero;

        float rotationThrust = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThrust);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThrust);
        }


    }

    private void PlayAudio()
    {
        if (audioIsPlaying)
        {
            if (!shipAudio.isPlaying)
            {
                shipAudio.PlayOneShot(mainEngine);
            }
        }
        else
        {
            shipAudio.Stop();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state != State.Alive) { return; }

        LoofForCollission(other);
    }

    private void LoofForCollission(Collision other)
    {
        if (other.gameObject.tag == Tags.TAG_FRIENDLY)
        {
            Debug.Log("OK");
        }
        else if (other.gameObject.tag == Tags.TAG_FINISH)
        {
            FinishLevelDetector();
        }
        else
        {
            PlayerDiedOnCollisions();
        }
    }

    private void FinishLevelDetector()
    {
        state = State.Transcending;
        shipAudio.PlayOneShot(levelFinish);
        levelSuccess.Play();
        Invoke(Tags.METHOD_NEXTSCENE, timeToNextScene);
    }

    private void PlayerDiedOnCollisions()
    {
        if (!isColliding) { return; }
        state = State.Dying;
        shipAudio.PlayOneShot(playerDeath);
        deathExplosion.Play();
        Invoke(Tags.METHOD_RESTART, timeToNextScene);
    }

    private void GetAndRunNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings;
        
        if (nextSceneIndex >= lastSceneIndex)
        {
            Restart();
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
