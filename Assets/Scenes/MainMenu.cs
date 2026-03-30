using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject TitlePane;
    [SerializeField] private GameObject CreditsPane;
    [SerializeField] private GameObject PlayPropertiesPane;
    [SerializeField] private GameObject LeaderBoardsPane;
    public AudioManager aud;

    [Header("Difficulty Text")]
    [SerializeField] private TextMeshProUGUI EasyTxt;
    [SerializeField] private TextMeshProUGUI NormalTxt;
    [SerializeField] private TextMeshProUGUI HardTxt;

    [SerializeField] private ScoreSystem ScoreSystem;
    [SerializeField] private TextMeshProUGUI LeaderboardTxt;

    private void Start()
    {
        ScoreSystem.DisplayLeaderboard(LeaderboardTxt);
        aud.PlaySound(aud.Ambience, aud.a_Static);
        aud.PlaySound(aud.SubAmbience, aud.a_VinylCrack);

    }

    //MAIN MENU
    public void PlayButton()
    {
        aud.PlaySound(aud.SoundFX, aud.s_MenuAppear);
        TitlePane.SetActive(false);
        PlayPropertiesPane.SetActive(true);
        PlayerPrefs.GetInt("LVL", 2);
    }

    public void Credits()
    {

        aud.PlaySound(aud.SoundFX, aud.s_MenuAppear);
        TitlePane.SetActive(false);
        CreditsPane.SetActive(true);
        LeaderBoardsPane.SetActive(false);
    }

    public void QuitButton()
    {
        aud.PlaySound(aud.SoundFX, aud.s_MenuAppear);
        Application.Quit();
    }



    //PLAY PROPPERTIES
    public void StartGame()
    {
        aud.PlaySound(aud.SoundFX, aud.s_MenuAppear);
        SceneManager.LoadScene("Cutscene");
    }

    public void BackToMenu()
    {
        aud.PlaySound(aud.SoundFX, aud.s_MenuAppear);
        TitlePane.SetActive(true);
        CreditsPane.SetActive(false);
        PlayPropertiesPane.SetActive(false);
        LeaderBoardsPane.SetActive(true);
    }

    public void SetDifficulty (int lvl)
    {

        aud.PlaySound(aud.SoundFX, aud.s_MenuSelect);
        switch (lvl)
        {
            case 1:
                {
                    EasyTxt.fontStyle = FontStyles.Bold;
                    NormalTxt.fontStyle = FontStyles.Normal;
                    HardTxt.fontStyle = FontStyles.Normal;
                    PlayerPrefs.SetInt("LVL", lvl);
                    break;
                }
            case 2:
                {
                    EasyTxt.fontStyle = FontStyles.Normal;
                    NormalTxt.fontStyle = FontStyles.Bold;
                    HardTxt.fontStyle = FontStyles.Normal;
                    PlayerPrefs.SetInt("LVL", lvl);
                    break;
                }
            case 3:
                {
                    EasyTxt.fontStyle = FontStyles.Normal;
                    NormalTxt.fontStyle = FontStyles.Normal;
                    HardTxt.fontStyle = FontStyles.Bold;
                    PlayerPrefs.SetInt("LVL", lvl);
                    break;
                }
        }
    }
}
