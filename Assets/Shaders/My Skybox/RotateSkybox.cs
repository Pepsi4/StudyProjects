using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class RotateSkybox : MonoBehaviour
{
    private Material _skyboxMat;

    private void Start()
    {
        RenderSettings.skybox.SetFloat("_Rotation", 0f);
        var tween = RenderSettings.skybox.DOFloat(360f, "_Rotation", 360);
        tween.SetEase(Ease.Linear);

        tween.SetLoops(-1, LoopType.Incremental);
    }


}
