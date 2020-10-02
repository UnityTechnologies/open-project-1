using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UOP1.Dialogue;
using UOP1.Cutscene;
public class DialogueRendererer : MonoBehaviour
{
    public static DialogueRendererer dialogueRendererer_static;
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
        p.GetGenericBinding(o);
        dialogueRendererer_static = this;
           Text = gameObject.GetComponent<TextMeshProUGUI>();
        //  StartCoroutine(NewChat(conversation conversation));
        track = timelineasset.GetRootTrack(0);
        Debug.Log(track.duration);
    }

    
 public   IEnumerator NewChat(conversation conversation)
    {

        
        for (int i = 0; i < conversation.lines.Length ; i++)
        {
          
            Text.color = conversation.lines[sentence].color;
            Text.fontStyle = (FontStyles)conversation.lines[sentence].fontstyle;
      
            for (int t = 0; t < conversation.lines[sentence].text.Length ; t++)
            {
                Debug.Log(t);
                displayed += conversation.lines[sentence].text[t];
                Text.text = displayed;
                yield return new WaitForSeconds(letterdelay);
            }
            sentence++;
            displayed = "";
        }
    }

}
