using ActionSample.Components.Ui;
using ActionSample.Components.Unit;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace ActionSample.Scenes
{
    public class BattleSceneController : MonoBehaviour
    {
        [SerializeField]
        Camera mainCamera;

        [SerializeField]
        GameObject player;

        [SerializeField]
        Joystick joystick;

        PixelPerfectCamera pixelPerfectCamera;

        GameObject frontWall;
        GameObject rearWall;
        GameObject leftWall;
        GameObject rightWall;

        StatusCardOrganizer statusCardOrganizer = null;

        private void Start()
        {
            this.pixelPerfectCamera = this.mainCamera.GetComponent<PixelPerfectCamera>();

            this.BuildWall();
            this.mainCamera.transform.eulerAngles = new Vector3(30.0f, 0, 0);
            statusCardOrganizer = GameObject.FindGameObjectWithTag("status_card_organizer").GetComponent<StatusCardOrganizer>();
            statusCardOrganizer.Add(player.GetComponent<IUnit>());
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            this.handleInput();
        }


        void LateUpdate()
        {
            float screenW = this.pixelPerfectCamera.refResolutionX;
            float screenH = this.pixelPerfectCamera.refResolutionY;
            float worldWidth = 2000;
            float worldHeight = 300;
            float cameraX = Mathf.Clamp(this.player.transform.position.x, screenW / 2, worldWidth - (screenW / 2));
            float cameraY = Mathf.Clamp(this.player.transform.position.y, screenH / 2, worldHeight - (screenH / 2));

            this.mainCamera.transform.position = new Vector3(cameraX, cameraY);

        }

        private void handleInput()
        {
            Vector2 direction = joystick.Direction;

            float forceX = (Mathf.Abs(direction.x) > 0) ? Mathf.Clamp(direction.x, -1, 1) : 0.0f;
            float forceZ = (Mathf.Abs(direction.y) > 0) ? Mathf.Clamp(direction.y, -1, 1) : 0.0f;

            //// 移動に関するイベントを送信するようにする
            this.player.GetComponent<IUnit>().MoveToward(forceX, forceZ);
        }


        private void BuildWall()
        {
            this.frontWall = new GameObject("Front Wall");
            this.frontWall.transform.position = new Vector3(-100, -100, 200);
            BoxCollider frontCollider = this.frontWall.AddComponent<BoxCollider>();
            frontCollider.size = new Vector3(2000 + 100, 360 + 100, 10);
            frontCollider.center = frontCollider.size / 2;
            frontCollider.tag = "obstacle";

            this.rearWall = new GameObject("Rear Wall");
            this.rearWall.transform.position = new Vector3(-100, -100, -20);
            BoxCollider rearCollider = this.rearWall.AddComponent<BoxCollider>();
            rearCollider.size = new Vector3(2000 + 100, 360 + 100, 10);
            rearCollider.center = rearCollider.size / 2;
            rearCollider.tag = "obstacle";

            this.leftWall = new GameObject("Left Wall");
            this.leftWall.transform.position = new Vector3(-100, -100, 0);
            BoxCollider leftCollider = this.leftWall.AddComponent<BoxCollider>();
            leftCollider.size = new Vector3(100, 360 + 100, 200);
            leftCollider.center = leftCollider.size / 2;
            leftCollider.tag = "obstacle";

            this.rightWall = new GameObject("Right Wall");
            this.rightWall.transform.position = new Vector3(2000, -100, 0);
            BoxCollider rightCollider = this.rightWall.AddComponent<BoxCollider>();
            rightCollider.size = new Vector3(100 + 100, 360 + 100, 200);
            rightCollider.center = rightCollider.size / 2;
            rightCollider.tag = "obstacle";

        }
    }

}
