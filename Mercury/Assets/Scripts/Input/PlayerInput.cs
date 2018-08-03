using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput {
    // A player input is a mapping between a player and a device.
    // The device can be a TableRealms device or a controller.

    public enum InputType {TableRealms, Controller, Keyboard}

    public InputType inputType = InputType.TableRealms;
    public string playerID = "-1";

    public PlayerInput(string playerID, InputType inputType) {
        this.playerID = playerID;
        this.inputType = inputType;
    }

    // ** Methods :
    public Vector2 GetMoveDirection () {
        if (inputType == InputType.TableRealms) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Keyboard) {

            Vector2 moveDir = Vector2.zero;
            
            if (Input.GetKey(KeyCode.W)) {
                moveDir += Vector2.up;
            }
            if (Input.GetKey(KeyCode.S)) {
                moveDir += Vector2.down;
            }
            if (Input.GetKey(KeyCode.A)) {
                moveDir += Vector2.left;
            }
            if (Input.GetKey(KeyCode.D)) {
                moveDir += Vector2.right;
            }

            return moveDir.normalized;
        }

        return Vector2.zero;
    }

    public KeyCode getInteractionKey()
    {
        if (Input.GetKey(KeyCode.F12))
        {
            return KeyCode.F12;
        }

        if (Input.GetKey(KeyCode.E))
        {
            return KeyCode.E;
        }

        if (Input.GetKey(KeyCode.F11))
        {
            return KeyCode.F11;
        }
        return KeyCode.None;
    }

    public Vector2 GetAimDirection () {
        if (inputType == InputType.TableRealms) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Controller) {
            Debug.LogError("Not implemented");
        }
        if (inputType == InputType.Keyboard) {
            if (Game.instance) {
                Vector3 playerPosition = Game.instance.GetPlayerActor(playerID).transform.position;
                
                // Get world position of mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, playerPosition);
                float distance = 0;
                Vector3 worldPosition = Vector3.zero;
                if (plane.Raycast(ray, out distance)) {
                    worldPosition = ray.GetPoint(distance);
                }

                Vector3 delta = (worldPosition - playerPosition).normalized;

                // Return result in 2d space
                return new Vector2(delta.x, delta.z);                
            }
        }

        return Vector2.zero;
    }

    public bool GetAttack () {
        if (inputType == InputType.Controller || inputType == InputType.TableRealms) {
            if (GetAimDirection().magnitude > 0.5f) {
                return true;
            }
            else {
                return false;
            }
        }
        if (inputType == InputType.Keyboard) {
            return Input.GetKey(KeyCode.Mouse0);
        }

        return false;
    }
}
