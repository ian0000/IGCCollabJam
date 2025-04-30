using UnityEngine;
using UnityEngine.UI;

public class DisplayNumber : MonoBehaviour
{
    // TODO: These are in order, should be made static or something
    [SerializeField] Sprite[] _digits;
    [SerializeField] Image _singleDigit, _ones, _tens;
    
    public void SetNumber(int number)
    {
        var digitCount = _digits.Length;
        if (number > digitCount)
        {
            _singleDigit.gameObject.SetActive(false);
            _ones.gameObject.SetActive(true);
            _tens.gameObject.SetActive(true);

            _ones.sprite = _digits[number % digitCount];
            _tens.sprite = _digits[number / digitCount];
        }
        else
        {
            if (_ones != null) _ones.gameObject.SetActive(false);
            if (_tens != null) _tens.gameObject.SetActive(false);

            _singleDigit.gameObject.SetActive(true);
            _singleDigit.sprite = _digits[number];
        }
    }
}
