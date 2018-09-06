﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput {
    // A player input is a mapping between a player and a device.
    // The device can be a TableRealms device or a controller or mouse and keyboard.

    public enum InputType { TableRealms, Controller, Keyboard }

    public InputType inputType = InputType.TableRealms;
    public string playerID = "-1";

    public PlayerInput(string playerID, InputType inputType) {
        this.playerID = playerID;
        this.inputType = inputType;
    }

    // ** Methods :

    public bool GetUpPressed(string playerID) {
        if (inputType == InputType.TableRealms) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller) {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.LeftStickUp.WasPressed;
        }
        if (inputType == InputType.Keyboard) {
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                return true;
            }
        }
        return false;
    }

    public bool GetDownPressed(string playerID) {
        if (inputType == InputType.TableRealms) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller) {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.LeftStickDown.WasPressed;
        }
        if (inputType == InputType.Keyboard) {
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                return true;
            }
        }
        return false;
    }

    public bool GetSelectPressed(string playerID) {
        if (inputType == InputType.TableRealms) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller) {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.Action1.WasPressed;
        }
        if (inputType == InputType.Keyboard) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                return true;
            }
        }
        return false;
    }

    public bool GetBackPressed(string playerID) {
        if (inputType == InputType.TableRealms) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller) {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.Action2.WasPressed;
        }
        if (inputType == InputType.Keyboard) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                return true;
            }
        }
        return false;
    }


    
    public Vector2 GetMoveDirection ()
    {
        if (inputType == InputType.TableRealms)
        {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller)
        {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.LeftStick.Value;
        }
        if (inputType == InputType.Keyboard)
        {
            Vector2 moveDir = Vector2.zero;
            
            if (Input.GetKey(KeyCode.W))
            {
                moveDir += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDir += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDir += Vector2.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDir += Vector2.right;
            }
            return moveDir.normalized;
        }

        return Vector2.zero;
    }

    Vector2 controllerAimDir = Vector2.up;
    public Vector2 GetAimDirection ()
    {
        if (inputType == InputType.TableRealms)
        {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller)
        {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);

            if (d.RightStick.Value.magnitude > 0.1f) {
                controllerAimDir = d.RightStick.Value;
            }

            return controllerAimDir;
        }
        if (inputType == InputType.Keyboard)
        {
            if (Game.instance)
            {
                Vector3 playerPosition = Game.instance.GetPlayerActor(playerID).transform.position;
                
                // Get world position of mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, playerPosition);
                float distance = 0;
                Vector3 worldPosition = Vector3.zero;
                if (plane.Raycast(ray, out distance))
                {
                    worldPosition = ray.GetPoint(distance);
                }

                Vector3 delta = (worldPosition - playerPosition).normalized;

                // Return result in 2d space
                return new Vector2(delta.x, delta.z);                
            }
        }

        return Vector2.zero;
    }

    public bool GetAttack ()
    {
        if (inputType == InputType.TableRealms)
        {
            if (GetAimDirection().magnitude > 0.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (inputType == InputType.Controller) {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.RightTrigger.IsPressed;
        }

        if (inputType == InputType.Keyboard)
        {
            return Input.GetKey(KeyCode.Mouse0);
        }

        return false;
    }

    public bool GetInteract()
    {
        if (inputType == InputType.TableRealms)
        {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller)
        {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.Action4.IsPressed;
        }
        if (inputType == InputType.Keyboard)
        {
            if (Input.GetKey(KeyCode.E))
            {
                return true;
            }
        }

        return false;
    }

    bool interactPressedCache = false;
    public bool GetInteractPressed ()
    {
        bool result = false;
        if (interactPressedCache == false && GetInteract() == true)
        {
            result = true;
        }

        interactPressedCache = GetInteract();

        return result;
    }

    public bool GetUseAbility()
    {
        if (inputType == InputType.TableRealms)
        {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller)
        {
            InControl.InputDevice d = ControllerManger.instance.GetDevice(playerID);
            return d.Action1.IsPressed;
        }
        if (inputType == InputType.Keyboard)
        {
            if (Input.GetKey(KeyCode.R))
            {
                return true;
            }
        }

        return false;
    }

    bool useAbilityPressedCache = false;
    public bool GetUseAbilityPressed()
    {
        bool result = false;
        if (useAbilityPressedCache == false && GetUseAbility() == true)
        {
            result = true;
        }

        useAbilityPressedCache = GetUseAbility();

        return result;
    }
}
