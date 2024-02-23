using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class NoteManager : MonoBehaviour
{
    private static NoteManager _instance;

    public static NoteManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<NoteManager>();
            return _instance;
        }
    }

    public List<Note> pooledObjects;
    public List<Note> _notePrefab;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        pooledObjects = new List<Note>();
        Note tmp;
        for (int i = 0; i < 40; i++)
        {
            tmp = Instantiate(GetNotePrefab(0));
            tmp.gameObject.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }
    
    public Note GetNotePrefab(int index)
    {
        if (index >= 0 && index < _notePrefab.Count)
        {
            return _notePrefab[index];
        }
        return null;
    }

    public void Reset()
    {
        for (int i = 0; i < 40; i++)
        {
            pooledObjects[i].gameObject.SetActive(false);
        }
    }
    public Note GetPooledObject()
    {
        for (int i = 0; i < 40; i++)
        {
            if (!pooledObjects[i].gameObject.activeSelf)
            {
                pooledObjects[i].noteRank = GameManager.Rank.NONE;
                pooledObjects[i].noteTapRank = GameManager.Rank.NONE;
                pooledObjects[i].gameObject.SetActive(true);
                return pooledObjects[i];
            }
        }
        return null;
    }

}
