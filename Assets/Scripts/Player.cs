using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float[] spawnPosY;
    public float posX;

    public float hitTimeTop;
    public float releaseTimeTop;
    public float hitTimeBot;
    public float releaseTimeBot;

    [SerializeField]
    private Animator topAni;
    [SerializeField]
    private Animator botAni;

    public void Initialize(float[] spawnPosY, float posX, Animator topAni, Animator botAni)
    {
        this.spawnPosY = spawnPosY;
        this.posX = posX;
        this.topAni = topAni;
        this.botAni = botAni; 
        this.transform.position = new Vector3(posX, spawnPosY[2], 0);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (GameManager.paused) { return; }
        inputHandle();
    }

    public bool top = false;
    public bool bot = false;
    void inputHandle()
    {
        if (Input.touchCount > 0)
        {
            
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.position.y > Screen.height / 4 * 3)
                {
                    continue;
                }
                if (touch.phase == TouchPhase.Began)
                {
                    
                    if (touch.position.x < Screen.width / 2)
                    {
                        hitTop();
                        top = true;
                    }
                    if (touch.position.x > Screen.width / 2)
                    {
                        hitBot();
                        bot = true;
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (touch.position.x < Screen.width / 2)
                    {
                        releaseTop();
                        top = false;
                    }
                    if (touch.position.x > Screen.width / 2)
                    {
                        releaseBot();
                        bot = false;
                    }
                }
            }
            if (top && bot)
            {
                goMid();
            }
            else if (top)
            {
                goTop();
            }
            else if (bot)
            {
                goBot();
            }    
            else goBot();
        }

        else
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                hitTop();
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                hitBot();
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
            {
                goMid();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                goTop();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                goBot();
            }
            else
                goBot();

            if (Input.GetKeyUp(KeyCode.A))
            {
                releaseBot();
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                releaseTop();
            }
        }
           
    }    
    void hitTop()
    {
        AudioManager.Instance.PlaySfx("Tap");

        hitTimeTop = GameManager.Instance.songPos;
        GameManager.Instance.checkNoteHit(hitTimeTop, true);
        topAni.Play("FlashLight1");
    }

    void hitBot()
    {
        AudioManager.Instance.PlaySfx("Tap");
        hitTimeBot = GameManager.Instance.songPos;
        GameManager.Instance.checkNoteHit(hitTimeBot, false);
        botAni.Play("FlashLight2");
    }
    void releaseTop()
    {
        AudioManager.Instance.PlaySfx("Release");

        releaseTimeTop = GameManager.Instance.songPos;
        GameManager.Instance.checkNoteRelease(releaseTimeTop, true);
        topAni.SetTrigger("Norm");
    }

    void releaseBot()
    {
        AudioManager.Instance.PlaySfx("Release");
        releaseTimeBot = GameManager.Instance.songPos;
        GameManager.Instance.checkNoteRelease(releaseTimeBot, false);
        botAni.SetTrigger("Norm");
    }

    void goBot()
    {
        this.transform.DOMove(new Vector3(this.transform.position.x, spawnPosY[2], this.transform.position.z), 0.03f);
    }
    void goTop()
    {
        this.transform.DOMove(new Vector3(this.transform.position.x, spawnPosY[0], this.transform.position.z), 0.03f);
    }
    void goMid()
    {
        this.transform.DOMove (new Vector3(this.transform.position.x, spawnPosY[1], this.transform.position.z),0.03f);
    }
}
