using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bastion
{
    public class SetPlayerPosition : MonoBehaviour
    {
        [SerializeField] Material[] _materials;

        // Update is called once per frame
        void Update()
        {
            foreach (Material mat in _materials)
            {
                mat.SetVector("_PlayerPosition", transform.position);
            }
        }
    }
}