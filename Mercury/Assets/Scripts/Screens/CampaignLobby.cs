using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using InControl;

public class CampaignLobby : MonoBehaviour {

    public static CampaignLobby instance;

    public class CharacterSelect {
        public enum Status {
            Selecting,
            Ready
        }

        public Status status = Status.Selecting;
        public string playerID = "-1";
        public GameObject portrait;

        GameObject characterSelectGO;
        GameObject characterSelectImageGO;
        GameObject characterSelectUpArrowGO;
        GameObject characterSelectDownArrowGO;

        GameObject characterSelectedGO;        

        Sprite characterImage;
        string characterName = "no name";
        public int characterIndex = 0;

        CampaignLobby lobby;

        public CharacterSelect(string playerID, GameObject portrait, CampaignLobby lobby) {
            this.playerID = playerID;
            this.portrait = portrait;
            this.lobby = lobby;

            characterSelectGO = portrait.transform.Find("CharacterSelect_Panel").gameObject;            
            characterSelectedGO = portrait.transform.Find("CharacterSelected_Panel").gameObject;

            characterSelectImageGO = characterSelectGO.transform.Find("Character_Image").gameObject;
            characterSelectUpArrowGO = characterSelectGO.transform.Find("UpArrow_Image").gameObject;
            characterSelectDownArrowGO = characterSelectGO.transform.Find("DownArrow_Image").gameObject;

            characterSelectGO.transform.Find("UpArrow_Image").GetComponent<Button>().onClick.AddListener(Up);
            characterSelectGO.transform.Find("DownArrow_Image").GetComponent<Button>().onClick.AddListener(Down);
            
            characterSelectGO.SetActive(true);
            characterSelectedGO.SetActive(false);

            UpdatePortrait();

            portrait.transform.localScale = Vector3.zero;
        }

        public void Update() {
            if (status == Status.Selecting) {
                if (InputManager.instance.GetUpPressed(playerID)) {
                    Up();
                }
                if (InputManager.instance.GetDownPressed(playerID)) {
                    Down();
                }
                if (InputManager.instance.GetSelectPressed(playerID)) {
                    Select();
                }
                if (InputManager.instance.GetBackPressed(playerID)) {
                    Leave();
                }

                portrait.transform.localScale = Vector3.Lerp(portrait.transform.localScale, Vector3.one, Time.deltaTime * 10);

                characterSelectImageGO.transform.localScale = Vector3.Lerp(characterSelectImageGO.transform.localScale, Vector3.one, Time.deltaTime * 10);
                characterSelectUpArrowGO.transform.localScale = Vector3.Lerp(characterSelectUpArrowGO.transform.localScale, Vector3.one, Time.deltaTime * 10);
                characterSelectDownArrowGO.transform.localScale = Vector3.Lerp(characterSelectDownArrowGO.transform.localScale, Vector3.one, Time.deltaTime * 10);

            }

            if (status == Status.Ready) {
                if (InputManager.instance.GetBackPressed(playerID)) {
                    DeSelect();
                }
            }

        }

        void Select() {
            if (status == Status.Ready) {
                return;
            }

            status = Status.Ready;
            characterSelectGO.SetActive(false);
            characterSelectedGO.SetActive(true);
        }

        void DeSelect () {
            characterSelectGO.SetActive(true);
            characterSelectedGO.SetActive(false);

            status = Status.Selecting;
        }

        void Leave () {
            lobby.PlayerLeave(playerID);
        }

        void Up () {
            characterSelectUpArrowGO.transform.localScale *= 2f;

            characterIndex++;
            if (characterIndex == 4) {
                characterIndex = 0;
            }
            
            UpdatePortrait();
        }

        void Down () {
            characterSelectDownArrowGO.transform.localScale *= 2f;

            characterIndex--;
            if (characterIndex == -1) {
                characterIndex = 3;
            }

            UpdatePortrait();
        }

        void UpdatePortrait () {
            characterImage = lobby.characterPortraits[characterIndex];
            characterName = lobby.characterNames[characterIndex];

            characterSelectGO.transform.Find("Character_Image").GetComponent<Image>().sprite = characterImage;
            characterSelectedGO.transform.Find("Character_Image").GetComponent<Image>().sprite = characterImage;

            characterSelectGO.transform.Find("CharacterName_Text").GetComponent<Text>().text = characterName;
            characterSelectedGO.transform.Find("CharacterName_Text").GetComponent<Text>().text = characterName;

            characterSelectImageGO.transform.localScale *= 1.2f;
        }
    }












    public Color[] playerColors;
    public string[] characterNames;
    public Sprite[] characterPortraits;    
    
