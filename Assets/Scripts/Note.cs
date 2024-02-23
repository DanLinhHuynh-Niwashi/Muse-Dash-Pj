<<<<<<< HEAD
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> c8a00a4dfed3d74998fd476d3a1105f463f5d5c1
using UnityEngine;

public class Note : MonoBehaviour
{
    // Start is called before the first frame update

<<<<<<< HEAD
    protected int _positionY;

    public Vector3 SpawnPos;
    public Vector3 RemovePos;
    public float endX;
    public float noteY;

    public float notePosInBeats;
    public float noteEndPosInBeats;
    public float songPosInBeats;


    [SerializeField] private GameObject noteEnd;
    [SerializeField] private GameObject noteConnector;

    public GameManager.Rank noteRank = GameManager.Rank.NONE;
    public GameManager.Rank noteTapRank = GameManager.Rank.NONE;
    public string type;

    
    public void Initialize(float startX, float endX, float removeX, float startY, float notePosInBeats, float noteEndPos, Color color)
    {
        this.endX = endX;
        this.notePosInBeats = notePosInBeats;

        this.noteEndPosInBeats = noteEndPos;

        SpawnPos = new Vector3(startX, startY, 1);
        RemovePos = new Vector3(endX, startY, 1);

        noteY = startY;
        transform.position = SpawnPos;
        if (noteEndPos > notePosInBeats) type = "LONG";
        else type = "SHORT";

        this.GetComponent<Renderer>().material.color = color;
        noteEnd.gameObject.GetComponent<Renderer>().material.color = color;
        noteConnector.gameObject.GetComponent<Renderer>().material.color = color;

    }
    void Update()
    {
        if (type == "LONG" && noteTapRank != GameManager.Rank.NONE && noteTapRank != GameManager.Rank.MISS && noteRank != GameManager.Rank.MISS)
        {
            transform.position = new Vector3(endX, transform.position.y, transform.position.z);
        }
        else
            transform.position = new Vector3(SpawnPos.x + (endX - SpawnPos.x) * (1f - (notePosInBeats - GameManager.Instance.songPosBeat) / GameManager.BeatsShownOnScreen), transform.position.y, transform.position.z);
        
        if (noteEnd.transform.position.x <= transform.position.x && noteTapRank != GameManager.Rank.NONE && noteTapRank != GameManager.Rank.MISS)
        {
            noteEnd.transform.position = transform.position;
        }    
        else
            noteEnd.transform.position = new Vector3(SpawnPos.x + (endX - SpawnPos.x) * (1f - (noteEndPosInBeats - GameManager.Instance.songPosBeat) / GameManager.BeatsShownOnScreen), transform.position.y, transform.position.z);
        
        noteConnector.transform.localScale = new Vector3(noteEnd.transform.position.x - this.transform.position.x, noteConnector.transform.localScale.y, noteConnector.transform.localScale.z);
        noteConnector.transform.position = new Vector3((noteEnd.transform.position.x + this.transform.position.x)/2, this.transform.position.y, this.transform.position.z);
    }
    
    public void failFade()
    {
        Color tempC = this.gameObject.GetComponent<Renderer>().material.color;
        tempC.a = 0.5f;
        this.gameObject.GetComponent<Renderer>().material.color = tempC;
        noteEnd.gameObject.GetComponent<Renderer>().material.color = tempC;
        noteConnector.gameObject.GetComponent<Renderer>().material.color = tempC;
    }    
    
=======
    int position;
    float speed;
    float speed
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
>>>>>>> c8a00a4dfed3d74998fd476d3a1105f463f5d5c1
}
