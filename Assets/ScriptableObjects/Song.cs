using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Song")]
public class Song : ScriptableObject
{
    public AudioClip songSound;
    public float BPM;
    public float length;
    public string songName;
    public string songID;
    public Sprite coverImg;

    //-1 appeared between two beats as a signal for long notes
    [field: TextArea]
    public string description = "A";
    public NoteInfo topNoteBeats = new NoteInfo(new List<float> { 7, 8, 12, 14, 17, 18, 20, 21, 22, 25, 32, 35, 37, 38, 41, 46, 52, 53, 57, 58, 62, 63, 64, 71, 73, 75, 77, 82, 85, 95, 96, 101, 103, 106, 107, 108, 111, 112, 117, 122, 124, 126, 127, 132, 141, 142, 146, 149, 150, 151, 152, 154, 159, 164, 166, 171, 173, 177, 183, 184 }, new List<float> {7, 11,  12, 14, 17, 18, 20, 21, 22, 30, 32, 35, 37, 38, 41, 50, 52, 53, 57, 60, 62, 63, 64, 71, 73, 75, 77, 82, 90, 95, 96, 101, 103, 106, 107, 108, 111, 112, 117, 122, 124, 126, 127, 132, 141, 142, 146, 149, 150, 151, 152, 154, 159, 164, 166, 171, 173, 177, 183, 184
});
    public NoteInfo botNoteBeats = new NoteInfo(new List<float> { 6, 9, 10, 24, 26, 29, 31, 36, 40, 42, 43, 45, 48, 49, 51, 54, 55, 56, 61, 64, 72, 74, 76, 78, 80, 90, 95, 97, 98, 102, 109, 110, 114, 116, 118, 119, 123, 125, 129, 131, 134, 137, 143, 144, 148, 156, 157, 160, 161, 162, 163, 165, 169, 172, 174, 176, 178, 180, 181, 182, 187, 189, 191, 193 }, new List<float> { 6, 9, 16, 24, 26, 29, 31, 36, 40, 42, 43, 45, 48, 49, 51, 54, 55, 56, 61, 67, 72, 74, 76, 78, 80, 90, 95, 97, 98, 102, 109, 110, 114, 116, 118, 119, 123, 125, 129, 131, 134, 140, 143, 144, 148, 156, 157, 160, 161, 162, 163, 165, 169, 172, 174, 176, 178, 180, 181, 182, 187, 189, 191, 193
});

    [Serializable]
    public class NoteInfo
    {
        [SerializeField]
        public int Count;
        [SerializeField]
        public List<float> s;
        [SerializeField]
        public List<float> e;

        public NoteInfo(List<float> s, List<float> e)
        {
            this.s = s;
            this.e = e;
        }

    }
}
