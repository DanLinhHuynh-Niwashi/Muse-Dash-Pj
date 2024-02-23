using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        
        if (_instance == null)
        {
            _instance = this;
            
        }
        else
            Destroy(this.gameObject);
        
    }
    public enum Rank { PERFECT, GOOD, BAD, MISS, NONE };

    public Player playerPrefab;
    public static Song songInfo;
    public static AudioClip songclip;
    private static float songLength = -1;
    private static string songName = "";

    public Animator liveClearAni;
    [SerializeField]
    private Animator topAni;
    [SerializeField]
    private Animator botAni;

    

    public static bool paused = true;

    public bool done = false;

    [Header("Spawn Points")]
    public GameObject FinishLineX;
    public GameObject RemoveLineX;
    public GameObject StartLineX;
    public GameObject spawnPosY1;
    public GameObject spawnPosY2;

    public float[] spawnPosY;

    public float startLineX;
    public float finishLineX;

    public float removeLineX;

    public float badOffsetX;
    public float goodOffsetX;
    public float perfectOffsetX;
    private int topIndex = 0;
    private int botIndex = 0;

    [Header("NoteCheck")]
    public Queue<Note> topNotes;
    public Queue<Note> botNotes;
    public Note topNIndex;
    public Note botNIndex;

    [Header("Song Pos")]
    public float songPos;
    public float songPosBeat;
    public static float secPerBeat;

    public static float BeatsShownOnScreen = 4f;
    

    

    public static Song.NoteInfo topNoteBeats;
    public static Song.NoteInfo botNoteBeats;

    
    private Player player;

    private float countDown = 3;
    [SerializeField]
    private TextMeshProUGUI countDownText;
    [SerializeField]
    Camera cam;

    void Start()
    {
        load();
    }

    public void load()
    {
        topNotes = new Queue<Note>();
        botNotes = new Queue<Note>();
        
        
        removeLineX = cam.ScreenToWorldPoint(RemoveLineX.transform.position).x;
        startLineX = cam.ScreenToWorldPoint(StartLineX.transform.position).x;
        finishLineX = cam.ScreenToWorldPoint(FinishLineX.transform.position).x;
        spawnPosY = new float[3];
        spawnPosY[0] = cam.ScreenToWorldPoint(spawnPosY1.transform.position).y;
        spawnPosY[2] = cam.ScreenToWorldPoint(spawnPosY2.transform.position).y;
        spawnPosY[1] = (spawnPosY[0] + spawnPosY[1]) / 2;


        player = Instantiate(playerPrefab);
        player.Initialize(spawnPosY, removeLineX, topAni, botAni);
    }    

    public static void SongLoad(Song songInfomation)
    {
        songInfo = songInfomation;
        songLength = songInfo.length;
        secPerBeat = 60f / songInfo.BPM;
        songclip = songInfo.songSound;
        topNoteBeats = songInfo.topNoteBeats;
        botNoteBeats = songInfo.botNoteBeats;
        songName = songInfo.songID;

    }

    [SerializeField]
    private GameObject panelEndScreen;
    void Update()
    {
        if (songLength < 0) return;
        if (AudioManager.Instance.GetMusicTime() >= AudioManager.Instance.GetMusicLength() && done == false)
        {
            liveClearAni.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx("LiveClear");
            liveClearAni.SetTrigger("Start");
            
            done = true;
            liveClearAni.SetTrigger("End");

        }

        if (done == true)
        {
            if (liveClearAni.GetCurrentAnimatorClipInfo(0)[0].clip.name != "LiveClearEnd") return;
            else if (panelEndScreen.activeSelf == false)
            {
                panelEndScreen.SetActive(true);
                AudioManager.Instance.PlaySfx("Switch");
            }
            return;
        }    
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
            countDownText.text = Mathf.CeilToInt(countDown).ToString();
            if (countDown == 1 || countDown == 2 || countDown == 3)
            {
                AudioManager.Instance.PlaySfx("LiveClear");
            }
        }    
        if (countDown <= 0 && paused == true)
        {
            countDownText.text = "";
            paused = false;

            AudioManager.Instance.PlayFromPos(songName, songPos);
            
        }
        if (paused)
        {
            return;
        }
        songPos = AudioManager.Instance.GetMusicTime();
        songPosBeat = songPos / secPerBeat;

        Spawn();
        DestroyPassed();
    }

    public void PauseGame()
    {
        countDown = 3;
        AudioManager.Instance.PauseMusic();
        paused = true;
        this.gameObject.SetActive(false);
    }

    public void ReplayGame()
    {
        PointManager.Instance.Reset();
        NoteManager.Instance.Reset();
        Reset();
    }

    public void Continue()
    {
        this.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        songLength = -1;
    }
    public void Reset()
    {
        this.gameObject.SetActive(true);
        topIndex = 0;
        botIndex = 0;
        songPos = 0;
        done = false;
        paused = true; 

        topNotes.Clear();
        botNotes.Clear();
        AudioManager.Instance.StopMusic();
    }
    public void checkNoteHit(float hitTime, bool isTopTrack)
    {
        if (isTopTrack && topNotes.Count > 0)
        {
            Note i = topNotes.Peek();

            if (i.notePosInBeats * secPerBeat - perfectOffsetX < hitTime && i.notePosInBeats * secPerBeat + perfectOffsetX > hitTime)
            {
                i.noteTapRank = Rank.PERFECT;
            }    
                
            else if (i.notePosInBeats * secPerBeat - goodOffsetX < hitTime && i.notePosInBeats * secPerBeat + goodOffsetX > hitTime)
            {
                i.noteTapRank = Rank.GOOD;
            }    
                
            else if (i.notePosInBeats * secPerBeat - badOffsetX < hitTime && i.notePosInBeats * secPerBeat + badOffsetX > hitTime)
            {
                i.noteTapRank = Rank.BAD;
            }
            PointManager.Instance.UpdatePoint(i.noteTapRank, true);
            if (i.type == "SHORT" && i.noteTapRank != Rank.NONE)
            {
                i.gameObject.SetActive(false);
                topNotes.Dequeue();
            }    
        }    
        else if (isTopTrack == false && botNotes.Count > 0)
        {
            Note i = botNotes.Peek();
            if (i.notePosInBeats * secPerBeat - perfectOffsetX < hitTime && i.notePosInBeats * secPerBeat + perfectOffsetX > hitTime)
            {
                i.noteTapRank = Rank.PERFECT;
  
            }

            else if (i.notePosInBeats * secPerBeat - goodOffsetX < hitTime && i.notePosInBeats * secPerBeat + goodOffsetX > hitTime)
            {
                i.noteTapRank = Rank.GOOD;
            }

            else if (i.notePosInBeats * secPerBeat - badOffsetX < hitTime && i.notePosInBeats * secPerBeat + badOffsetX > hitTime)
            {
                i.noteTapRank = Rank.BAD;
            }

            PointManager.Instance.UpdatePoint(i.noteTapRank, false);
            if (i.type == "SHORT" && i.noteTapRank != Rank.NONE)
            {
                i.noteRank = i.noteTapRank;
                i.gameObject.SetActive(false);
                botNotes.Dequeue();
            }
        }
    }

    public void checkNoteRelease(float releaseTime, bool isTopTrack)
    {
        if (isTopTrack && topNotes.Count > 0)
        {
            Note i = topNotes.Peek();
            if (i.noteTapRank == Rank.NONE || i.noteRank == Rank.MISS) return;
            if (i.type == "SHORT") return;
            if (i.noteEndPosInBeats * secPerBeat - perfectOffsetX < releaseTime && i.noteEndPosInBeats * secPerBeat + perfectOffsetX > releaseTime)
            {
                i.noteRank = Rank.PERFECT;
            }

            else if (i.noteEndPosInBeats * secPerBeat - goodOffsetX < releaseTime && i.noteEndPosInBeats * secPerBeat + goodOffsetX > releaseTime)
            {
                i.noteRank = Rank.GOOD;
            }

            else if (i.noteEndPosInBeats * secPerBeat - badOffsetX < releaseTime && i.noteEndPosInBeats * secPerBeat + badOffsetX > releaseTime)
            {
                i.noteRank = Rank.BAD;
            }

            else if (i.noteEndPosInBeats * secPerBeat - badOffsetX > releaseTime)
            {
                i.noteRank = Rank.MISS;
            }

            PointManager.Instance.UpdatePoint(i.noteRank, true);
            if (i.noteRank != Rank.NONE)
            {
                i.gameObject.SetActive(false);
                topNotes.Dequeue();
            }
        }
        else if (isTopTrack == false && botNotes.Count > 0)
        {
            Note i = botNotes.Peek();
            if (i.noteTapRank == Rank.NONE || i.noteRank == Rank.MISS) return;
            if (i.type == "SHORT") return;
            if (i.noteEndPosInBeats * secPerBeat - perfectOffsetX < releaseTime && i.noteEndPosInBeats * secPerBeat + perfectOffsetX > releaseTime)
            {
                i.noteRank = Rank.PERFECT;
            }

            else if (i.noteEndPosInBeats * secPerBeat - goodOffsetX < releaseTime && i.noteEndPosInBeats * secPerBeat + goodOffsetX > releaseTime)
            {
                i.noteRank = Rank.GOOD;
            }

            else if (i.noteEndPosInBeats * secPerBeat - badOffsetX < releaseTime && i.noteEndPosInBeats * secPerBeat + badOffsetX > releaseTime)
            {
                i.noteRank = Rank.BAD;
            }
            else if (i.noteEndPosInBeats * secPerBeat - badOffsetX > releaseTime)
            {
                i.noteRank = Rank.MISS;
            }

            PointManager.Instance.UpdatePoint(i.noteRank, false);
            if (i.noteRank != Rank.NONE)
            {
                i.gameObject.SetActive(false);
                botNotes.Dequeue();
            }
        }
    }
    void DestroyPassed()
    {
        if (topNotes.Count > 0)
        {
            if (topNotes.Peek().notePosInBeats * secPerBeat + badOffsetX < songPos)
            {
                Note i = topNotes.Peek();
                if (i.noteTapRank == Rank.NONE)
                {
                    i.noteRank = Rank.MISS;
                    i.noteTapRank = Rank.MISS;
                    i.failFade();

                    if (i.type == "LONG")
                        PointManager.Instance.UpdatePoint(i.noteRank, true);
                }    
            }
            if (topNotes.Peek().noteEndPosInBeats * secPerBeat + badOffsetX < songPos)
            {
                Note i = topNotes.Peek();
                if (i.noteRank == Rank.NONE)
                {
                    i.noteRank = Rank.MISS;
                }
                i.gameObject.SetActive(false);
                topNotes.Dequeue();
                PointManager.Instance.UpdatePoint(i.noteRank, true);
            }
            
        }

        if (botNotes.Count > 0)
        {
            if (botNotes.Peek().notePosInBeats * secPerBeat + badOffsetX < songPos)
            {
                Note i = botNotes.Peek();
                if (i.noteTapRank == Rank.NONE)
                {
                    i.noteRank = Rank.MISS;
                    i.noteTapRank = Rank.MISS;
                    i.failFade();

                    if (i.type == "LONG")
                        PointManager.Instance.UpdatePoint(i.noteRank, false);
                }
            }
            if (botNotes.Peek().noteEndPosInBeats * secPerBeat + badOffsetX < songPos)
            {
                Note i = botNotes.Peek();
                if (i.noteRank == Rank.NONE)
                {
                    i.noteRank = Rank.MISS;
                }
                i.gameObject.SetActive(false);
                botNotes.Dequeue();
                PointManager.Instance.UpdatePoint(i.noteRank, false);
            }

        }

    }
    void Spawn()
    {
        if (topIndex < topNoteBeats.Count && topNoteBeats.s[topIndex] < songPosBeat + BeatsShownOnScreen)
        {
            Note temp = NoteManager.Instance.GetPooledObject();

            temp.Initialize(startLineX, finishLineX, removeLineX, spawnPosY[0], topNoteBeats.s[topIndex], topNoteBeats.e[topIndex], Color.magenta);

            
            topNotes.Enqueue(temp);
            topIndex++;
        }

        if (botIndex < botNoteBeats.Count && botNoteBeats.s[botIndex] < songPosBeat + BeatsShownOnScreen)
        {
            Note temp = NoteManager.Instance.GetPooledObject();
            temp.Initialize(startLineX, finishLineX, removeLineX, spawnPosY[2], botNoteBeats.s[botIndex], botNoteBeats.e[botIndex], Color.cyan);

            botNotes.Enqueue(temp);
            botIndex++;
        }
    }
}
