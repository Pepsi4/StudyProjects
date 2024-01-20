using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class OffsetAnimation : MonoBehaviour
{
    [SerializeField] Vector2 _offset;
    [SerializeField] float _duration;

    void Start()
    {
        Debug.Log("Start");
        Debug.Log(_offset + " " + _duration);

        var tween = gameObject.GetComponent<MeshRenderer>().material.DOOffset(_offset, _duration);
        tween.SetEase(Ease.Linear);
        tween.SetLoops(-1, LoopType.Incremental);
    }
}
