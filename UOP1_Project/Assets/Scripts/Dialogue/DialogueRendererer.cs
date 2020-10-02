using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DialogueRendererer : MonoBehaviour
{
  [SerializeField]  private TextMeshProUGUI Text;
  [SerializeField] string[] sentences;
  [SerializeField] int sentence;
    private float letterdelay = 0.2f;
    private string displayed;
    private Color color;
   public string color_string;
    // Start is called before the first frame update
    void Start()
    {
        Text = gameObject.GetComponent<TextMeshProUGUI>();
        StartCoroutine(NewChat("Assets/Dialogue/1.csv"));
    }

    // Update is called once per frame
    void Update()
    {
        //  Text.text = System.IO.File.ReadAllText("Assets/Dialogue/1.csv");
        //  chat = System.IO.File.ReadAllText("Assets/Dialogue/1.csv").Split("/"[0]);
  

    }
    IEnumerator NewChat(string directory)
    {
    
        sentences = System.IO.File.ReadAllText(directory).Split("."[0]);
        for (int i = 0; i < sentences.Length ; i++)
        {
            findtag(sentences[sentence]);
          ColorUtility.TryParseHtmlString(color_string.TrimStart('#'), out color);
            Text.color = color;
        
            for (int t = 0; t < sentences[sentence].Replace(color_string, "").Length ; t++)
            {
                Debug.Log(t);
                displayed += sentences[sentence].Replace(color_string, "")[t];
                Text.text = displayed;
                yield return new WaitForSeconds(letterdelay);
            }
            sentence++;
            displayed = "";





        }
    }
    void findtag(string sentence)
    {
        for (int i = 0; i < sentence.Length; i++)
        {
 if(sentence[i].ToString() == "#")
            {
                while (sentence[i].ToString() != " ")
                {
                    color_string += sentence[i].ToString();
                    i++;
                }

            }
        }
    }
}
