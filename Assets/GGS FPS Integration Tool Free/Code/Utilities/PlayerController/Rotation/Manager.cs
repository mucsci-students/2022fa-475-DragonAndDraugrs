using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.PlayerController.Rotation
{
    public class Manager
    {
        Vector2 _MouseDifference = Vector2.zero;


        CharacterController CharacterControllerProperty { get; set; }
        Camera CharacterEnvironmentCamera { get; set; }
        string MouseXAxisName { get; set; }
        string MouseYAxisName { get; set; }
        float MouseXSensitivity { get; set; } = 1f;
        float MouseYSensitivity { get; set; } = 1f;

        //bool IsCursorLocked { get; set; } = true;
        Vector2 MouseDifference { get; set; } = Vector2.zero; // ? is init needed?

        public Manager(
            CharacterController characterControllerProperty,
            Camera characterEnvironmentCamera,
            string mouseXAxisName,
            string mouseYAxisName,
            float mouseXSensitivity,
            float mouseYSensitivity
            )
        {
            CharacterControllerProperty = characterControllerProperty;
            CharacterEnvironmentCamera = characterEnvironmentCamera;
            MouseXAxisName = mouseXAxisName;
            MouseYAxisName = mouseYAxisName;
            MouseXSensitivity = mouseXSensitivity;
            MouseYSensitivity = mouseYSensitivity;
        }

        public void Update()
        {
            // #


            // Cursor locking input
            //if (Input.GetKeyUp(KeyCode.Escape))
            //{
            //    IsCursorLocked = false;
            //}
            //else if (Input.GetMouseButtonUp(0))
            //{
            //    IsCursorLocked = true;
            //}

            // Cursor locking
            // Functionality has been moved, thus checks Cursor.lockState if it has changed
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                MouseDifference = new Vector2(Input.GetAxis(MouseXAxisName), Input.GetAxis(MouseYAxisName));
            }
            else
            {
                MouseDifference = Vector2.zero;
            }

            // #

            //if (IsCursorLocked)
            //{
            //    Cursor.lockState = CursorLockMode.Locked;
            //    Cursor.visible = false;

            //    MouseDifference = new Vector2(Input.GetAxis(MouseXAxisName), Input.GetAxis(MouseYAxisName));
            //}
            //else
            //{
            //    Cursor.lockState = CursorLockMode.None;
            //    Cursor.visible = true;

            //    MouseDifference = Vector2.zero;
            //}

            _MouseDifference.x *= MouseXSensitivity;
            _MouseDifference.y *= MouseYSensitivity;

            // Vertical camera rotation
            Quaternion xAxisRotation = Quaternion.AngleAxis(MouseDifference.y, -Vector3.right);

            Quaternion cameraRotation = CharacterEnvironmentCamera.transform.localRotation;
            cameraRotation *= xAxisRotation;

            cameraRotation.x = Mathf.Clamp(cameraRotation.x, -0.707f, 0.707f);
            cameraRotation.w = Mathf.Clamp(cameraRotation.w, 0.707f, 2f);

            CharacterEnvironmentCamera.transform.localRotation = cameraRotation;

            // Horizontal character rotation
            Quaternion yAxisRotation = Quaternion.AngleAxis(MouseDifference.x, Vector3.up);

            Quaternion characterRotation = CharacterControllerProperty.transform.localRotation;
            characterRotation *= yAxisRotation;

            CharacterControllerProperty.transform.localRotation = characterRotation;
        }
    }
}