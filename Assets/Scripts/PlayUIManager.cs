using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject panelPauseScreen;
    [SerializeField]
    private GameObject panelStartScreen;
    [SerializeField]
    private GameObject panelChooseScreen;

    [SerializeField]
    private Animator transiScreen;
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Menu_Click()
    {
        AudioManager.Instance.PlaySfx("ButtonTap");
        AudioManager.Instance.PlayScreenSfx("Switch");

        GameManager.Instance.Reset();
        GameManager.Instance.EndGame();
        StartCoroutine(loadScreen("ChoosingScene"));
    }

    public Song songInfo;

    public void Choosing()
    {
        AudioManager.Instance.PlaySfx("ButtonTap");
        AudioManager.Instance.PlayScreenSfx("Switch");

        StartCoroutine(loadScreen("ChoosingScene"));
    }

    public void StartGame_Click()
    {
        songInfo = DynamicScrollView.Instance.crnSong;

        AudioManager.Instance.PlaySfx("ButtonTap");
        AudioManager.Instance.PlayScreenSfx("Switch");

        StartCoroutine(loadScreen("PlayingScene"));
        GameManager.SongLoad(songInfo);
    }

    IEnumerator loadScreen(string screenName)
    {
        AudioManager.Instance.StopMusic();
        transiScreen.SetTrigger("Load");   
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(screenName, LoadSceneMode.Single);
    }
    public void Pause_Click()
    {
        AudioManager.Instance.PlaySfx("ButtonTap");
        AudioManager.Instance.PlayScreenSfx("MessageBox");

        panelPauseScreen.SetActive(true);
        GameManager.Instance.PauseGame();
    }    

    public void Replay_Click()
    {
        AudioManager.Instance.PlaySfx("ButtonTap");

        panelPauseScreen.SetActive(false);
        GameManager.Instance.ReplayGame();
    }    

    public void Continue_Click()
    {
        AudioManager.Instance.PlaySfx("ButtonTap");

        panelPauseScreen.SetActive(false);
        GameManager.Instance.Continue();
    }    

}
