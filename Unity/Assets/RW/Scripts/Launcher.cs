/*
 * Copyright (c) 2019 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */




using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

// begin:   audio-import
using UnityEngine.Audio;
// end:     audio-import




namespace Photon.Pun.Demo.PunBasics
{
    // begin:   documentation
    // added:   2020-11-29
    // by:      Overlord-Supreme
    // why:     Links
    /// <summary>
    /// Main Menu Button Handler
    /// </summary>
    /// Play audio with: <see href="https://docs.unity3d.com/ScriptReference/AudioSource.PlayOneShot.html">AudioSource.PlayOneShot</see>
    /// Exit game with: <see href="https://docs.unity3d.com/ScriptReference/Application.Quit.html">Application.Quit</see>
    /// Muting audio: <see href="https://forum.unity.com/threads/mute-all-sounds-in-a-game.125202/">Mute all sounds in a game</see>
    /// Settings Menu: <see href="https://www.youtube.com/watch?v=YOaYQrN1oYQ">SETTINGS MENU in Unity</see>
    /// Audio Mixing: <see href="https://www.raywenderlich.com/532-audio-tutorial-for-unity-the-audio-mixer">Audio tutorial for Unity: the Audio Mixer</see>
    [RequireComponent(typeof(AudioSource))]
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private GameObject controlPanel;

        [SerializeField]
        private Text feedbackText;

        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        bool isConnecting;

        string gameVersion = "1";

        [Space(10)]
        [Header("Custom Variables")]
        public InputField playerNameField;
        public InputField roomNameField;

        [Space(5)]
        public Text playerStatus;
        public Text connectionStatus;

        [Space(5)]
        public GameObject roomJoinUI;
        public GameObject buttonLoadArena;
        public GameObject buttonJoinRoom;
        public GameObject mainMenuUI;
        public GameObject buttonOptions;
        public GameObject buttonEndGame;

        // begin:   options-variables
        // added:   2020-11-29
        // by:      Overlord-Supreme
        // why:     Options
        [Space(5)]
        [Header("Options Refs")]
        public GameObject optionsMenuUI;
        public GameObject optionsCancel;
        public GameObject optionsConfirm;
        public GameObject optionsButtonAudioToggle;
        public AudioMixer masterMixer;
        
        // Each Option has a Current and Prior State (set on confirm/cancel)
        private bool muted = false;
        private bool wasMuted = false;

        private float volume = 0f;
        private float oldVolume = 0f;
        
        // end:     options-variables

        string playerName = "";
        string roomName = "";

        // begin:   sound-variables
        // added:   2020-11-28
        // by:      Overlord-Supreme
        // why:     sound effects
        [Header("Menu Audio")]
        public AudioSource menuAudio;
        public AudioClip audioHover;
        public AudioClip audioSelect;
        public AudioClip audioBack;
        public AudioClip audioConfirm;
        public AudioClip audioError;
        // end:     sound-variables




        // Start Method
        void Start()
        {
            //1
            PlayerPrefs.DeleteAll();

            Debug.Log("Connecting to Photon Network");

            //2
            mainMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
            roomJoinUI.SetActive(false);
            buttonLoadArena.SetActive(false);

            //3
            ConnectToPhoton();
        }




        void Awake()
        {
            //4 
            PhotonNetwork.AutomaticallySyncScene = true;
        }




        // Helper Methods
        public void SetPlayerName(string name)
        {
            playerName = name;

            // Play Confirmation
            menuAudio.PlayOneShot(audioConfirm, 1f);
        }




        public void SetRoomName(string name)
        {
            roomName = name;

            // Play Confirmation
            menuAudio.PlayOneShot(audioConfirm, 1f);
        }




        public void EnterLobbyMenu()
        {
            // mainMenuUI.SetActive(false); // Disabled to allow exiting at any time
            roomJoinUI.SetActive(true);

            // Play Confirmation
            menuAudio.PlayOneShot(audioConfirm, 1f);
        }



        
        // Tutorial Methods
        void ConnectToPhoton()
        {
            connectionStatus.text = "Connecting...";
            PhotonNetwork.GameVersion = gameVersion; //1
            PhotonNetwork.ConnectUsingSettings(); //2
        }




