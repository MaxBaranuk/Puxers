using UnityEngine;
using UnityEngine.UI;

namespace ScriptsOld
{
    public class SettingsManager : MonoBehaviour {

        GameObject pushersPanel;
        GameObject backPanel;
        GameObject soundsPanel;

        GameObject musicDetailPanel;
        GameObject soundDetailPanel;
        GameObject extraDetailPanel;

        Image pusherButton;
        Image backButton;
        Image soundButton;

        Image soundDetailButton;
        Image musicDetailButton;
        Image extraDetailButton;

        Color selectedTop;
        Color selectedItem;
        Color unselected;

        GameObject shopPanel;

        // Use this for initialization
        void Start() {

            pushersPanel = transform.Find("Panel").Find("ContentPanel").Find("PushersPanel").gameObject;
            backPanel = transform.Find("Panel").Find("ContentPanel").Find("BackPanel").gameObject;
            soundsPanel = transform.Find("Panel").Find("ContentPanel").Find("SoundsPanel").gameObject;

            pusherButton = transform.Find("Panel").Find("Buttons").Find("Pushers").GetComponent<Image>();
            backButton = transform.Find("Panel").Find("Buttons").Find("Background").GetComponent<Image>();
            soundButton = transform.Find("Panel").Find("Buttons").Find("Sound").GetComponent<Image>();

            musicDetailPanel = soundsPanel.transform.Find("Detail").Find("Musics").gameObject;
            soundDetailPanel = soundsPanel.transform.Find("Detail").Find("Sounds").gameObject;
            extraDetailPanel = soundsPanel.transform.Find("Detail").Find("Effects").gameObject;

            musicDetailButton = soundsPanel.transform.Find("Buts").Find("Music").GetComponent<Image>();
            soundDetailButton = soundsPanel.transform.Find("Buts").Find("Sound").GetComponent<Image>();
            extraDetailButton = soundsPanel.transform.Find("Buts").Find("Effects").GetComponent<Image>();

            shopPanel = GameObject.Find("Canvas").transform.Find("Panel").Find("Shop").gameObject;

            selectedTop = new Color(.0392f, .0392f, .0392f, 1);
            selectedItem = new Color(.1529f, .1529f, .1529f, 1);
            unselected = new Color(0, 0, 0, 1);

            pusherButton.GetComponent<Image>().color = selectedTop;
            pushersPanel.SetActive(true);
            musicDetailPanel.SetActive(true);
            musicDetailButton.color = selectedItem;
        }

        // Update is called once per frame
        void Update() {

        }

        public void CloseSettings() {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        public void PushersSelect() {
            backButton.color = unselected;
            soundButton.color = unselected;
            backPanel.SetActive(false);
            soundsPanel.SetActive(false);

            pusherButton.GetComponent<Image>().color = selectedTop;
            pushersPanel.SetActive(true);
        }

        public void BackSelect() {
            pusherButton.color = unselected;
            soundButton.color = unselected;
            pushersPanel.SetActive(false);
            soundsPanel.SetActive(false);

            backButton.color = selectedTop;
            backPanel.SetActive(true);
        }

        public void SoundsSelect() {
            pusherButton.color = unselected;
            backButton.color = unselected;
            pushersPanel.SetActive(false);
            backPanel.SetActive(false);

            soundButton.color = selectedTop;
            soundsPanel.SetActive(true);
        }

        public void SelectMusic() {
            soundDetailPanel.SetActive(false);
            extraDetailPanel.SetActive(false);

            soundDetailButton.color = selectedTop;
            extraDetailButton.color = selectedTop;

            musicDetailPanel.SetActive(true);
            musicDetailButton.color = selectedItem;
        }

        public void SelectSound() {
            musicDetailPanel.SetActive(false);
            extraDetailPanel.SetActive(false);

            musicDetailButton.color = selectedTop;
            extraDetailButton.color = selectedTop;

            soundDetailPanel.SetActive(true);
            soundDetailButton.color = selectedItem;
        }

        public void SelectEffects() {
            musicDetailPanel.SetActive(false);
            soundDetailPanel.SetActive(false);

            musicDetailButton.color = selectedTop;
            soundDetailButton.color = selectedTop;

            extraDetailPanel.SetActive(true);
            extraDetailButton.color = selectedItem;
        }

        public void OpenShopPanel() {
            shopPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
