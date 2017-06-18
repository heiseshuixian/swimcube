using Giu.Basic;
using UnityEngine;
using UnityEngine.UI;

class MainPanel : MonoBehaviour
{
    public Button btn_level1;
    public Button btn_level2;
    public Button btn_level3;

    public Button btn_back2main;
    public GameObject mainPanel;
    public GameObject selectLevelPanel;


    public Transform LevelPanel;
    public GameObject onelevelprefab;
    private Seq<oneLevelBtn> levelbtnList=new Seq<oneLevelBtn>();
    private void Start()
    {
        InitUI();
    }
    void InitUI() {
        btn_level1.onClick.AddListener(InitLevelSelect);
        mainPanel.SetActive(true);
        selectLevelPanel.SetActive(false);
        btn_back2main.onClick.AddListener(
            () => {
                mainPanel.SetActive(true);
                selectLevelPanel.SetActive(false);
            }
            );
    }
    void InitLevelSelect() {
        for (int index =0;index <CsvLevel.items.Count ;index++) {
            if (levelbtnList.Count >= index + 1)
            {

            }
            else {
              GameObject newone =   Instantiate<GameObject>(onelevelprefab, LevelPanel);
                levelbtnList.Add(newone.GetComponent<oneLevelBtn>());

            }
            levelbtnList[index].gameObject.SetActive(true);
            levelbtnList[index].levelIndex = index+1;
            levelbtnList[index].InitUI();
        }
        for (int index = CsvLevel.items.Count; index < levelbtnList.Count; index++ ) {
            levelbtnList[index].gameObject.SetActive(false);
        }
        mainPanel.SetActive(false);
        selectLevelPanel.SetActive(true);
    }
}

