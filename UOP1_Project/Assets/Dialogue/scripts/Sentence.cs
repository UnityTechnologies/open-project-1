using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Sentence 
{
    public string text;
    public Color color;
    public FontStyle fontstyle;
    public Sprite Speaking;
  [Tooltip("dont enter anything to use the sprite name")]  public string NameOveride;
}
