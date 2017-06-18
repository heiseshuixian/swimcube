using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManger : MonoBehaviour
{
    public static GameManger me;
    public GameObject mainUI;
    public GameObject CtrlUI;
    public GameObject gameoverUI;
    public Text deathMsg;
    public Image deathType;
    public Button btn_replay;
    private string deathPath = "Sprite/death";
    private string winPath = "Sprite/win";
    public GameObject player;
    public GameObject fun;
    public GameObject map;
    public GameObject[] EmeyPrefab;
    public int selectLevel = 0;
    public int target = 0;
    public int currentTarget = 0;
    void Awake()
    {

        me = this;
        gameoverUI.SetActive(false);
        btn_replay.onClick.AddListener(Replay);
        CsvLevel.Init();
    }
    private void OnEnable()
    {
        TriggerManager.GetInstance().AddTrigger(E_TriggerType.on_level_selected, StartGame);
        TriggerManager.GetInstance().AddTrigger(E_TriggerType.on_get_gift, OnGetGift);
    }
    private void OnDisable()
    {
        TriggerManager.GetInstance().RemoveTrigger(E_TriggerType.on_level_selected, StartGame);
        TriggerManager.GetInstance().RemoveTrigger(E_TriggerType.on_get_gift, OnGetGift);
    }
    public void StartGame(object obj)
    {
        fun.SetActive(false);
        for (int index = 0; index < CsvLevel.items[selectLevel].etype.Count; index++)
        {
            for (int num = 0; num < CsvLevel.items[selectLevel].etype[index].num; num++)
            {
                GameObject oneEnemy = GameObject.Instantiate(EmeyPrefab[CsvLevel.items[selectLevel].etype[index].type - 1], map.transform);
            }
        }
        mainUI.SetActive(false);
        CtrlUI.SetActive(true);
        player.SetActive(true);

        TriggerManager.GetInstance().Trigger(E_TriggerType.on_game_start);
    }
    public void OnGetGift(object obj) {
        currentTarget++;
        if (currentTarget>=target) {
            ShowGameOver(4);
        }
    }
    public void ShowGameOver(int type)
    {
        gameoverUI.SetActive(true);
        TriggerManager.GetInstance().Trigger(E_TriggerType.on_game_over);
        CtrlUI.SetActive(false);
        player.SetActive(false);
        switch (type)
        {
            case 1:
                deathMsg.text = "你被鲨鱼吃掉了";
                deathType.sprite = Resources.Load<Sprite>(deathPath + type);
                break;
            case 2:
                deathMsg.text = "你炸死了";
                deathType.sprite = Resources.Load<Sprite>(deathPath + type);
                break;
            case 3:
                deathMsg.text = "你被憋死了";
                deathType.sprite = Resources.Load<Sprite>(deathPath + type);
                break;
            case 4:
                deathMsg.text = "恭喜你！获得了胜利！";
                deathType.sprite = Resources.Load<Sprite>(winPath + "1");
                if (selectLevel == PlayerPrefs.GetInt("levelindex")+1) {
                    PlayerPrefs.SetInt("levelindex", PlayerPrefs.GetInt("levelindex") + 1);
                }
                break;
            default:
                deathMsg.text = "你死了";
                break;
        }

        O2Ctrl.me.isOver = true;
        SoundManager.me.isOver = true;
    }
    public void Replay()
    {
        SceneManager.LoadSceneAsync(0); ;
    }
}
