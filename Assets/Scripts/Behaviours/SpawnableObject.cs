using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SpawnableObject
{
    [Range(1, 100)] public int Rate;
    public GameObject Prefab;
}
