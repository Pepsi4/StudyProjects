using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingRadiusController : MonoBehaviour
{
    Material _mat;

    float _time, _radius, _randomValue;

    [SerializeField] float _speed;

    void Awake()
    {

    }

    private void Start()
    {
        _mat = this.GetComponent<MeshRenderer>().material;
        _mat.SetFloat("_radius_A", _radius);

        float rnd = Random.Range(0.01f, 0.04f);
        _mat.SetFloat("_radius_B", rnd);
    }

    private void Update()
    {
        _time += Time.deltaTime * _speed;

        if (_time < 1)
        {
            _radius = Mathf.Lerp(0, 1, _time);
            _mat.SetFloat("_radius_A", _radius);
        }

        if (_radius >= 0.65f)
        {
            Destroy(this.gameObject);
        }
    }
}
