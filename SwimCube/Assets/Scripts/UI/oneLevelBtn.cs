using UnityEngine;
using UnityEngine.UI;

class oneLevelBtn : MonoBehaviour
{
    public int levelIndex = 0;
    public Button btn;
    public Text lab_levelIndex;

    public void InitUI() {
        lab_levelIndex.text = levelIndex.ToString();
        if (PlayerPrefs.GetInt("levelindex") + 1 < levelIndex) {
            GetComponent<Image>().color = Color.gray;
        }
        btn.onClick.AddListener(()=> {
            if (PlayerPrefs.GetInt("levelindex")+1>= levelIndex) {
                GameManger.me.selectLevel = levelIndex;
                GameManger.me.target = CsvLevel.items[levelIndex].param1;
                TriggerManager.GetInstance().Trigger(E_TriggerType.on_level_selected);
            }
        });
    }

}

