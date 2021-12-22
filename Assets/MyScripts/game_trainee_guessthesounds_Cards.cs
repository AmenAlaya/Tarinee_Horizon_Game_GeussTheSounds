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

    public Game_Over_Manager gameOverMan;

    public GameObject circle;


    void Start()
    {
        gameOverMan = FindObjectOfType<Game_Over_Manager>();
        myImage.sprite = _mySprite;
        if (game_trainee_guessthesounds_MenuManager.isTutorial)
            StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (_myIndex == _gameMan._indexOfTheRightAnswer)
            Instantiate(circle, transform.position, Quaternion.identity, transform);

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
