﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;

public class GameResultManager : MonoBehaviour
{
    public Text musicTitleUI;
    public Text scoreUI;
    public Text maxComboUI;
    public Image RankUI;

    // Start is called before the first frame update
    void Start()
    {
        musicTitleUI.text = PlayerInformation.musicTitle;
        scoreUI.text = "점수 : " + (int)PlayerInformation.score;
        maxComboUI.text = "최대 콤보 : " + PlayerInformation.maxCombo;
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + PlayerInformation.selectedMusic);
        StringReader reader = new StringReader(textAsset.text);
        reader.ReadLine();
        reader.ReadLine(); // 2번째줄까지 무시
        string beatInformation = reader.ReadLine();
        int scoreS = Convert.ToInt32(beatInformation.Split(' ')[3]);
        int scoreA = Convert.ToInt32(beatInformation.Split(' ')[4]);
        int scoreB = Convert.ToInt32(beatInformation.Split(' ')[5]);
        if(PlayerInformation.score >= scoreS)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank S");
        }
        else if (PlayerInformation.score >= scoreA)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank A");
        }
        else if (PlayerInformation.score >= scoreB)
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank B");
        }
        else
        {
            RankUI.sprite = Resources.Load<Sprite>("Sprites/Rank C");
        }
    }
    public void Replay()
    {
        SceneManager.LoadScene("SongSelect");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
