using System;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

namespace CWUEssentialsCheat
{
    public class PingMasterClient : MonoBehaviour
    {
        public bool MasterHasCheat { get; private set; }
        private float _cooldown = .5f;
        private float _timeLeftCooldown;
        private bool _pinged;
        private PhotonView _photonView;
        public static PingMasterClient Instance;

        public static PingMasterClient Init()
        {
            if (Instance) return Instance;
            var pmc = new GameObject(nameof(PingMasterClient)).AddComponent<PingMasterClient>();
            pmc._photonView = pmc.gameObject.AddComponent<PhotonView>();
            pmc._photonView.ViewID = 7767;
            pmc._photonView.Synchronization = ViewSynchronization.ReliableDeltaCompressed;
            DontDestroyOnLoad(pmc.gameObject);
            Instance = pmc;
            return pmc;
        }
        
        private void Ping()
        {
            _pinged = false;
            _photonView.RPC(nameof(RPCSendMasterPingEp1), RpcTarget.MasterClient);
        }

        [PunRPC]
        private void RPCSendMasterPingEp2()
        {
            _pinged = true;
        }

        [PunRPC]
        private void RPCSendMasterPingEp1(PhotonMessageInfo info)
        {
            _photonView.RPC(nameof(RPCSendMasterPingEp2), info.Sender);
        }

        private void Update()
        {
            _timeLeftCooldown += Time.deltaTime;

            if (_timeLeftCooldown >= _cooldown)
            {
                MasterHasCheat = _pinged;
                _timeLeftCooldown = 0;
                Ping();
            }
        }
    }
}