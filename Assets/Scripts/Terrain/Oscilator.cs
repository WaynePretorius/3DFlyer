using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscilator : MonoBehaviour
{
    [SerializeField] private Vector3 obstacleMovementRange;
    [SerializeField] private float period = 2f;

    [Range(0, 1)] [SerializeField] private float moveMentFactor;

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        MoveObstacle();
    }

    private void MoveObstacle()
    {
        if (period <= Mathf.Epsilon) { return; }
        GetOscilatorValues();

        Vector3 offset = obstacleMovementRange * moveMentFactor;
        transform.position = startPos + offset;
    }

    private void GetOscilatorValues()
    {
        const float getRightValue = 2f;
        const float addToGetFullNumbers = 0.5f;

        float cycles = Time.time / period;

        const float tau = Mathf.PI * getRightValue;

        float rawSinWave = Mathf.Sin(tau * cycles);
        moveMentFactor = rawSinWave * getRightValue + addToGetFullNumbers;
    }
}
