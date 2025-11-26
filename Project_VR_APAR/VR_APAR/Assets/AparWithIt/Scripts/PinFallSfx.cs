using UnityEngine;

public class PinFallSfx : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private bool _audioClipPlayed;
    private void OnCollisionEnter()
    {
        if(_audioClipPlayed) return;
        
        audioSource.Play();
        _audioClipPlayed = true;
    }
}
