using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "Scriptable Objects/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    public float jumpHeight;
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float crouchHeight;
    public float standHeight;
    public float slideLenght;

}
