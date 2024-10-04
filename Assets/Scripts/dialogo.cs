using System.Collections;
using UnityEngine;
using TMPro;

public class dialogo : MonoBehaviour
{
    [SerializeField, TextArea(4, 6)] private string[] LineaDialogo;
    [SerializeField] private GameObject PanelDialogo;
    [SerializeField] private TMP_Text Dialogo;

    private float TypingTime = 0.05f;

    private bool IsPlayerInRange;
    private bool didDialogoStart;
    private int LineIndex;
    
    void Update()
    {
       if (IsPlayerInRange && Input.GetButtonDown("Fire1"))
       {
            if (!didDialogoStart)
            {
                StartDialogo();
            }
            else if (Dialogo.text == LineaDialogo[LineIndex])
            {
                NextLineaDialogo();

            }
            else
            {
                StopAllCoroutines();
                Dialogo.text = LineaDialogo[LineIndex];
            }
       }
    }

    private void StartDialogo()
    {
        didDialogoStart = true;
        PanelDialogo.SetActive(true);
        LineIndex = 0;
        Time.timeScale = 0f;
        StartCoroutine(ShowLine());
    }

    private void NextLineaDialogo()
    {
        LineIndex++;
        if (LineIndex < LineaDialogo.Length)
        {
            StartCoroutine(ShowLine());

        }
        else
        {
            didDialogoStart = false;
            PanelDialogo.SetActive(false);
            Time.timeScale = 1f;

        }

    }

    private IEnumerator ShowLine()
    {
        Dialogo.text = string.Empty;

        foreach (char ch in LineaDialogo[LineIndex])
        {
            Dialogo.text += ch;
            yield return new WaitForSecondsRealtime(TypingTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        { 
            IsPlayerInRange = false; 
        }
    }
}
