using UnityEngine;
using System.Collections;
using Assets.Script.Component;
using UnityEngine.Networking;
using System;

namespace Assets.Script.chess_thai
{
    public class NetworkManagement : NetworkManager
    {
        int gameId;
        public string matchId;
        public string hostName = "MakRukOnline";

        private bool isRefreshingHostList;
        private HostData[] hostList;
        public GameObject playerPrefabWhite;
        public GameObject playerPrefabBlack;
        public ReadData readData;
        public bool gameIsStart;
        bool firstTime;
        //bool opponentIsDisconnect;

        void Start()
        {
            MasterServer.ipAddress = readData.userData.xsignature;
            Network.natFacilitatorIP = readData.userData.xsignature;
            MasterServer.port = 23466;
            Network.natFacilitatorPort = 50005;

            isRefreshingHostList = false;
            gameIsStart = false;
            firstTime = true;
            //opponentIsDisconnect = false;
            gameId = readData.setting.gameId;
            matchId = hostName+readData.match.matchId;
            if (readData.match.isHost == 2)
            {
                StartGameServer();
            }
            else if (readData.match.isHost == 1)
            {
                RefreshHostList();
                if (hostList != null)
                {
                    JoinServer(hostList[0]);
                }
            }

            //Application.RegisterLogCallback(LogCallback);
        }

        /*void OnGUI()
        {
            if (!Network.isClient && !Network.isServer)
            {
                if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                    StartGameServer();

                if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                    RefreshHostList();

                if (hostList != null)
                {
                    for (int i = 0; i < hostList.Length; i++)
                    {
                        if (GUI.Button(new Rect(100, 250 + (150 * (i + 1)), 250, 100), hostList[i].gameName))
                            JoinServer(hostList[i]);
                    }
                }
            }
        }*/

        public void StartGameServer()
        {
            //Debug.Log("StartGameServer");
            gameIsStart = true;
            Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
            MasterServer.RegisterHost(matchId, hostName);
        }

        private void RefreshHostList()
        {
            if (!isRefreshingHostList)
            {
                //Debug.Log("RefreshHostList");
                isRefreshingHostList = true;
                MasterServer.RequestHostList(matchId);
            }
        }

        void Update()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (readData.match.isHost == 2)
                {
                    InternetServerDown();
                }
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().OpenInternetProblemBlock();
            }
            else if (gameIsStart)
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CloseInternetProblemBlock();
            }

            if (isRefreshingHostList && MasterServer.PollHostList().Length > 0)
            {
                isRefreshingHostList = false;
                hostList = MasterServer.PollHostList();
            }

            if (readData.match.isHost == 1 && !gameIsStart)
            {
                if (hostList != null && Application.internetReachability != NetworkReachability.NotReachable)
                {
                    JoinServer(hostList[0]);
                }
            }
            else if (readData.match.isHost == 2 && !gameIsStart)
            {
                if (Application.internetReachability != NetworkReachability.NotReachable)
                {
                    StartGameServer();
                }
            }
        }

        public void JoinServer(HostData hostData)
        {
            //Debug.Log("JoinServer");
            gameIsStart = true;
            Network.Connect(hostData);
        }

        public void OnServerInitialized()
        {
            //Debug.Log("OnServerInitialized");
            if (firstTime)
            {
                firstTime = false;
                Network.Instantiate(playerPrefabBlack, Vector3.zero, Quaternion.identity, 0);
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().GameStart(true);
                StartCoroutine(GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().SetOnePlayer(readData.userData.displayName, readData.userData.statuser[gameId - 1].score, true));
            }
        }

        public void OnConnectedToServer()
        {
            //Debug.Log("OnConnectedToServer");
            GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CloseInternetProblemBlock();
            GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CloseOpponentInternetProblemBlock();
            if (firstTime)
            {
                firstTime = false;
                Network.Instantiate(playerPrefabWhite, Vector3.zero, Quaternion.identity, 0);
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().GameStart(false);
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StartCountdownTime(false);
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().isGameOver = false;
                StartCoroutine(GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().SetPlayer(readData.userData.displayName, readData.userData.statuser[gameId - 1].score, readData.match.oppenentDisplayname, readData.match.oppenentScore, false));
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CloseLoading();
            }
            else
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StartCountdownTimeReconnected();
            }
        }

        public void OnPlayerConnected(NetworkPlayer player)
        {
            //Debug.Log("OnPlayerConnected");
            GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CloseInternetProblemBlock();
            GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CloseOpponentInternetProblemBlock();
            if (GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().iAmBlack && GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().playerMin2 == 45)
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StartCountdownTime(true);
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().isGameOver = false;
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().CloseLoading();
            }
            else
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StartCountdownTimeReconnected();
            }
        }
        public void OnPlayerDisconnected(NetworkPlayer player)
        {
            //Debug.Log("Clean up after player " + player);
            if (readData.match.isHost == 2)
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StartCountdownDisconnectTime(false);
            }
            GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().OpenOpponentInternetProblemBlock();
        }

        public void OnDisconnectedFromServer(NetworkDisconnection info) // server disconnect
        {
            //Debug.Log("Disconnected from server: " + info);
            if (readData.match.isHost == 1)
            {
                InternetClientDown();
            }
            if (info == NetworkDisconnection.LostConnection)
            {
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StartCountdownDisconnectTime(true);
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().OpenOpponentInternetProblemBlock();
            }
        }

        /*private void LogCallback(string condition, string stackTrace, LogType type)
        {
            Debug.Log("LogCallback");
            if (type == LogType.Error)
            {
                const string MessageBeginning = "Receiving NAT punchthrough attempt from target";
                if (condition.StartsWith(MessageBeginning, StringComparison.Ordinal))
                {
                    OnFailedToConnect(NetworkConnectionError.NATPunchthroughFailed);
                }
            }
        }*/

        public void OnFailedToConnectToMasterServer(NetworkConnectionError info)
        {
            //Debug.Log("Could not connect to master server: " + info);
        }

        public void OnFailedToConnect(NetworkConnectionError error)
        {
            //Debug.Log("Could not connect to server: " + error);
            if (readData.match.isHost == 1)
            {
                JoinServer(hostList[0]);
            }
        }

        public void InternetClientDown()
        {
            if (gameIsStart)
            {
                //Debug.Log("InternetClientDown");
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StopCountdownTime();
                gameIsStart = false;
            }
        }

        public void InternetServerDown()
        {
            if (gameIsStart)
            {
                //Debug.Log("InternetServerDown");
                GameObject.Find("PlayOnlineScript").GetComponent<PlayOnline>().StopCountdownTime();
                gameIsStart = false;
                MasterServer.ClearHostList();
                MasterServer.UnregisterHost();
            }
        }

        public void QuitNetwork()
        {
            //Debug.Log("QuitNetwork");
            MasterServer.ClearHostList();
            MasterServer.UnregisterHost();
            Shutdown();
            Destroy(gameObject);
        }
    }
}
