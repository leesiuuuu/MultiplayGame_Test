using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    public PhotonView photonView;
    public GameObject BubbleSpeechobject;
    public Text updatedText;

    private InputField ChatInputField;
    private bool DisableSend;

    private void Awake()
    {
        ChatInputField = GameObject.Find("ChatInputField").GetComponent<InputField>();
        ChatInputField.lineType = InputField.LineType.SingleLine; // 엔터키가 줄바꿈을 하지 않도록 설정
        ChatInputField.onEndEdit.AddListener(HandleEndEdit); // onEndEdit 이벤트에 리스너 추가
    }

    private void HandleEndEdit(string text)
    {
        if (!DisableSend && photonView.isMine && !string.IsNullOrEmpty(text))
        {
            photonView.RPC("SendMessage", PhotonTargets.AllBuffered, text);
            BubbleSpeechobject.SetActive(true);

            ChatInputField.text = "";
            DisableSend = true;

            // 포커스 해제
            ChatInputField.DeactivateInputField();
        }
    }

    [PunRPC]
    private void SendMessage(string message)
    {
        updatedText.text = message;

        StartCoroutine("Remove");
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(4f);
        BubbleSpeechobject.SetActive(false);
        DisableSend = false;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(BubbleSpeechobject.activeSelf);
        }
        else if (stream.isReading)
        {
            BubbleSpeechobject.SetActive((bool)stream.ReceiveNext());
        }
    }
}
