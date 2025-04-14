using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedDisplay : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _seedText;

    void Start() {
        StartCoroutine(WaitForSeedManager());
    }

    System.Collections.IEnumerator WaitForSeedManager() {
        // Wait until SeedManager.Instance is available
        while (SeedManager.Instance == null)
            yield return null;

        // Subscribe to seed change updates and update display once initially
        SeedManager.Instance.onSeedsChanged += UpdateDisplay;
        UpdateDisplay();
    }

    void UpdateDisplay() {
        _seedText.text = $"Seeds: {SeedManager.Instance.currentSeeds}/{SeedManager.Instance.maxSeeds}";
    }

    void OnDestroy() {
        if (SeedManager.Instance != null)
            SeedManager.Instance.onSeedsChanged -= UpdateDisplay;
    }
}
