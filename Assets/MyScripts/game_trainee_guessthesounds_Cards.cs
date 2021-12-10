using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_trainee_guessthesounds_Cards : MonoBehaviour
{
    [HideInInspector]
    public Sprite mySprite;
    [HideInInspector]
    public int myIndex;
    public Image myImage;

    private game_trainee_guessthesounds_GameManager _gameMan;

    void Start()
    {
        myImage.sprite = mySprite;
        Debug.Log(myIndex);
    }
}
