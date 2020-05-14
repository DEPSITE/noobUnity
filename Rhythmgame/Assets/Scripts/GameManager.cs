using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager instance { get; set; }
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
    }
    public float noteSpeed;
    public GameObject scoreUI;
    private float score;
    private Text scoreText;
    public GameObject comboUI;
    private int combo;
    private Text comboText;
    private Animator ComboAnimator;
    public GameObject judgeUI;
    private Sprite[] judgeSprites;
    private Image judgementSpriteRenderer;
    private Animator judgementSpriteAnimator;

    public enum judges {NONE=0,BAD,GOOD,PERFECT,MISS};
    public GameObject[] trails;
    private SpriteRenderer[] trailSpriteRenderers;
    void Start()
    {
        judgementSpriteRenderer = judgeUI.GetComponent<Image>();
        judgementSpriteAnimator = judgeUI.GetComponent<Animator>();
        scoreText = scoreUI.GetComponent<Text>();
        comboText = comboUI.GetComponent<Text>();
        ComboAnimator = comboUI.GetComponent<Animator>();
        judgeSprites = new Sprite[4];
        judgeSprites[0] = Resources.Load<Sprite>("Sprites/Bad");
        judgeSprites[1] = Resources.Load<Sprite>("Sprites/Good");
        judgeSprites[2] = Resources.Load<Sprite>("Sprites/Miss");
        judgeSprites[3] = Resources.Load<Sprite>("Sprites/Perfect");

        trailSpriteRenderers = new SpriteRenderer[trails.Length];
        for(int i = 0; i <trails.Length;i++)
        {
            trailSpriteRenderers[i] = trails[i].GetComponent<SpriteRenderer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D)) ShineTrail(0);
        if (Input.GetKey(KeyCode.F)) ShineTrail(1);
        if (Input.GetKey(KeyCode.J)) ShineTrail(2);
        if (Input.GetKey(KeyCode.K)) ShineTrail(3);
        for(int i=0; i< trailSpriteRenderers.Length;i++)
        {
            Color color = trailSpriteRenderers[i].color;
            color.a -= 0.01f;
            trailSpriteRenderers[i].color = color;
        }
    }
    public void ShineTrail(int index)
    {
        Color color = trailSpriteRenderers[index].color;
        color.a = 0.32f;
        trailSpriteRenderers[index].color = color;
    }
    //판정결과
    void showJudgement()
    {
        string scoreFormat = "000000";
        scoreText.text = score.ToString(scoreFormat);
        judgementSpriteAnimator.SetTrigger("Show");
        //콤보2이상일때부터 이미지 출력
        if(combo>=2)
        {
            comboText.text = "COMBO " + combo.ToString();
            ComboAnimator.SetTrigger("Show");
        }
    }
    //노트판정
    public void processJudge(judges judge, int noteType)
    {
        if (judge == judges.NONE) return;
        //미스인경우 콤보끊김
        if (judge == judges.MISS)
        {
            judgementSpriteRenderer.sprite = judgeSprites[2];
            combo = 0;
            if (score >= 15) score -= 15;
        }
        //배드도 콤보 끊기게
        else if (judge == judges.BAD)
        {
            judgementSpriteRenderer.sprite = judgeSprites[0];
            combo = 0;
            if (score >= 5) score -= 5;
        }
        else //퍼펙트와 굿
        {
            if(judge == judges.PERFECT)
            {
                judgementSpriteRenderer.sprite = judgeSprites[3];
                score += 25;
            }
            else if(judge == judges.GOOD)
            {
                judgementSpriteRenderer.sprite = judgeSprites[1];
                score += 15;
            }
            combo += 1;
            score += (float)combo * 0.1f;
        }
        showJudgement();
    }
}
