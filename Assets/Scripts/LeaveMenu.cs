using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaveMenu : NetworkBehaviour
{
    private NetworkManager networkManager;

    // Start is called before the first frame update
    void Start()
    {
        Button yesButton = transform.Find("Yes").gameObject.GetComponent<Button>();
        Button noButton = transform.Find("No").gameObject.GetComponent<Button>();

        yesButton.onClick.AddListener(YesButtonPressed);
        noButton.onClick.AddListener(NoButtonPressed);

        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<NetworkManager>();
    }

    private void YesButtonPressed()
    {
        if (networkManager.IsHost) {
            networkManager.SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        else {
            networkManager.Shutdown();
            Destroy(networkManager.gameObject);
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
        networkManager.Shutdown();
        Destroy(networkManager.gameObject);
    }
}
