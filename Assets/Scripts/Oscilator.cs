using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    [SerializeField] private Vector3 obstacleMovementRange;
    [Range(0, 1)] [SerializeField] private float tempMoveBar;
}
