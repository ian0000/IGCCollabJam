using UnityEngine;
using UnityEngine.UI;

public class SeedDisplay : MonoBehaviour {
    // These are in order
    [SerializeField] Sprite[] _digits;

    // may have either one
    SpriteRenderer _spriteRenderer;
    Image _image;

    void Start() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _image = GetComponent<Image>();
        SeedManager.Instance.onSeedsChanged += UpdateDisplay;
        UpdateDisplay();
    }

    void UpdateDisplay() {
        var digit = _digits[SeedManager.Instance.currentSeeds];
        if (_spriteRenderer != null)
            _spriteRenderer.sprite = digit;
        if (_image != null)
            _image.sprite = digit;

    }

    void OnDestroy() {
        if (SeedManager.Instance != null)
            SeedManager.Instance.onSeedsChanged -= UpdateDisplay;
    }
}
