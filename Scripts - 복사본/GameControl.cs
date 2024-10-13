using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public int stageIndex = 4;
    public int playerLife;
    public PlayerMove player;
    public GameObject[] Stages;

    public Image[] UIlife;
    public Text UIstage;
    public GameObject RestartBtn;
    SpriteRenderer spriteRenderer;

    AudioSource audioSource;
    public AudioClip BackGround;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameQuit();
        }
    }
    void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = BackGround;
        audioSource.loop = true;

        void Start()
        {
            audioSource.Play();
        }
    }

    // Start is called before the first frame update
    public void NextStage() {
        if (stageIndex < Stages.Length - 1) {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIstage.text = "STAGE" + stageIndex;
        }
        else {
            Time.timeScale = 0;
            audioSource.Stop();
            Text btnText = RestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Game Clear!";
            RestartBtn.SetActive(true);

        }


    }
    public void LifeDown() {
        if (playerLife > 0) {
            playerLife--;
            UIlife[playerLife].color = new Color(1, 0, 0, 0.4f);
        }
        else {
            player.Die();
            audioSource.Stop();
            RestartBtn.SetActive(true);

        }
    }
    public void LifeUp() {
        playerLife++;
        UIlife[playerLife - 1].color = new Color(1, 1, 1, 1);
    }
    void PlayerReposition() {
        player.transform.position = new Vector3(-22, 3, -1);
        player.VelocityZero();
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    void GameQuit()
    {
    
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); 
        #endif
    }

}
