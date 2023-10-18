using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTransform : MonoBehaviour
{
    [SerializeField] Transform _parentTransform;

    private void Start()
    {
        SetParent(_parentTransform);
    }

    private void SetParent(Transform newTransform)
    {
        transform.parent = newTransform;
    }

    private void DetachFromParent()
    {
        transform.parent = null;
    }
}
