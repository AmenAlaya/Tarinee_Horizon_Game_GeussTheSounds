using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UPersian.Components;

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

    public RtlText anwerTxt;

    public AudioClip wrongSfx, rightSfx;

    private int NumberOfEmelentsInLevel;
    private float _health = 2;
    private float _maxHealth;

    public Game_Over_2_GameInteraction gameOverManager;

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

    private void Start()
    {
        if (Game_Over_2_LevelManager.isTuto)
        {
            Spawn = false;
            tutorialPanel.SetActive(true);
        }
        _maxHealth = _health;
        _lvlCounter = 0;
        if (Game_Over_2_LevelManager.isLevel)
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

        if (Spawn && !Game_Over_2_LevelManager.isPaused)
        {
            Fill_Lists();
            StartCoroutine(Start_The_Phase(timeBettwenAudioClips, timeBeforeSpawn));
            Spawn = false;
        }
    }

    private void Update()
    {
        Timer();
        Health_Progress();
        if (!Game_Over_2_LevelManager.isLevel)
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

    private void Incrice_The_Number_Of_Element()
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

    private void Clear_List_Of_Right_Indexes()
    {
        if (_ListOfRightIndex.Count == listOfSprites.Count / 2)
            _ListOfRightIndex.Clear();
    }

    private IEnumerator Start_The_Phase(float timeOfPlaySounds, float timeForSpawnImages)
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

    private void Play_Sound(int index)
    {
        _audioSource.PlayOneShot(_listOfAudioClipsInThePhase[index]);
    }

    private void Stop_Sound(int index)
    {
        _audioSource.Stop();
        _listOfAudioClipsInThePhase.RemoveAt(index);
    }

    private void Spawn_Manager()
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

    private void Fill_Lists()
    {
        if (!Game_Over_2_LevelManager.isLevel)
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

    private int Random_Index()
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

    private int Randomize_Sounds()
    {
        int randSound = Random.Range(0, _listOfAudioClipsInThePhase.Count);
        return randSound;
    }

    public void Answer_Btn(int index)
    {
        if (index == _indexOfTheRightAnswer)
        {
            hiden.Hide_The_Image(false);
            if (!Game_Over_2_LevelManager.isLevel)
            {
                _score = Score_Manager();
                Set_Score();
                _timer = time;
                _health = _maxHealth;
            }
            if (!Game_Over_2_OptionPanel.sfxMuted)
                _audioSource.PlayOneShot(rightSfx);

            Debug.Log(listOfAudioClips[index].name);
            anwerTxt.text = listOfAudioClips[index].name;
        }
        else
        {
            hiden.Hide_The_Image(false);
            if (!Game_Over_2_OptionPanel.sfxMuted)
                _audioSource.PlayOneShot(wrongSfx);
            _health = Health((int)_health);
            anwerTxt.text = "خطأ";
        }

        Continue_Game();

        _lvlCounter++;
        playSfx = false;
        _timerIsRunning = false;
        Spawn = true;
    }

    private IEnumerator Clear_Lists()
    {
        for (int i = 0; i < _lisOfcards.Count; i++)
        {
            Destroy(_lisOfcards[i]);
        }
        answerPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        hiden.Hide_The_Image(true);
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
            Debug.Log(listOfAudioClips[_indexOfTheRightAnswer].name + " " + _indexOfTheRightAnswer);
            StartCoroutine(Count_Timer());
        }
    }

    private IEnumerator Count_Timer()
    {
        _audioSource.PlayOneShot(listOfAudioClips[_indexOfTheRightAnswer]);
        playSfx = false;
        yield return new WaitForSeconds(2f);
        playSfx = true;
        _audioSource.Stop();
    }

    private void Timer()
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

    private void Timer_Fill_Amont()
    {
        _progress = _timer / time;
        timerImg.fillAmount = _progress;
    }

    private void Health_Progress()
    {
        _healthProgress = (_health + 1) / (_maxHealth + 1);
        healthImg.fillAmount = Mathf.Lerp(healthImg.fillAmount, _healthProgress, Time.deltaTime * _progressSpeed);
    }

    private void LevelManager()
    {
        switch (Game_Over_2_LevelManager.levelIndex)
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

    private int Health(int health)
    {
        int h = health;
        h--;
        return h;
    }

    private void WinLoseBeBehaviour()
    {
        if (_timer <= 0 || _health < 0)
        {
            if (Game_Over_2_LevelManager.isLevel)
            {
                gameOverManager.Game_Over(false);
            }
            else
            {
                gameOverManager.Score_Handeler((int)_totalScore);
            }
            _endGame = true;
        }
        else
        {
            if (Game_Over_2_LevelManager.isLevel)
            {
                if (_lvlCounter > numberOfSoundsInTheLvl)
                {
                    gameOverManager.Game_Over(true, NumberOfStars((int)_health));
                    _endGame = true;
                }
            }
        }
    }

    private int NumberOfStars(int numHealth)
    {
        numHealth++;
        return numHealth;
    }

    private int Randomize_Right_Index()
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

    private float Score_Manager()
    {
        float score = 100;

        score = (int)((score * _timer) / time);
        return score;
    }

    private void Set_Score()
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

    private void Continue_Game()
    {
        if (Game_Over_2_LevelManager.isTuto)
        {
            Game_Over_2_LevelManager.isTuto = false;
            tutorialPanel.SetActive(false);
        }
    }

    public void Stop_Tutorial()
    {
        if (Game_Over_2_LevelManager.isTuto)
            Game_Over_2_LevelManager.isTuto = false;
    }
}