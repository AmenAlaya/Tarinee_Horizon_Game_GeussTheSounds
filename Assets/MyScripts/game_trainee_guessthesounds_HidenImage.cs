using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class game_trainee_guessthesounds_HidenImage : MonoBehaviour
{
    public Image HidenImage;
    public game_trainee_guessthesounds_GameManager gameMan;

    private Sprite _mySprite;

    public IEnumerator UnhideCard()
    {

        while (transform.eulerAngles.y <= 178f)
        {
            transform.Rotate(0, 300 * Time.deltaTime, 0);

            _mySprite = gameMan.listOfSprites[gameMan._indexOfTheRightAnswer];
            if (transform.eulerAngles.y > 90)
            {
                HidenImage.sprite = _mySprite;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator Hide_Card_Coroutine()
    {
        while (transform.eulerAngles.y >= 2)
        {
            transform.Rotate(0, -300 * Time.deltaTime, 0);

            _mySprite = gameMan.baffel;

            if (transform.eulerAngles.y > 90)
            {
                HidenImage.sprite = _mySprite;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
