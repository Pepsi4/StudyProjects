using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingInctancier : MonoBehaviour
{
    [SerializeField] GameObject _ringsInstance;
    [SerializeField] float _time;
    void Awake()
    {
        InvokeRepeating(nameof(InstantinateRings),1, _time);
    }

    void InstantinateRings()
    {
        GameObject _rings = Instantiate(_ringsInstance, transform);
    }
}
