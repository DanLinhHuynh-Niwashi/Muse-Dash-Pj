using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    private static PointManager _instance;

    public int PerfectCount = 0;
    public int GoodCount = 0;
    public int BadCount = 0;
    public int MissCount = 0;

    private int []comboAdd = {0, 50, 100, 150 };
    private int comboAddI = 0;

    public int comboCount = 0;
    public int longestCombo = 0;
    public int PPoint = 300;
    public int GPoint = 200;
    public int BPoint = 100;

    public long totalPoint = 0;

    private float topStatusShowLeft = 0;
    private float botStatusShowLeft = 0;

    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI comboText;
    [SerializeField]
    private TextMeshProUGUI topStat;
    [SerializeField]
    private TextMeshProUGUI botStat;
    public static PointManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PointManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    public string Ranking()
    {
        long highest = PPoint * (PerfectCount + GoodCount + BadCount + MissCount);
        if (totalPoint / (highest * 1.0f) < 0.4f) return "D";
        if (totalPoint / (highest * 1.0f) < 0.6f) return "C";
        if (totalPoint / (highest * 1.0f) < 0.8f) return "B";
        if (totalPoint / (highest * 1.0f) < 1f) return "A";
        return "S";
    }    
    private void Update()
    {
        if (GameManager.paused) return;
        if (topStatusShowLeft > 0)
        {
            topStatusShowLeft-= Time.deltaTime;
        }
        if (botStatusShowLeft > 0)
        {
            botStatusShowLeft -= Time.deltaTime;
        }

        if (botStatusShowLeft <= 0)
            botStat.text = "";
        if (topStatusShowLeft <= 0)
            topStat.text = "";
    }

    public void Reset()
    {
        topStatusShowLeft = 0;
        botStatusShowLeft = 0;

        PerfectCount = 0;
        GoodCount = 0;
        BadCount = 0;
        MissCount = 0;

        totalPoint = 0;

        comboCount = 0;
        longestCombo = 0;
        comboAddI = 0;

        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = totalPoint.ToString();
        comboText.text = comboCount.ToString();
    }    
    public void UpdatePoint(GameManager.Rank rank, bool isTopPos)
    {
        if (rank == GameManager.Rank.NONE) return;
        if (rank == GameManager.Rank.PERFECT)
        {
            PerfectCount++;
            totalPoint += PPoint += comboAdd[comboAddI];
            comboCount++;
        }
        if (rank == GameManager.Rank.GOOD)
        {
            GoodCount++;
            totalPoint += GPoint += comboAdd[comboAddI];
            comboCount++;
        }
        if (rank == GameManager.Rank.BAD)
        {
            BadCount++;
            totalPoint += BPoint;
            comboCount = 0;
        }
        if (rank == GameManager.Rank.MISS)
        {
            MissCount++;
            comboCount = 0;
        }

        comboAddI = 0;
        if (comboCount > 10)
        {
            comboAddI = 1;
        }
        if (comboCount > 50)
        {
            comboAddI = 2;
        }    
        if (comboCount > 100)
        {
            comboAddI = 3;
        }    

        if (comboCount > longestCombo)
        {
            longestCombo = comboCount;
        }

        UpdateText();

        if (isTopPos)
        {
            topStat.text = rank.ToString();
            topStatusShowLeft = 0.5f;
        }
        else
        {
            botStat.text = rank.ToString();
            botStatusShowLeft = 0.5f;
        }    
    } 
}
