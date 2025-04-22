using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeSeedEffect : MonoBehaviour
{
    SeedManager _seedManager;

    void Start()
    {
        _seedManager = FindObjectOfType<SeedManager>();
        _seedManager.MaxSeeds++;
    }

    void ODestroy()
    {
        _seedManager.MaxSeeds--;
    }
}
