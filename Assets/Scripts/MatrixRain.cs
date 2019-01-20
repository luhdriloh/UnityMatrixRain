using UnityEngine;

public class MatrixRain : MonoBehaviour
{
    public Color _startColor;
    public Color _endColor;

    public Font _fontToUse;

    public int _numberOfStreams;
    public GameObject _letterStreamPrototype;

    private void Start()
    {
        for (int i = 0; i < _numberOfStreams; i++)
        {
            GameObject letterStreamObject = Instantiate(_letterStreamPrototype, transform.position, Quaternion.identity);
            LetterStream letterStream = letterStreamObject.GetComponent<LetterStream>();

            letterStream._fontToUse = _fontToUse;
            letterStream._startColor = _startColor;
            letterStream._endColor = _endColor;
        }
    }
}
