using Messaging;
using Messaging.Interfaces;
using UnityEngine;

public class ToastController : MonoBehaviour
    ,IToastController
    ,ISubscriber<ToastMessage>
{
    [SerializeField] private ToastView _toastView;

    private void Awake()
        => MessageBus.Get().Subscribe(this);

    public void OnMessage(ToastMessage message)
        => _toastView.Show(message.title, message.message);
}
