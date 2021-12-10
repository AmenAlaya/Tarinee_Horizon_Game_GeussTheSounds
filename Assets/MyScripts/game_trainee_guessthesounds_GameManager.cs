using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class game_trainee_guessthesounds_GameManager : MonoBehaviour
{
    public List<Sprite> listOfSprites;
    public List<AudioClip> listOfAudioClips;

    [SerializeField]
    private int _numberOfEllemnts;// Makes sure to hide in the inspector later
    [SerializeField]
    private int _indexOfTheRightAnswer; // Makes sure to hide in the inspector later

    private List<Sprite> _listOfSpritesInThePhase = new List<Sprite>();
    private List<AudioClip> _listOfAudioClipsInThePhase = new List<AudioClip>() ;
    private List<int> _listOfIndexes = new List<int>();

    public GameObject btnPrefab;
    public Transform panel;

    private List<game_trainee_guessthesounds_Cards> _listOfCrads = new List<game_trainee_guessthesounds_Cards>();

    void Start()
    {       
        Spawn_Manager();
    }

    void Update()
    {

    }

    void Spawn_Manager()
    {
        Fill_Lists();
        _numberOfEllemnts++;
        for (int i = 0; i < _numberOfEllemnts; i++)
        {
            GameObject newBtn = Instantiate(btnPrefab, panel);
            int randomIndex = Random.Range(0, _listOfSpritesInThePhase.Count);
            newBtn.GetComponent<game_trainee_guessthesounds_Cards>().mySprite = _listOfSpritesInThePhase[randomIndex];
            newBtn.GetComponent<game_trainee_guessthesounds_Cards>().myIndex = _listOfIndexes[randomIndex];
            _listOfIndexes.RemoveAt(randomIndex);
            _listOfSpritesInThePhase.RemoveAt(randomIndex);           
        }
    }

    void Fill_Lists()
    {
        _listOfSpritesInThePhase.Add(listOfSprites[_indexOfTheRightAnswer]);
        _listOfAudioClipsInThePhase.Add(listOfAudioClips[_indexOfTheRightAnswer]);
        _listOfIndexes.Add(_indexOfTheRightAnswer);

        for (int i = 0; i < _numberOfEllemnts; i++)
        {
            int indexOfSprite = Random_Index();
            _listOfSpritesInThePhase.Add(listOfSprites[indexOfSprite]);
            _listOfIndexes.Add(indexOfSprite);
            int indexOfAudioClip = Random_Index();
            _listOfAudioClipsInThePhase.Add(listOfAudioClips[indexOfAudioClip]);
            _listOfIndexes.Add(indexOfAudioClip);
        }

    }

    int Random_Index()
    {
        bool checkIndex = false;
        int ran = 0;
        while (!checkIndex)
        {
            checkIndex = true;
            ran = Random.Range(0, listOfAudioClips.Count);

            for (int i = 0; i < _listOfIndexes.Count; i++)
            {
                if (ran == _listOfIndexes[i])
                {
                    checkIndex = false;
                    break;
                }
            }
        }

        return ran;
    }

    public void Answer_Btn()
    {
        if(btnPrefab.GetComponent<game_trainee_guessthesounds_Cards>().myIndex == _indexOfTheRightAnswer)
        {
            Debug.Log("GoodJob !");
        }
        else
        {
            Debug.Log("You Fail");
        }
    }

}
