using UnityEngine;

public class IncreaseSeedEffect : MonoBehaviour
{
    SeedManager _seedManager;

    void Start()
    {
        _seedManager = FindObjectOfType<SeedManager>();
        _seedManager.MaxSeeds++;
        _seedManager.AddSeed();
    }

    void OnDestroy()
    {
        _seedManager.MaxSeeds--;
    }
}
