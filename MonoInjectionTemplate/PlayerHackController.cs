using System;
using Photon.Pun;
using UnityEngine;

namespace MonoInjectionTemplate
{
    public class PlayerHackController : MonoBehaviour
    {
        private HackMain _hackMain;
        private bool canJump = true;

        private void Awake()
        {
            canJump = true;
        }

        public void ResetFall()
        {
            canJump = true;
        }
        
        
        public void Fall(float seconds)
        {
            // if (!canJump) return;
            // canJump = false;
            // Invoke(nameof(ResetFall), seconds / 2f);
            // _hackMain.localPlayer.data.fallTime = seconds;
            // _hackMain.localPlayer.refs.view.RPC("RPCA_Fall", RpcTarget.All, seconds);
        }
        
    }
    
    
}