using UnityEngine;
using UnityEngine.UI;

public class SeedDisplay : MonoBehaviour {
    // These are in order
    [SerializeField] Sprite[] _digits;
    [SerializeField] Image _singleDigit, _ones, _tens;

    void Start() {
        SeedManager.Instance.onSeedsChanged += UpdateDisplay;
        UpdateDisplay();
    }

    void UpdateDisplay() {
        var digitCount = _digits.Length;
        var seeds = SeedManager.Instance.currentSeeds;
        if (seeds > digitCount)
        {
            _singleDigit.gameObject.SetActive(false);
            _ones.gameObject.SetActive(true);
            _tens.gameObject.SetActive(true);

            _ones.sprite = _digits[seeds % digitCount];
            _tens.sprite = _digits[seeds / digitCount];
        }
        else
        {
            _singleDigit.gameObject.SetActive(true);
            _ones.gameObject.SetActive(false);
            _tens.gameObject.SetActive(false);

            _singleDigit.sprite = _digits[seeds];
        }
    }

    void OnDestroy() {
        if (SeedManager.Instance != null)
            SeedManager.Instance.onSeedsChanged -= UpdateDisplay;
    }
}
