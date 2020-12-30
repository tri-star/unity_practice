using ActionSample.Components;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace ActionSample.Scenes
{
    public class BattleSceneController : MonoBehaviour
    {
        Camera _mainCamera;

        [SerializeField]
        GameObject _player;

        [SerializeField]
        Joystick _joystick;

        PixelPerfectCamera _pixelPerfectCamera;

        GameObject _frontWall;
        GameObject _rearWall;
        GameObject _leftWall;
        GameObject _rightWall;

        private void Start()
        {
            this._mainCamera = Component.FindObjectOfType<Camera>();
            this._pixelPerfectCamera = this._mainCamera.GetComponent<PixelPerfectCamera>();

            this.BuildWall();
        }

        // Update is called once per frame
        void Update()
        {
            float screenW = this._pixelPerfectCamera.refResolutionX;
            float screenH = this._pixelPerfectCamera.refResolutionY;
            float worldWidth = 2000;
            float worldHeight = 360;
            float cameraX = Mathf.Clamp(this._player.transform.position.x, screenW / 2, worldWidth - (screenW / 2));
            float cameraY = Mathf.Clamp(this._player.transform.position.y, screenH / 2, worldHeight - (screenH / 2));

            this.handleInput();

            this._mainCamera.transform.position = new Vector3(cameraX, cameraY);


        }


        private void handleInput()
        {
            Vector2 direction = _joystick.Direction;

            float forceX = (Mathf.Abs(direction.x) > 0) ? Mathf.Clamp(direction.x, -1, 1) : 0.0f;
            float forceZ = (Mathf.Abs(direction.y) > 0) ? Mathf.Clamp(direction.y, -1, 1) : 0.0f;

            //// 移動に関するイベントを送信するようにする
            this._player.GetComponent<Unit>().MoveToward(forceX, forceZ);
        }


        private void BuildWall()
        {
            this._frontWall = new GameObject("Front Wall");
            this._frontWall.transform.position = new Vector3(-100, -100, 200);
            BoxCollider frontCollider = this._frontWall.AddComponent<BoxCollider>();
            frontCollider.size = new Vector3(2000 + 100, 360 + 100, 10);
            frontCollider.center = frontCollider.size / 2;
            frontCollider.tag = "obstacle";

            this._rearWall = new GameObject("Rear Wall");
            this._rearWall.transform.position = new Vector3(-100, -100, -20);
            BoxCollider rearCollider = this._rearWall.AddComponent<BoxCollider>();
            rearCollider.size = new Vector3(2000 + 100, 360 + 100, 10);
            rearCollider.center = rearCollider.size / 2;
            rearCollider.tag = "obstacle";

            this._leftWall = new GameObject("Left Wall");
            this._leftWall.transform.position = new Vector3(-100, -100, 0);
            BoxCollider leftCollider = this._leftWall.AddComponent<BoxCollider>();
            leftCollider.size = new Vector3(100, 360 + 100, 200);
            leftCollider.center = leftCollider.size / 2;
            leftCollider.tag = "obstacle";

            this._rightWall = new GameObject("Right Wall");
            this._rightWall.transform.position = new Vector3(2000, -100, 0);
            BoxCollider rightCollider = this._rightWall.AddComponent<BoxCollider>();
            rightCollider.size = new Vector3(100 + 100, 360 + 100, 200);
            rightCollider.center = rightCollider.size / 2;
            rightCollider.tag = "obstacle";

        }
    }

}
