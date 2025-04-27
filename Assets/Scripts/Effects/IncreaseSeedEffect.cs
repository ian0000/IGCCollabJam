using UnityEngine;

public class IncreaseSeedEffect : MonoBehaviour
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