    List<CharacterSelect> characterSelectors = new List<CharacterSelect>();

    GameObject portraitPrefab;
    GameObject joinPanel;
    GameObject countdownPanel;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        portraitPrefab = GameObject.Find("Portrait_Prefab");
        joinPanel = GameObject.Find("Join_Panel");
        countdownPanel = GameObject.Find("Countdown_Panel");

        portraitPrefab.SetActive(false);
    }

    float countdown = 5;
	void Update () {
        // Leave
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (InputManager.instance.GetPlayerInput("Keyboard|0001") == null) {
                SceneManager.LoadScene("Menu");
            }
        }
        
        if (InControl.InputManager.ActiveDevice.Action2.WasPressed) {
            string controllerID = ControllerManger.instance.GetDeviceID(InControl.InputManager.ActiveDevice);
            if (InputManager.instance.GetPlayerInput(controllerID) == null) {
                SceneManager.LoadScene("Menu");
            }
        }

        // TODO: Add table realms leave


        // Selectors update
        for (int i=0; i < characterSelectors.Count; i++) {
            characterSelectors[i].Update();
        }

        // Game start
        bool ready = true;
        if (characterSelectors.Count <= 0) {
            ready = false;
        }
        for (int i=0; i < characterSelectors.Count; i ++) {
            if (characterSelectors[i].status != CharacterSelect.Status.Ready) {
                ready = false;
            }
        }

        if (ready) {
            joinPanel.SetActive(false);
            countdownPanel.SetActive(true);

            countdown -= Time.deltaTime;

            countdownPanel.transform.Find("Timer_Text").GetComponent<Text>().text = countdown.ToString("F1");
            if (countdown <= 0) {
                StartGame();
            }
        } else {
            joinPanel.SetActive(true);
            countdownPanel.SetActive(false);

            countdown = 5;
        }

        // Join
        if (Input.GetKeyDown(KeyCode.Return)) {
            PlayerJoin("Keyboard|0001", PlayerInput.InputType.Keyboard);
        }
        if (InControl.InputManager.ActiveDevice.Action1.WasPressed) {
            string controllerID = ControllerManger.instance.GetDeviceID(InControl.InputManager.ActiveDevice);
            PlayerJoin(controllerID, PlayerInput.InputType.Controller);
        }
        // TODO: Add table realms join


        // Portrait updates
        float panelWidth = 230;
        float totalWidth = characterSelectors.Count * panelWidth;
        for (int i = 0; i < characterSelectors.Count; i++) {
            RectTransform rt = characterSelectors[i].portrait.GetComponent<RectTransform>();
            Vector2 targetPos = new Vector2(-totalWidth * 0.5f + i * panelWidth + panelWidth * 0.5f, 0);
            rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetPos, Time.deltaTime * 20f);

            GameObject p = characterSelectors[i].portrait;

            Color col = playerColors[i];

            p.GetComponent<Outline>().effectColor = col;

            col *= 0.8f;
            col.a = 1;

            p.GetComponent<Image>().color = col;
            p.transform.Find("Text").GetComponent<Text>().text = "Player " + (i + 1).ToString();
            p.transform.Find("Text").GetComponent<Text>().color = col;
        }
    }

    void PlayerJoin (string playerID, PlayerInput.InputType type) {

        if (InputManager.instance.GetPlayerInput(playerID) == null) {
            PlayerInput pi = new PlayerInput(playerID, type);
            InputManager.instance.AddPlayerInput(pi);

            CreatePortrait(playerID);
        }
    }

    void PlayerLeave(string playerID) {
        PlayerInput pi = InputManager.instance.GetPlayerInput(playerID);
        InputManager.instance.RemovePlayerInput(pi);

        for (int i = 0; i < characterSelectors.Count; i ++) {
            if (characterSelectors[i].playerID == playerID) {
                Destroy(characterSelectors[i].portrait);
                characterSelectors.RemoveAt(i);

                break;
            }
        }
    }

    void StartGame () {
        if (InputManager.instance.GetPlayerInputDictionary().Count >= 1) {
            
            foreach (CharacterSelect player in characterSelectors)
            {

                PlayerData.AddPlayer(player.playerID, characterNames[player.characterIndex]);
                Debug.Log(player.portrait.name);
            }
            SceneManager.LoadScene("GameCOOP");
        }
    }

    void CreatePortrait (string playerID) {
        
        GameObject p = Instantiate(portraitPrefab);
        p.transform.SetParent(portraitPrefab.transform.parent, false);
        
        

        p.SetActive(true);

        CharacterSelect cs = new CharacterSelect(playerID, p, this);
        characterSelectors.Add(cs);
    }
}