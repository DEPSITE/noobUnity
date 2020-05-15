using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NoteController : MonoBehaviour
{
    // Start is called before the first frame update
    class Note
    {
        public int noteType {get; set;}
        public int order { get; set; }
        public Note(int noteType , int order)
        {
            this.noteType = noteType;
            this.order = order;
        }

    }
    public GameObject[] Notes;
    private List<Note> notes = new List<Note>();
    private float beatInterval;
    private ObjectPooler noteObjectPooler;
    private float x, z, startY = 8.0f;
    void MakeNote(Note note)
    {
        GameObject obj = noteObjectPooler.getObject(note.noteType);
        x = obj.transform.position.x;
        z = obj.transform.position.z;
        obj.transform.position = new Vector3(x, startY, z);
        obj.GetComponent<NoteBehavior>().Initialize();
        obj.SetActive(true);
    }

    private string musicTitle;
    private string musicArtist;
    private int bpm;
    private int divider;
    private float startingPoint;
    private float beatCount;
    IEnumerator AwaitMakeNote(Note note)
    {
        int noteType = note.noteType;
        int order = note.order;
        yield return new WaitForSeconds(startingPoint + order * beatInterval);
        MakeNote(note);
    }
    void Start()
    {
        noteObjectPooler = gameObject.GetComponent<ObjectPooler>();
        //리소스에서 beat 텍스트 파일 불러오기
        TextAsset textAsset = Resources.Load<TextAsset>("Beats/" + PlayerInformation.selectedMusic);
        StringReader reader = new StringReader(textAsset.text);
        //첫번째줄은 곡 이름
        musicTitle = reader.ReadLine();
        //두번째줄은 작가 이름
        musicArtist = reader.ReadLine();
        //세번째줄은 비트정보(bpm,divider,시작시간)
        string beatInformation = reader.ReadLine();
        // beatinformation =" bpm , divider , starttime)
        bpm = Convert.ToInt32(beatInformation.Split(' ')[0]);
        divider = Convert.ToInt32(beatInformation.Split(' ')[1]);
        startingPoint = (float)Convert.ToDouble(beatInformation.Split(' ')[2]);
        beatCount = (float)bpm / divider;
        //beatcount는 비트가 떨어지는 간격 시간 조절
        beatInterval = 1 / beatCount;
        //각 beat들이 떨어지는 시간 및 위치 정보
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            Note note = new Note(
                   Convert.ToInt32(line.Split(' ')[0]) +1,
                   Convert.ToInt32(line.Split(' ')[1])
                );
            notes.Add(note);
        }
        for(int i =0; i <notes.Count;i++)
        {
            StartCoroutine(AwaitMakeNote(notes[i]));
        }
        //마지막 노트 기준 게임종료함수 호출
        StartCoroutine(AwaitGameResult(notes[notes.Count - 1].order));
    }
    IEnumerator AwaitGameResult(int order)
    {
        yield return new WaitForSeconds(startingPoint + order * beatInterval +8.0f);
        GameResult();
    }
    void GameResult()
    {
        PlayerInformation.maxCombo = GameManager.instance.maxCombo;
        PlayerInformation.score = GameManager.instance.score;
        PlayerInformation.musicTitle = musicTitle;
        PlayerInformation.musicArtist = musicArtist;
        SceneManager.LoadScene("GameResultScene");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
