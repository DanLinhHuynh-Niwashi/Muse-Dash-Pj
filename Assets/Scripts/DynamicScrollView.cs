using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DynamicScrollView : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform scrollViewContent;
    public GameObject prefab;
    public List<Song>  songs;

    public Image cover;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Info;
    public Button button;

    public Song crnSong;

    private static DynamicScrollView _instance;

    public static DynamicScrollView Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<DynamicScrollView>();
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

    private void Start()
    {
        foreach (Song song in songs)
        {
            GameObject newSong = Instantiate(prefab, scrollViewContent);
            if (newSong.TryGetComponent(out ScrollViewItem item))
            {
                item.ChangeInfo(song);
            }    

        }
        if (songs.Count > 0)
        {
            setInfo(songs[0]);
        }
        
    }

    public void setInfo(Song song)
    {
        cover.sprite = song.coverImg;
        Title.text = song.songName;
        Info.text = song.description;
        crnSong = song;

        cover.gameObject.SetActive(true);
        Title.gameObject.SetActive(true);
        Info.gameObject.SetActive(true);
        button.gameObject.SetActive(true);

    }
}
