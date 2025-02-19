using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaveMenu : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button yesButton = transform.Find("Yes").gameObject.GetComponent<Button>();
        Button noButton = transform.Find("No").gameObject.GetComponent<Button>();

        yesButton.onClick.AddListener(YesButtonPressed);
        noButton.onClick.AddListener(NoButtonPressed);
    }

    private void YesButtonPressed()
    {
        if (NetworkManager.Singleton.IsServer) {
            NetworkManager.Singleton.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else {
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void NoButtonPressed()
    {
        Destroy(gameObject);
    }

    [ClientRpc]
    private void QuitClientRpc()
    {
        NetworkManager.Singleton.Shutdown();
        Destroy(NetworkManager.Singleton.gameObject);
    }
}
