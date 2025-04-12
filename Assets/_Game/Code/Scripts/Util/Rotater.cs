using System;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private Vector3 rotation;

    private void Update()
    {
        transform.Rotate(rotation * Time.deltaTime, Space.Self);
    }
}