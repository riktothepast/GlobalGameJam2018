namespace MultiplayerBasicExample
{
    using System.Collections.Generic;
    using InControl;
    using UnityEngine;

    public class MultiPlayerManager : MonoBehaviour
    {
        public List<GameObject> playerPrefab;
        public delegate void ConnectedPlayerDelegate(int playerNumber);
        public UiManagerScript uiManager;
        public ConnectedPlayerDelegate connectedPlayers;
        const int maxPlayers = 4;
        [HideInInspector]
        public List<Bot> players = new List<Bot>(maxPlayers);
        public GameBoard gameBoard;
        public EffectsService effectServices;

        void Start()
        {
            InputManager.OnDeviceDetached += OnDeviceDetached;
            connectedPlayers += uiManager.receiveCreatedPlayer;
        }


        public void CheckForControllers()
        {
            var inputDevice = InputManager.ActiveDevice;

            if (JoinButtonWasPressedOnDevice(inputDevice))
            {
                if (ThereIsNoPlayerUsingDevice(inputDevice))
                {
                    CreatePlayer(inputDevice);
                }
            }
        }


        bool JoinButtonWasPressedOnDevice(InputDevice inputDevice)
        {
            return inputDevice.Action1.WasPressed || inputDevice.Action2.WasPressed || inputDevice.Action3.WasPressed || inputDevice.Action4.WasPressed;
        }


        Bot FindPlayerUsingDevice(InputDevice inputDevice)
        {
            var playerCount = players.Count;
            for (var i = 0; i < playerCount; i++)
            {
                var player = players[i];
                if (player.Device == inputDevice)
                {
                    return player;
                }
            }

            return null;
        }


        bool ThereIsNoPlayerUsingDevice(InputDevice inputDevice)
        {
            return FindPlayerUsingDevice(inputDevice) == null;
        }


        void OnDeviceDetached(InputDevice inputDevice)
        {
            var player = FindPlayerUsingDevice(inputDevice);
            if (player != null)
            {
                RemovePlayer(player);
            }
        }

        Bot CreatePlayer(InputDevice inputDevice)
        {
            if (players.Count < maxPlayers)
            {
                Vector3 playerPosition = gameBoard.GetPosition();
                Bot player = Instantiate(playerPrefab[players.Count], playerPosition, Quaternion.identity).GetComponent<Bot>();
                player.SetGameBoard(gameBoard);
                player.Device = inputDevice;
                player.SetUIManager(uiManager);
                player.effectService = effectServices;
                player.StartEngine();
                players.Add(player);
                player.playerNumber = players.Count - 1;
                effectServices.UpdateAudioVolumeThreshold(players.Count);
                if (connectedPlayers != null)
                {
                    connectedPlayers(players.Count - 1);
                }
                return player;
            }

            return null;
        }


        void RemovePlayer(Bot player)
        {
            players.Remove(player);
            player.Device = null;
            Destroy(player.gameObject);
        }

        //void OnGUI()
        //{
        //    const float h = 22.0f;
        //    var y = 10.0f;

        //    GUI.Label(new Rect(10, y, 300, y + h), "Active players: " + players.Count + "/" + maxPlayers);
        //    y += h;

        //    if (players.Count < maxPlayers)
        //    {
        //        GUI.Label(new Rect(10, y, 300, y + h), "Press a button to join!");
        //        y += h;
        //    }
        //}
    }
}