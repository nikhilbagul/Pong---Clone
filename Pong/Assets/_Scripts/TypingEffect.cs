using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    public Text textToAnimate; // Reference to the TextMeshPro component
    public float typingSpeed = 0.1f; // Speed of typing in seconds

    private string fullText; // The complete text to be typed

    private void Start()
    {
        textToAnimate = this.gameObject.GetComponent<Text>();        
    }

    public void StartTypingTextVFX(UnityAction onCompleteCallback)
    {
        StartCoroutine(TypingTextVFX(onCompleteCallback));
    }

    // Coroutine to simulate typing effect
    IEnumerator TypingTextVFX(UnityAction onCompleteCallback)
    {
        fullText = textToAnimate.text; // Store the full text
        textToAnimate.text = string.Empty; // Clear the text

        foreach (char letter in fullText)
        {
            textToAnimate.text += letter; // Append each letter to the text
            yield return new WaitForSeconds(typingSpeed); // Wait for the specified duration
        }

        if (onCompleteCallback != null)
            onCompleteCallback();

    }
}