using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGenerator : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Transform appleBoard;
    [SerializeField] private ObjectPool objectPool;

    public GameObject GenerateApple()
    {
        GameObject appleObject = objectPool.GetObject();
        appleObject.TryGetComponent(out Apple apple);
        apple.SetNumber();
        return appleObject;
    }
}
