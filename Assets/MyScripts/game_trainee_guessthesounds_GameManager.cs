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

    public int _indexOfTheRightAnswer; // Makes sure to hide in the inspector later

    private List<Sprite> _listOfSpritesInThePhase = new List<Sprite>();
    private List<AudioClip> _listOfAudioClipsInThePhase = new List<AudioClip>();
    private List<int> _listOfIndexes = new List<int>();

    public GameObject btnPrefab;
    public Transform panel;

    public float timeBettwenAudioClips = 1, timeBeforeSpawn = 1;

    private AudioSource _audioSource;

    public bool Spawn = true;
    private bool playSfx = false;

    public Image hidenImg;
    public Sprite baffel, equationMark;

    public float time;
    public Image timerImg;

    private bool _timerIsRunning = false;
    private float _timer;
    private float _progress;

    private List<GameObject> _lisOfcards = new List<GameObject>();

    private float timeLeft;
    public float reloadTime;

    public game_trainee_guessthesounds_HidenImage hiden;


    void Start()
    {

        _timer = time;
        _audioSource = GetComponent<AudioSource>();
        if (Spawn)
        {
            Fill_Lists();
            StartCoroutine(Start_The_Phase(timeBettwenAudioClips, timeBeforeSpawn));
            Spawn = false;
        }
    }

    void Update()
    {

        Timer();
    }

    IEnumerator Start_The_Phase(float timeOfPlaySounds, float timeForSpawnImages)
    {       
        hidenImg.color = new Color32(0, 0, 0, 100);
        hidenImg.sprite = baffel;
        int NumberOfPhases = _listOfAudioClipsInThePhase.Count;
        for (int i = 0; i < NumberOfPhases; i++)
        {
            int ran = Randomize_Sounds();
            Play_Sound(ran, timeOfPlaySounds);
            yield return new WaitForSeconds(timeOfPlaySounds);
            Stop_Sound(ran);
        }

        yield return new WaitForSeconds(timeForSpawnImages);
        _timerIsRunning = true;
        hidenImg.sprite = equationMark;
        hidenImg.color = new Color32(255, 255, 255, 100);
        Spawn_Manager();
        playSfx = true;
    }

    void Play_Sound(int index, float time)
    {
        _audioSource.PlayOneShot(_listOfAudioClipsInThePhase[index], time);
    }

    void Stop_Sound(int index)
    {
        _audioSource.Stop();
        _listOfAudioClipsInThePhase.RemoveAt(index);
    }

    void Spawn_Manager()
    {
        _numberOfEllemnts++;
        for (int i = 0; i < _numberOfEllemnts; i++)
        {
            GameObject newBtn = Instantiate(btnPrefab, panel);
            _lisOfcards.Add(newBtn);
            newBtn.GetComponent<game_trainee_guessthesounds_Cards>().Set_Game_Manager(this);
            int randomIndex = Random.Range(0, _listOfSpritesInThePhase.Count);
            newBtn.GetComponent<game_trainee_guessthesounds_Cards>().Set_Me(_listOfSpritesInThePhase[randomIndex], _listOfIndexes[randomIndex]);
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

    int Randomize_Sounds()
    {
        int randSound = Random.Range(0, _listOfAudioClipsInThePhase.Count);
        return randSound;
    }

    public void Answer_Btn(int index)
    {
        if (index == _indexOfTheRightAnswer)
        {
            StartCoroutine(hiden.UnhideCard());
        }
        else
        {
            StartCoroutine(hiden.UnhideCard());
        }

        playSfx = false;
        _timerIsRunning = false;

        StartCoroutine(Clear_Lists());
    }

    IEnumerator Clear_Lists()
    {
        for (int i = 0; i < _lisOfcards.Count; i++)
        {
            Destroy(_lisOfcards[i]);
        }
        StartCoroutine(hiden.UnhideCard());
        yield return new WaitForSeconds(1);
        StartCoroutine(hiden.Hide_Card_Coroutine());
        yield return new WaitForSeconds(1);
        _numberOfEllemnts = 2;
        _indexOfTheRightAnswer++;
        _lisOfcards.Clear();
        _listOfIndexes.Clear();
        Spawn = true;
        if (Spawn)
        {
            Fill_Lists();
            StartCoroutine(Start_The_Phase(timeBettwenAudioClips, timeBeforeSpawn));
            Spawn = false;
        }

    }

    public void Play_Rigth_Sound()
    {
        if (playSfx)
        {
            _audioSource.PlayOneShot(listOfAudioClips[_indexOfTheRightAnswer]);
            playSfx = false;
            timeLeft = 0;
            StartCoroutine(CountTimer());
        }



    }
    IEnumerator CountTimer()
    {
        while (!playSfx)
        {
            yield return new WaitForSeconds(1f);

            timeLeft++;

            if (timeLeft > reloadTime)
                playSfx = true;

        }
    }

    void Timer()
    {
        if (_timerIsRunning)
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
                Timer_Fill_Amont();


            }
            else
            {
                _timer = 0;
                _timerIsRunning = false;
            }
        }
    }

    void Timer_Fill_Amont()
    {
        _progress = _timer / time;
        timerImg.fillAmount = _progress;
    }

}
