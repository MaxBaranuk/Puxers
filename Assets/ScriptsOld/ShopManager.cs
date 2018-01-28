using UnityEngine;
using UnityEngine.UI;

namespace ScriptsOld
{
    public class ShopManager : MonoBehaviour {

        GameObject pushersPanel;
        GameObject backPanel;
        GameObject soundsPanel;
        GameObject powerPanel;

//    GameObject musicDetailPanel;
        GameObject soundDetailPanel;
//    GameObject extraDetailPanel;

        Image pusherButton;
        Image backButton;
        Image soundButton;
        Image powerButton;

//    Image soundDetailButton;
//    Image musicDetailButton;
//    Image extraDetailButton;

        Color selectedTop;
        Color selectedItem;
        Color unselected;

        GameObject settingsPanel;

        // Use this for initialization
        void Start() {

            pushersPanel = transform.Find("Panel").Find("ContentPanel").Find("PushersPanel").gameObject;
            backPanel = transform.Find("Panel").Find("ContentPanel").Find("BackPanel").gameObject;
            soundsPanel = transform.Find("Panel").Find("ContentPanel").Find("SoundsPanel").gameObject;
            powerPanel = transform.Find("Panel").Find("ContentPanel").Find("PowerPanel").gameObject;

            pusherButton = transform.Find("Panel").Find("Buttons").Find("Pushers").GetComponent<Image>();
            backButton = transform.Find("Panel").Find("Buttons").Find("Background").GetComponent<Image>();
            soundButton = transform.Find("Panel").Find("Buttons").Find("Sound").GetComponent<Image>();
            powerButton = transform.Find("Panel").Find("Buttons").Find("Power").GetComponent<Image>();

//        musicDetailPanel = soundsPanel.transform.FindChild("Detail").FindChild("Musics").gameObject;
            soundDetailPanel = soundsPanel.transform.Find("Detail").Find("Sounds").gameObject;
            //        extraDetailPanel = soundsPanel.transform.FindChild("Detail").FindChild("Effects").gameObject;

            //        musicDetailButton = soundsPanel.transform.FindChild("Buts").FindChild("Music").GetComponent<Image>();
            //        soundDetailButton = soundsPanel.transform.FindChild("Buts").FindChild("Sound").GetComponent<Image>();
            //        extraDetailButton = soundsPanel.transform.FindChild("Buts").FindChild("Effects").GetComponent<Image>();

            settingsPanel = GameObject.Find("Canvas").transform.Find("Panel").Find("Settings").gameObject;

            selectedTop = new Color(.0392f, .0392f, .0392f, 1);
            selectedItem = new Color(.1529f, .1529f, .1529f, 1);
            unselected = new Color(0, 0, 0, 1);

            pusherButton.GetComponent<Image>().color = selectedTop;
            pushersPanel.SetActive(true);
            //       musicDetailPanel.SetActive(true);
//        musicDetailButton.color = selectedItem;
        }

        // Update is called once per frame
        void Update() {

        }

        public void CloseShop() {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        public void OpenSettings() {
            settingsPanel.SetActive(true);
            gameObject.SetActive(false);
        }

        public void PushersSelect() {
            backButton.color = unselected;
            soundButton.color = unselected;
            powerButton.color = unselected;
            backPanel.SetActive(false);
            soundsPanel.SetActive(false);
            powerPanel.SetActive(false);

            pusherButton.GetComponent<Image>().color = selectedTop;
            pushersPanel.SetActive(true);
        }

        public void BackSelect() {
            pusherButton.color = unselected;
            soundButton.color = unselected;
            powerButton.color = unselected;
            pushersPanel.SetActive(false);
            soundsPanel.SetActive(false);
            powerPanel.SetActive(false);

            backButton.color = selectedTop;
            backPanel.SetActive(true);
        }

        public void SoundsSelect() {
            pusherButton.color = unselected;
            backButton.color = unselected;
            powerButton.color = unselected;
            pushersPanel.SetActive(false);
            backPanel.SetActive(false);
            powerPanel.SetActive(false);

            soundButton.color = selectedTop;
            soundsPanel.SetActive(true);
        }

        public void PowerSelect()
        {
            pusherButton.color = unselected;
            backButton.color = unselected;
            soundButton.color = unselected;
            pushersPanel.SetActive(false);
            backPanel.SetActive(false);
            soundsPanel.SetActive(false);

            powerButton.color = selectedTop;
            powerPanel.SetActive(true);
        }


        //public void SelectMusic() {
        //    soundDetailPanel.SetActive(false);
        //    extraDetailPanel.SetActive(false);

        //    soundDetailButton.color = selectedTop;
        //    extraDetailButton.color = selectedTop;

        //    musicDetailPanel.SetActive(true);
        //    musicDetailButton.color = selectedItem;
        //}

        //public void SelectSound() {
        //    musicDetailPanel.SetActive(false);
        //    extraDetailPanel.SetActive(false);

        //    musicDetailButton.color = selectedTop;
        //    extraDetailButton.color = selectedTop;

        //    soundDetailPanel.SetActive(true);
        //    soundDetailButton.color = selectedItem;
        //}

        //public void SelectEffects() {
        //    musicDetailPanel.SetActive(false);
        //    soundDetailPanel.SetActive(false);

        //    musicDetailButton.color = selectedTop;
        //    soundDetailButton.color = selectedTop;

        //    extraDetailPanel.SetActive(true);
        //    extraDetailButton.color = selectedItem;
        //}
    }
}