        public void JoinRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName; //1
                Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room " + roomNameField.text);
                RoomOptions roomOptions = new RoomOptions(); //2
                TypedLobby typedLobby = new TypedLobby(roomName, LobbyType.Default); //3
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, typedLobby); //4
            }
        }




        public void LoadArena()
        {
            // 5
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                PhotonNetwork.LoadLevel("CheckersGame");

                // Play Confirmation
                menuAudio.PlayOneShot(audioConfirm, 1f);
            }
            else
            {
                playerStatus.text = "Minimum 2 Players required to Load Arena!";

                // Play Error
                menuAudio.PlayOneShot(audioError, 1f);
            }
        }


        // begin:   ExitGame
        // added:   2020-11-29
        // by:      Overlord-Supreme
        /// <summary>
        /// Close the game from the main menu
        /// </summary>
        /// Why the macros? <see href="https://stackoverflow.com/questions/45636512/application-quit-wont-work-on-android">Application.Quit() won't work on Android</see>
        public void ExitGame()
        {
            // Play Back
            menuAudio.PlayOneShot(audioBack, 1f);

            // Exit
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        // end:     ExitGame




        // begin:   options-methods
        // added:   2020-11-29
        // by:      Overlord-Supreme
        // why:     Basic Options Configuration
        public void OpenOptions()
        {
            // Play Confirmation
            menuAudio.PlayOneShot(audioConfirm, 1f);

            // Close all other Menus
            mainMenuUI.SetActive(false);
            roomJoinUI.SetActive(false);

            // Open the Menu
            optionsMenuUI.SetActive(true);
        }

        public void ConfirmOptions()
        {
            // Play Confirmation
            menuAudio.PlayOneShot(audioConfirm, 1f);

            // Close the Menu
            optionsMenuUI.SetActive(false);

            // Check Muted
            if (muted)
            {
                wasMuted = true;
            }
            if (!muted)
            {
                wasMuted = false;
            }

            // Check Volume
            oldVolume = volume;

            // Re-open all other Menus
            mainMenuUI.SetActive(true);
            roomJoinUI.SetActive(true);
        }

        public void CancelOptions()
        {
            // Play Cancel
            menuAudio.PlayOneShot(audioBack, 1f);

            // Close the Menu
            optionsMenuUI.SetActive(false);

            // Check Muted
            if (muted != wasMuted)
            {
                muteAudio();
            }

            // Check Volume
            if (volume != oldVolume)
            {
                setAudio(oldVolume);
            }

            // Re-open all other Menus
            mainMenuUI.SetActive(true);
            roomJoinUI.SetActive(true);
        }

        public void setAudio(float newVolume)
        {
            // Play Select
            menuAudio.PlayOneShot(audioSelect, 0.1f);

            // Handle Mute State
            if (muted)
            {
                // UN-Mute it
                muted = false;
            }

            // DEBUG
            Debug.Log(newVolume);

            // Set it
            masterMixer.SetFloat("volume", newVolume);
            volume = newVolume;
        }

        public void muteAudio()
        {
            // Play Select
            menuAudio.PlayOneShot(audioSelect, 1f);

            if (!muted)
            {
                // Mute it
                masterMixer.SetFloat("volume", -80f);
                muted = true;
                return;
            }

            if (muted)
            {
                // UN-Mute it
                masterMixer.SetFloat("volume", volume);
                muted = false;
                return;
            }
        }
        // end:     options-methods




        // Photon Methods
        public override void OnConnected()
        {
            // 1
            base.OnConnected();
            // 2
            connectionStatus.text = "Connected to Photon!";
            connectionStatus.color = Color.green;
            roomJoinUI.SetActive(true);
            buttonLoadArena.SetActive(false);
        }




        public override void OnDisconnected(DisconnectCause cause)
        {
            // 3
            isConnecting = false;
            controlPanel.SetActive(true);
            Debug.LogError("Disconnected. Please check your Internet connection.");
        }




        public override void OnJoinedRoom()
        {
            // 4
            if (PhotonNetwork.IsMasterClient)
            {
                buttonLoadArena.SetActive(true);
                buttonJoinRoom.SetActive(false);
                playerStatus.text = "Your are Lobby Leader";
            }
            else
            {
                playerStatus.text = "Connected to Lobby";
            }
        }
    }
}
