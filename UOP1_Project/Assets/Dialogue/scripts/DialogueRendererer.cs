using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueRendererer : MonoBehaviour
{
    
  [SerializeField]  private TextMeshProUGUI Text;
 public int SentenceCount;
    private float letterdelay = 0.2f;
    private string displayed;
 
    void Start()
    {
       
           Text = gameObject.GetComponent<TextMeshProUGUI>();
    }

    
 public   IEnumerator NewChat(Conversation conversation)
    {
        if(!conversation.triggered_once || conversation.repeatable)
        {

            for (int i = 0; i < conversation.lines.Length; i++)
            {
                conversation.triggered_once = true;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = conversation.lines[SentenceCount].Speaking.name;
                if (conversation.lines[SentenceCount].NameOveride != null)
                {
                    this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = conversation.lines[SentenceCount].NameOveride;
                }

                gameObject.GetComponentInChildren<Image>().sprite = conversation.lines[SentenceCount].Speaking;
                Text.color = conversation.lines[SentenceCount].color;
                Text.fontStyle = (FontStyles)conversation.lines[SentenceCount].fontstyle;

                for (int t = 0; t < conversation.lines[SentenceCount].text.Length; t++)
                {
                  
                    displayed += conversation.lines[SentenceCount].text[t];
                    Text.text = displayed;
                    yield return new WaitForSeconds(letterdelay);
                }

                SentenceCount++;
                displayed = "";


            }
            Text.text = "";
            this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            SentenceCount = 0;
            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
