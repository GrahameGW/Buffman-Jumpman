using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player[] players = new Player[2];
    private Player activePlayer;
    private bool controlActive = true;

    public void AddPlayer(Player player, int index)
    {
        if (index == 0 || index == 1) {
            players[index] = player;
            if (activePlayer == null) {
                activePlayer = player;
                player.Activate();
            }
        }
    }

    public void RemovePlayer(Player player)
    {
        for (int i = 0; i < players.Length; i++) {
            if (player.Equals(players[i])) {
                if (activePlayer = players[i]) SwitchActivePlayer();
                players[i] = null;
            }
        }
    }

    private void Update()
    {
        controlActive = GameManager.Instance.Playing && !GameManager.Instance.GameWon;

        if (activePlayer != null && controlActive) {

            if (Input.GetKeyDown(KeyCode.Tab)) { // get next player
                SwitchActivePlayer();
            }

            // Move Left/Right
            activePlayer.Move(Input.GetAxis("Horizontal"));

            // Climb
            activePlayer.Climb(Input.GetAxis("Vertical"));

            if (Input.GetButtonDown("Jump")) {
                activePlayer.MainAction();
            }

            if (Input.GetButtonDown("Fire1")) {
                activePlayer.SecondAction();
            }
            /*
            if (Input.GetKeyDown(KeyCode.E)) {
                activePlayer.InteractAction();
            }
            */
        }
    }

    private void SwitchActivePlayer()
    {
        activePlayer.Deactivate();

        if (activePlayer == players[players.Length - 1]) activePlayer = players[0];
        else activePlayer = players[Array.IndexOf(players, activePlayer) + 1];

        activePlayer.Activate();
    }
}
