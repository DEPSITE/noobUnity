using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SongSelectManager : MonoBehaviour
{
    public Image musicImageUI;
    public Text musicTitleUI;
    public Text bpmUI;
    private int musicIndex;
    private int musicCount = 3;
    private void UpdateSong(int musicIndex)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + musicIndex.ToString());
        StringReader stringReader = new StringReader(textAsset.text);
        musicTitleUI.text = stringReader.ReadLine(); // 첫번째줄 읽기
        stringReader.ReadLine(); // 두번째줄 읽으나 작업은하지않음
        bpmUI.text = "BPM: " + stringReader.ReadLine().Split(' ')[0];  //세번째줄의 첫번째 
        AudioClip audioClip = Resources.Load<AudioClip>("Beats/ " + musicIndex.ToString());
        audioSource.clip = audioClip;
        audioSource.Play();
        musicImageUI.sprite = Resources.Load<Sprite>("Beats/" + musicIndex.ToString());
    }
    public void Right()
    {
        musicIndex += 1;
        if (musicIndex > musicCount) musicIndex = 1;
        UpdateSong(musicIndex);
    }
    public void Left()
    {
        musicIndex -= 1;
        if (musicIndex < 1) musicIndex = musicCount;
        UpdateSong(musicIndex);
    }

    // Start iscalled before the first frame update
    void Start()
    {
        musicIndex = 1;
        UpdateSong(musicIndex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStart()
    {
        PlayerInformation.selectedMusic = musicIndex.ToString();
        SceneManager.LoadScene("GameScene");
    }
}
