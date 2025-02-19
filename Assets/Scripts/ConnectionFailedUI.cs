using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConnectionFailedUI : NetworkBehaviour
{
    private TextMeshProUGUI messageText;
    
    private void Awake()
    {
        Button okButton = transform.Find("Ok").gameObject.GetComponent<Button>();

        okButton.onClick.AddListener(OkButtonPressed);

        messageText = transform.Find("Message").gameObject.GetComponent<TextMeshProUGUI>();
        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if (NetworkManager.Singleton.DisconnectReason == "") {
            messageText.text = "Failed to connect";
        }
    }

    private void OkButtonPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
}