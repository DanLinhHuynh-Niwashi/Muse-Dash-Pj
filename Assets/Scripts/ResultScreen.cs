using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Animator resultPopAni;

    int PerfectCount = 0;
    int GoodCount = 0;
    int BadCount = 0;
    int MissCount = 0;

    public long totalPoint;
    public int longestCombo = 0;

    string rank;

    [SerializeField]
    private TextMeshProUGUI ScoreText;
    [SerializeField]
    private TextMeshProUGUI ComboText;
    [SerializeField]
    private GameObject NoteText;
    [SerializeField]
    private GameObject ResultText;
    [SerializeField]
    private TextMeshProUGUI pText;
    [SerializeField]
    private TextMeshProUGUI gText;
    [SerializeField]
    private TextMeshProUGUI bText;
    [SerializeField]
    private TextMeshProUGUI mText;
    [SerializeField]
    private Image rankImage;
    [SerializeField]
    private Sprite[] rankSprites;


    void Start()
    {
        
    }

    private void OnEnable()
    {
        PerfectCount = PointManager.Instance.PerfectCount;
        GoodCount = PointManager.Instance.GoodCount;
        BadCount = PointManager.Instance.BadCount;
        MissCount = PointManager.Instance.MissCount;
        totalPoint = PointManager.Instance.totalPoint;
        longestCombo = PointManager.Instance.longestCombo;

        gText.text = GoodCount.ToString();
        bText.text = BadCount.ToString();
        mText.text = MissCount.ToString();
        pText.text = PerfectCount.ToString();
        ComboText.text = longestCombo.ToString();
        rank = PointManager.Instance.Ranking();
    }
    // Update is called once per frame
    void Update()
    {
        if (resultPopAni.GetCurrentAnimatorClipInfo(0)[0].clip.name != "ScorePanelAniEnd") return;
        if (ResultText.gameObject.activeSelf == false)
        {
            ResultText.gameObject.SetActive(true);
            AudioManager.Instance.PlaySfx("Count");
        }
        StartCoroutine(scoreCount(totalPoint));

        if (doneCounting)
        {
            
            if (rankImage.gameObject.activeSelf == false)
            {
                SetRankImage();
                rankImage.gameObject.SetActive(true);
            }
            if (NoteText.gameObject.activeSelf == false)
            {
                NoteText.gameObject.SetActive(true);
            }
        }
        
    }

    private void SetRankImage()
    {
        switch (rank[0])
        {
            case 'S':
                rankImage.sprite = rankSprites[0];
                break;
            case 'A':
                rankImage.sprite = rankSprites[1];
                break;
            case 'B':
                rankImage.sprite = rankSprites[2];
                break;
            case 'C':
                rankImage.sprite = rankSprites[3];
                break;
            case 'D':
                rankImage.sprite = rankSprites[4];
                break;
            default:
                rankImage.sprite = rankSprites[4];
                break;
        }   
    }
    float countDur = 2;
    public long crnPoint = 0;
    public long crnCombo = 0;
    bool doneCounting = false;
    IEnumerator scoreCount(long Target)
    {
        var rate = Mathf.Abs(Target - crnPoint) / countDur;
        
        while (crnPoint != Target)
        {
            crnPoint = (long)Mathf.MoveTowards(crnPoint, Target, rate * Time.deltaTime);
            ScoreText.text = crnPoint.ToString().PadLeft(7, '0');
            yield return null;
        }    
        if (crnPoint == Target && doneCounting == false) {
            AudioManager.Instance.PlaySfx("CountDone");
            doneCounting = true;
        }
    }


}
