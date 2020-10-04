using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UOP1.Dialogue;
using UOP1.Cutscene;
using UnityEngine.UI;

public class DialogueRendererer : MonoBehaviour
{
    
  [SerializeField]  private TextMeshProUGUI Text;
  [SerializeField] string[] sentences;
  [SerializeField] int sentence;
    private float letterdelay = 0.2f;
    private string displayed;
    
   public string color_string;
     public TimelineAsset timelineasset;
    public TrackAsset track;
    public PlayableDirector p;
    public Object o;
    void Start()
    {
     
    
           Text = gameObject.GetComponent<TextMeshProUGUI>();
        //  StartCoroutine(NewChat(conversation conversation));
   
    }

    
 public   IEnumerator NewChat(conversation conversation)
    {
        if(!conversation.triggered_once ){

            for (int i = 0; i < conversation.lines.Length; i++)
            {
                conversation.triggered_once = true;
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
                this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = conversation.lines[sentence].Speaking.name;
                if (conversation.lines[sentence].NameOveride != null)
                {
                    this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = conversation.lines[sentence].NameOveride;
                }

                gameObject.GetComponentInChildren<Image>().sprite = conversation.lines[sentence].Speaking;
                Text.color = conversation.lines[sentence].color;
                Text.fontStyle = (FontStyles)conversation.lines[sentence].fontstyle;

                for (int t = 0; t < conversation.lines[sentence].text.Length; t++)
                {
                    //   Debug.Log(t);
                    displayed += conversation.lines[sentence].text[t];
                    Text.text = displayed;
                    yield return new WaitForSeconds(letterdelay);
                }

                sentence++;
                displayed = "";


            }
            Text.text = "";
            this.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}
