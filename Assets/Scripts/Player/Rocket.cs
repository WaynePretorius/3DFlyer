using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

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
    private LevelManager levelManager;

    private bool audioIsPlaying = false;
    private bool isColliding = true;
   
    private State state = State.Alive;

    private void Awake()
    {
        AwakeCacheFunctions();
    }

    private void AwakeCacheFunctions()
    {
        myBody = GetComponent<Rigidbody>();
        shipAudio = GetComponent<AudioSource>();
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Debug.isDebugBuild)
        {
            DebugKeys();
        }
    }

    private void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            levelManager.LoadGetNextScene();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isColliding = !isColliding;
        }
    }

    private void Move()
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

        ShipFX();
    }

    private void ShipFX()
    {
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
        if (state != State.Alive || !isColliding) { return; }
        LookForCollission(other);
    }

    private void LookForCollission(Collision other)
    {
        if (other.gameObject.tag == Tags.TAG_FRIENDLY)
        {
            Debug.Log("OK");//Pickups
        }
        else if (other.gameObject.tag == Tags.TAG_FINISH)
        {
            StartCoroutine(FinishLevelDetector());
        }
        else
        {
            StartCoroutine(PlayerDiedOnCollisions());
        }
    }

    private IEnumerator FinishLevelDetector()
    {
        state = State.Transcending;
        shipAudio.PlayOneShot(levelFinish);
        levelSuccess.Play();
        yield return new WaitForSeconds(timeToNextScene);
        levelManager.LoadGetNextScene();
    }

    private IEnumerator PlayerDiedOnCollisions()
    {
        state = State.Dying;
        shipAudio.PlayOneShot(playerDeath);
        deathExplosion.Play();
        yield return new WaitForSeconds(timeToNextScene);
        levelManager.RestartLevel();
    }

}
