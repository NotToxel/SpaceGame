// Inspired by Brackeys on YouTube
// https://www.youtube.com/watch?v=_nRzoTzeyxU&t=186s

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}
