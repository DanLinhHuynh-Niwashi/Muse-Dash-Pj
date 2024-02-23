using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollViewItem : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public Image cover;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Info;
    public Song song;

    public void ChangeInfo(Song song)
    {
        cover.sprite = song.coverImg;
        Title.text = song.songName;
        Info.text = song.description;
        this.song = song;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySfx("ButtonTap");
        DynamicScrollView.Instance.setInfo(song);
    }

}
