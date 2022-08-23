using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Over_2_OptionPanel : MonoBehaviour
{
    private Game_Over_2_AudioManager _audioManager;

    [SerializeField] private Toggles _sfxTaggles;
    [SerializeField] private Toggles _musicTaggle;
    public static bool sfxMuted = false, musicMuted = false;

    [SerializeField] private GameObject _SettingsPanel;

    [SerializeField] private Animator _anim;

    private void Start()
    {
        _audioManager = Game_Over_2_AudioManager.audioManInstance;
        Switchs_State();
    }

    private void Switchs_State()
    {
        _sfxTaggles.onSwitch.SetActive(!sfxMuted);
        _sfxTaggles.offSwitch.SetActive(sfxMuted);
        _musicTaggle.onSwitch.SetActive(!musicMuted);
        _musicTaggle.offSwitch.SetActive(musicMuted);
    }

    public void Open_Close_Settings_Panel(bool isOpened)
    {
        _audioManager.Play_Sfx(Game_Over_2_Constants.CLICK_SFX);

        if (isOpened)
        {
            _anim.Play(Game_Over_2_Constants.OPEN_SETTINGS_PANEL_ANIM);
        }
        else
        {
            _anim.Play(Game_Over_2_Constants.CLOSE_SETTINGS_PANEl_ANIM);
        }
    }

    public void Mute_Sfx()
    {
        if (sfxMuted)
        {
            sfxMuted = false;
        }
        else
        {
            sfxMuted = true;
        }
        _sfxTaggles.onSwitch.SetActive(!sfxMuted);
        _sfxTaggles.offSwitch.SetActive(sfxMuted);
        _audioManager.Mute_Sfx(sfxMuted);
    }

    public void Mute_Music()
    {
        if (musicMuted)
        {
            musicMuted = false;
        }
        else
        {
            musicMuted = true;
        }
        _musicTaggle.onSwitch.SetActive(!musicMuted);
        _musicTaggle.offSwitch.SetActive(musicMuted);
        _audioManager.Mute_Music(musicMuted);
    }
}