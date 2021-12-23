using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class game_trainee_guessthesounds_MenuManager : MonoBehaviour
{
    public static bool isTutorial = false;
    public static bool canContinue = false;
    public static string key = "FIRSTTIMEOPENING";

    public Game_Over_Manager gameOverMan;

    void Start()
    {
        Start_Tutorial();
    }

    void Start_Tutorial()
    {
        if (PlayerPrefs.GetInt(gameOverMan.levelMapGameName, 1) == 1)
        {
            PlayerPrefs.SetInt(gameOverMan.levelMapGameName, 0);
            Load_Tutorial_Scene();
            canContinue = true;
        }
    }
    public void Load_Tutorial_Scene()
    {
        if (!isTutorial)
        {
            isTutorial = true;
            Game_Over_Manager.isLevel = true;
            gameOverMan.Start_Game();
        }
    }
}
