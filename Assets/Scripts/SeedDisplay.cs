using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DisplayNumber))]
public class SeedDisplay : MonoBehaviour {
    DisplayNumber _displayNumber;

    void Start() {
        _displayNumber = GetComponent<DisplayNumber>();
        SeedManager.Instance.onSeedsChanged += UpdateDisplay;
        UpdateDisplay();
    }

    void UpdateDisplay() {
        _displayNumber.SetNumber(SeedManager.Instance.currentSeeds);
    }

    void OnDestroy() {
        if (SeedManager.Instance != null)
            SeedManager.Instance.onSeedsChanged -= UpdateDisplay;
    }
}
