using ECS.Components;
using Unity.Entities;
using UnityEngine;

namespace ECS.Systems
{
    public class InputProcessorSystem : ComponentSystem
    {
        public struct InputGroup
        {
            public InputData InputData;
            public Camera Camera;
        }
        
        protected override void OnUpdate()
        {
            InputData inputData = null;
            Camera camera = null;
            foreach (var entity in GetEntities<InputGroup>())
            {
                inputData = entity.InputData;
                camera = entity.Camera;
                break;
            }

            if (inputData == null)
                return;

            //NOTE: Only for the purpose of the prototype
            if (Input.touchCount > 0)
            {
                ProcessTouchInput(camera, inputData);
            }
            else
            {
                ProcessMouseInput(camera, inputData);
            }
            
            ProcessSwipe(inputData);
        }

        private void ProcessTouchInput(Camera camera, InputData inputData)
        {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                inputData.DownPos = camera.ScreenToWorldPoint(touch.position);
            if (touch.phase != TouchPhase.Moved && touch.phase != TouchPhase.Ended)
                return;

            inputData.UpPos = camera.ScreenToWorldPoint(touch.position);
        }

        private void ProcessMouseInput(Camera camera, InputData inputData)
        {
            if (Input.GetMouseButtonDown(0))
                inputData.DownPos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
            
            if (Input.GetMouseButtonUp(0) == false)
                return;
            
            inputData.UpPos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        }

        private void ProcessSwipe(InputData inputData)
        {
            inputData.SwipeDirection = inputData.UpPos - inputData.DownPos;
            inputData.SwipeDirection.Normalize();

            Vector2 absoluteVal =
                new Vector2(Mathf.Abs(inputData.SwipeDirection.x), Mathf.Abs(inputData.SwipeDirection.y)); 
            if (absoluteVal.x > absoluteVal.y)
            {
                inputData.SwipeDirection.y = 0f;
                inputData.SwipeDirection.x = Mathf.Sign(inputData.SwipeDirection.x);
            }
            else if (absoluteVal.x < absoluteVal.y)
            {
                inputData.SwipeDirection.x = 0f;
                inputData.SwipeDirection.y = Mathf.Sign(inputData.SwipeDirection.y);
            }
            else
            {
                inputData.SwipeDirection.y = 0f;
                inputData.SwipeDirection.x = 0f;
            }
            
            inputData.IsSwipe = inputData.SwipeDirection != Vector2.zero;
        }
    }
}