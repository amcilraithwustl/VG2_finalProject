using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject menu;
    //public GameObject scoreboard;
    public InputActionProperty showButton;
    public Transform head;
    public float spawnDistance = 2;
    public float lagFactor = .5f;
    private void Update()
    {
        if(showButton.action.WasPressedThisFrame())
        {
            print("SHOWING MENU");
            menu.SetActive(!menu.activeSelf);
        }

        var newPosition = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        menu.transform.position = Vector3.Lerp(menu.transform.position, newPosition, lagFactor * Time.deltaTime);
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;
    }

    
    public void StartGame()
    {
        SceneManager.LoadScene("MechanicsFix");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
