using UnityEngine;

public class PlayerRole : MonoBehaviour
{
    // Creates 2 roles for the player, Rabbit and Fox
    public enum Role { Rabbit, Fox }

    // Sets the player current role to Rabbit initially
    public Role role = Role.Rabbit;
}
