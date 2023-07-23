using Messaging;
using UnityEngine;
using UnityEngine.UI;

public class PlayGroundController : MonoBehaviour
{
    [SerializeField] private Button _toastTextButton;

    private MessageBus _messageBus;

    void Awake()
    {
        _messageBus = MessageBus.Get();
        _toastTextButton.onClick.AddListener(ShowToast);
    }

    private void ShowToast()
    {
        _messageBus.Publish(new ToastMessage()
        {
            title = "Test Title",
            message = "Test toast message"
        });
    }
}
