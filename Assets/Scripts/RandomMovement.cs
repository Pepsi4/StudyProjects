using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _moveDir;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay = 2f;

    private float _startX, _startY, _startZ;
    IEnumerator SetRandomMovementDirection(float delay)
    {
        var x = _startX + Random.Range(-1, 1);
        var y = _startY + Random.Range(-1, 1);
        var z = _startZ + Random.Range(-1, 1);

        _moveDir = new Vector3(x, y, z);
        yield return new WaitForSeconds(delay);
        StartCoroutine(SetRandomMovementDirection(delay));
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetRandomMovementDirection(_delay));
        _startY = transform.position.y;
        _startX = transform.position.x;
        _startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _moveDir, _speed * Time.deltaTime);
    }
}
