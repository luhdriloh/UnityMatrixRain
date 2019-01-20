using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterStream : MonoBehaviour
{
    public Font _fontToUse;

    public Color _startColor;
    public Color _endColor;

    public int _fontSizeMin;
    public int _fontSizeMax;

    private float _fallSpeed;
    private int _fontSize;

    private float _screenHeight;
    private float _screenWidth;

    private int _numLettersToFadeToBase;
    private int _lettersFromBackToStartFading;
    private int _maxLength;

    private GUIStyle _guiStyle;
    private Vector2 _positionOnScreen;
    private string _stringtoDisplay;

    private char[] _values = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890$+-*/%=\"'#&_(),.;:?!\\|{}<>[]^~".ToCharArray();

    private float _currentTime;

	private void Start ()
    {
        _numLettersToFadeToBase = 1;
        _screenHeight = Camera.main.scaledPixelHeight;
        _screenWidth = Camera.main.scaledPixelWidth;

        _guiStyle = new GUIStyle();
        _guiStyle.font = _fontToUse;

        InitializeBackToTop();
	}

    private void InitializeBackToTop()
    {
        _fontSize = Random.Range(8, 15);
        _guiStyle.fontSize = _fontSize;
        _fallSpeed = Random.Range(.005f, .1f);
        _maxLength = Random.Range(10, 30);
        _lettersFromBackToStartFading = Random.Range(3, 8);

        float yPos = Random.Range(-(_screenHeight), 0);
        float xPos = Random.Range(_fontSize, _screenWidth - _fontSize);

        _stringtoDisplay = string.Empty;
        _positionOnScreen = new Vector2(xPos, yPos);
    }

    private void Update ()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _fallSpeed)
        {
            int newCharacter = Random.Range(0, _values.Length);
            _stringtoDisplay = _values[newCharacter] + _stringtoDisplay;

            if (_stringtoDisplay.Length > _maxLength)
            {
                _stringtoDisplay = _stringtoDisplay.Substring(0, _maxLength);
            }

            UpdatePosition();
            _currentTime = 0f;

            if (OutOfView())
            {
                InitializeBackToTop();
            }
        }
	}

    private void OnGUI()
    {
        DrawStream();
    }

    private void UpdatePosition()
    {
        _positionOnScreen += new Vector2(0, _fontSize);
    }

    private void DrawStream()
    {
        Vector2 positionToDrawLetter;

        char[] toDisplay = _stringtoDisplay.ToCharArray();
        for (int i = 0; i < toDisplay.Length; i++)
        {
            _guiStyle.normal.textColor = _endColor;
            if (i < _numLettersToFadeToBase)
            {
                _guiStyle.normal.textColor = Color.Lerp(_startColor, _endColor, .25f * i);
            }

            if (toDisplay.Length >= _maxLength)
            {
                int indexFromBack = i - (toDisplay.Length - _lettersFromBackToStartFading);

                if (indexFromBack >= 0)
                {
                    Color colorInUse = _guiStyle.normal.textColor;
                    colorInUse.a = 1 - ((1f / _lettersFromBackToStartFading) * indexFromBack);
                    _guiStyle.normal.textColor = colorInUse;
                }
            }

            positionToDrawLetter = new Vector2(_positionOnScreen.x, _positionOnScreen.y - (_fontSize * i));
            GUI.Label(new Rect(positionToDrawLetter.x, positionToDrawLetter.y, 100, 100), toDisplay[i].ToString(), _guiStyle);
        }
    }

    private bool OutOfView()
    {
        float yPos = _positionOnScreen.y - (_fontSize * _stringtoDisplay.Length);
        return yPos >= _screenHeight;
    }
}
