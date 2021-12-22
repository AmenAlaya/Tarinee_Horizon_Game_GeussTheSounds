using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class game_trainee_guessthesounds_GameManager : MonoBehaviour
{
    public List<Sprite> listOfSprites;
    public List<AudioClip> listOfAudioClips;

    private int _numberOfEllemnts;
    [HideInInspector]
    public int _indexOfTheRightAnswer;

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

    public GameObject answerPanel;
    public Image answerImg;

    public Sprite wrong, right;
    public AudioClip wrongSfx, rightSfx;

    private int NumberOfEmelentsInLevel;
    private float _health = 2;
    private float _maxHealth;

    public Game_Over_Manager gameOverManager;

    private bool _endGame = false;

    private int _lvlCounter = 0;
    public int numberOfSoundsInTheLvl;

    public Image healthImg;
    private float _healthProgress;
    public float _progressSpeed;

    private List<int> _ListOfRightIndex = new List<int>();

    private float _score;
    private float _totalScore;
    public TextMeshProUGUI _scoretxt;

    public GameObject tutorialPanel;
    public GameObject tutorialContainer;

    void Start()
    {
        if (game_trainee_guessthesounds_MenuManager.isTutorial)
        {
            Spawn = false;
            tutorialPanel.SetActive(true);
        }
        _maxHealth = _health;
        _lvlCounter = 0;
        if (Game_Over_Manager.isLevel)
            LevelManager();
        else
        {
            _scoretxt.enabled = true;
            _score = 0;
            _numberOfEllemnts = 1;
        }
        NumberOfEmelentsInLevel = _numberOfEllemnts;
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
        Health_Progress();
        if (!Game_Over_Manager.isLevel)
        {
            Incrice_The_Number_Of_Element();
            Clear_List_Of_Right_Indexes();
        }
        if (!_endGame)
            WinLoseBeBehaviour();

        if (Spawn)
        {
            if (!_endGame)
            {
                StartCoroutine(Clear_Lists());
            }
            Spawn = false;
        }
    }

    void Incrice_The_Number_Of_Element()
    {
        if (_numberOfEllemnts < 2)
        {
            if (_lvlCounter > 1)
            {
                NumberOfEmelentsInLevel++;
                _lvlCounter = 0;
            }
        }
    }

    void Clear_List_Of_Right_Indexes()
    {
        if (_ListOfRightIndex.Count == listOfSprites.Count / 2)
            _ListOfRightIndex.Clear();
    }

    IEnumerator Start_The_Phase(float timeOfPlaySounds, float timeForSpawnImages)
    {

        hidenImg.color = new Color32(0, 0, 0, 100);
        hidenImg.sprite = baffel;
        int NumberOfPhases = _listOfAudioClipsInThePhase.Count;
        for (int i = 0; i < NumberOfPhases; i++)
        {
            int ran = Randomize_Sounds();
            Play_Sound(ran);
            yield return new WaitForSeconds(timeOfPlaySounds);
            Stop_Sound(ran);
        }

        yield return new WaitForSeconds(timeForSpawnImages);
        _timerIsRunning = true;

        hidenImg.enabled = true;

        hidenImg.sprite = equationMark;
        hidenImg.color = new Color32(255, 255, 255, 100);
        Spawn_Manager();
        playSfx = true;
    }

    void Play_Sound(int index)
    {
        _audioSource.PlayOneShot(_listOfAudioClipsInThePhase[index]);
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
        if (!Game_Over_Manager.isLevel)
            _indexOfTheRightAnswer = Randomize_Right_Index();

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
            answerImg.sprite = right;
            if (!Game_Over_Manager.isLevel)
            {
                _score = Score_Manager();
                Set_Score();
                _timer = time;
                _health = _maxHealth;
            }
            if (Game_Over_Manager.sfxOn)
                _audioSource.PlayOneShot(rightSfx);
        }
        else
        {
            StartCoroutine(hiden.UnhideCard());
            if (Game_Over_Manager.sfxOn)
                _audioSource.PlayOneShot(wrongSfx);
            answerImg.sprite = wrong;
            _health = Health((int)_health);


        }

        Continue_Game();

        _lvlCounter++;
        playSfx = false;
        _timerIsRunning = false;
        Spawn = true;
    }

    IEnumerator Clear_Lists()
    {
        for (int i = 0; i < _lisOfcards.Count; i++)
        {
            Destroy(_lisOfcards[i]);
        }
        StartCoroutine(hiden.UnhideCard());
        answerPanel.SetActive(true);
        yield return new WaitForSeconds(1);
        StartCoroutine(hiden.Hide_Card_Coroutine());

        yield return new WaitForSeconds(1);

        answerPanel.SetActive(false);
        _numberOfEllemnts = NumberOfEmelentsInLevel;
        if (timeBettwenAudioClips > 1)
            timeBettwenAudioClips -= 0.1f;
        _indexOfTheRightAnswer++;
        _lisOfcards.Clear();
        _listOfIndexes.Clear();
        if (!_endGame)
        {

            Fill_Lists();
            StartCoroutine(Start_The_Phase(timeBettwenAudioClips, timeBeforeSpawn));

        }

    }

    public void Play_Rigth_Sound()
    {
        if (playSfx)
        {
            StartCoroutine(Count_Timer());
        }
    }
    IEnumerator Count_Timer()
    {
        _audioSource.PlayOneShot(listOfAudioClips[_indexOfTheRightAnswer]);
        playSfx = false;
        yield return new WaitForSeconds(2f);
        playSfx = true;
        _audioSource.Stop();
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

    void Health_Progress()
    {
        _healthProgress = (_health + 1) / (_maxHealth + 1);
        healthImg.fillAmount = Mathf.Lerp(healthImg.fillAmount, _healthProgress, Time.deltaTime * _progressSpeed);
    }

    void LevelManager()
    {
        switch (Game_Over_Manager.levelIndex)
        {
            case 0:
                _numberOfEllemnts = 1;
                _indexOfTheRightAnswer = 0;
                break;

            case 1:
                _numberOfEllemnts = 1;
                _indexOfTheRightAnswer = 6;
                break;
            case 2:
                _numberOfEllemnts = 2;
                _indexOfTheRightAnswer = 12;
                break;
            case 3:
                _numberOfEllemnts = 2;
                _indexOfTheRightAnswer = 18;
                break;
            case 4:
                _numberOfEllemnts = 2;
                _indexOfTheRightAnswer = 24;
                break;
            case 5:
                _numberOfEllemnts = 3;
                _indexOfTheRightAnswer = 30;
                break;
            case 6:
                _numberOfEllemnts = 3;
                _indexOfTheRightAnswer = 36;
                break;
        }
    }

    int Health(int health)
    {
        int h = health;
        h--;
        return h;
    }

    void WinLoseBeBehaviour()
    {

        if (_timer <= 0 || _health < 0)
        {
            if (Game_Over_Manager.isLevel)
            {
                gameOverManager.WinLoseLevelManager(false);
            }
            else
            {
                gameOverManager.In_Game_Score_Panel_Handler((int)_totalScore);
            }
            _endGame = true;
        }
        else
        {
            if (Game_Over_Manager.isLevel)
            {
                if (_lvlCounter > numberOfSoundsInTheLvl)
                {
                    gameOverManager.WinLoseLevelManager(true, NumberOfStars((int)_health));
                    _endGame = true;
                }
            }
        }


    }

    int NumberOfStars(int numHealth)
    {
        numHealth++;
        return numHealth;
    }

    int Randomize_Right_Index()
    {
        bool isTheRightIndex = false;
        while (!isTheRightIndex)
        {
            isTheRightIndex = true;
            _indexOfTheRightAnswer = Random.Range(0, listOfAudioClips.Count);
            for (int i = 0; i < _ListOfRightIndex.Count; i++)
            {
                if (_indexOfTheRightAnswer == _ListOfRightIndex[i])
                {
                    isTheRightIndex = false;
                    break;
                }
            }
        }

        _ListOfRightIndex.Add(_indexOfTheRightAnswer);

        return _indexOfTheRightAnswer;
    }

    float Score_Manager()
    {
        float score = 100;

        score = (int)((score * _timer) / time);
        return score;
    }

    void Set_Score()
    {
        _totalScore += _score;
        _scoretxt.text = _totalScore.ToString();
    }

    public void NextBtn()
    {
        tutorialContainer.SetActive(false);
        Fill_Lists();
        StartCoroutine(Start_The_Phase(timeBettwenAudioClips, timeBeforeSpawn));
    }

    void Continue_Game()
    {
        if (game_trainee_guessthesounds_MenuManager.isTutorial && game_trainee_guessthesounds_MenuManager.canContinue)
        {
            game_trainee_guessthesounds_MenuManager.isTutorial = false;
            game_trainee_guessthesounds_MenuManager.canContinue = false;
            tutorialPanel.SetActive(false);
        }
        else if (game_trainee_guessthesounds_MenuManager.isTutorial && !game_trainee_guessthesounds_MenuManager.canContinue)
        {
            game_trainee_guessthesounds_MenuManager.isTutorial = false;
            gameOverManager.HomeBtn();
        }
    }

    public void Stop_Tutorial()
    {
        if (game_trainee_guessthesounds_MenuManager.isTutorial)
            game_trainee_guessthesounds_MenuManager.isTutorial = false;
    }
}
