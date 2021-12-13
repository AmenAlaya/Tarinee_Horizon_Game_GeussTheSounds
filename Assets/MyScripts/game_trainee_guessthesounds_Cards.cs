using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_trainee_guessthesounds_Cards : MonoBehaviour
{
    private Sprite _mySprite;
    private int _myIndex;
    public Image myImage;

    private game_trainee_guessthesounds_GameManager _gameMan;

    private bool _checkAnswer = false;

    void Start()
    {
        myImage.sprite = _mySprite;
    }

    public void Set_Me(Sprite sprite, int index)
    {
        _mySprite = sprite;
        _myIndex = index;
    }

    void Update()
    {
        if (_checkAnswer)
        {
            _gameMan.Answer_Btn(_myIndex);
            _checkAnswer = false;
        }
    }

    public void Answer_Btn()
    {
        _checkAnswer = true;
    }

    public void Set_Game_Manager(game_trainee_guessthesounds_GameManager gameMan)
    {
        this._gameMan = gameMan;
    }
}
