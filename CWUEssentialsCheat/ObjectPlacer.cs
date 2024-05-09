using System;
using Photon.Pun;
using UnityEngine;

namespace CWUEssentialsCheat
{
    public enum OpState
    {
        None, Rotating, Positioning
    }
    public class ObjectPlacer : MonoBehaviour
    {
        private OpState _state = OpState.None;
        private FurniturePhoton _furniture;
        private Transform objectToPlace;
        private Vector3 offset;
        private Vector3 viewOffset;
        private float magnitude;
        private float cooldownSeconds;

        private Vector3 objectPosition;
        private Quaternion objectRotation;
        
        private void SpawnObject()
        {
            // PhotonNetwork.Instantiate(_furniture.ModelName, objectPosition, objectRotation);
           HackMain.Instance.photonView.RPC("RPCA_SpawnFurniture", RpcTarget.MasterClient, _furniture.ModelName, objectPosition, objectRotation);

        }

        public void ShowObject(FurniturePhoton furniture)
        {
            _furniture = furniture;
            _furniture.Position = HackValues.LookAtPos();
            offset = _furniture.Position - Player.localPlayer.HeadPosition();
            objectRotation = furniture.Rotation;
            
            objectToPlace = Resources.Load<Transform>(furniture.ModelName);
            objectToPlace = Instantiate(objectToPlace, furniture.Position, furniture.Rotation);
            magnitude = offset.magnitude;
            _state = OpState.Positioning;
            cooldownSeconds = 1f;

        }

        private void OnRightClick()
        {
            // rotate
            _state = OpState.Rotating;

            
        }
        private void OnLeftClick()
        {
            //place
            _state = OpState.None;
            Destroy(objectToPlace.gameObject);
            SpawnObject();

        }

        private void OnRotation()
        {

            var angles = new Vector3();
            angles.x -= Input.GetAxis("Mouse X");
            angles.y += Input.GetAxis("Mouse Y");
            angles.z += Input.mouseScrollDelta.y * 5f;
            objectToPlace.Rotate(angles);
           
            objectRotation = objectToPlace.rotation;
          
            
        }

        private void DrawAxis()
        {
            
        }

        private Vector2 W2S(Vector3 pos)
        {
            return Camera.main.WorldToScreenPoint(pos, Camera.MonoOrStereoscopicEye.Mono);
        }
        

        private void OnPositioning()
        {
            magnitude += Input.mouseScrollDelta.y * .1f;
            var view = (Player.localPlayer.data.lookDirection) * magnitude;
            objectPosition = Player.localPlayer.HeadPosition() + view;
            objectToPlace.transform.position = objectPosition;

            // var pointOfRotation = Player.localPlayer.HeadPosition();
            // objectToPlace.RotateAround(pointOfRotation, Vector3.up, Deg);
        }

        // private void OnGUI()
        // {
        //     GUI.color = Color.red;
        //     var angle = Player.localPlayer.data.lookDirection;
        //     GUI.TextArea(new Rect(50, 50, 200, 200), angle.ToString());
        // }

        private void Update()
        {
            switch (_state)
            {
                case OpState.None:
                    return;
                case OpState.Positioning:
                    OnPositioning();
                    break;
                case OpState.Rotating:
                    OnRotation();
                    break;
                
            }

            if (cooldownSeconds > 0)
            {
                cooldownSeconds -= Time.deltaTime;
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                OnLeftClick();
            }
            if (Input.GetMouseButton(1))
            {
                Player.localPlayer.data.hookedIntoTerminal = true;
                OnRightClick();
            }
            else
            {
                if (_state == OpState.Rotating)
                {
                    Player.localPlayer.data.hookedIntoTerminal = false;
                    _state = OpState.Positioning;
                }
            }

            if (Input.GetKey(KeyCode.Escape))
            {
                Destroy(objectToPlace.gameObject);
                _state = OpState.None;
            }
            
            
            
           
            
        }
    }
}