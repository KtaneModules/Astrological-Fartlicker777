using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class Astrological : MonoBehaviour {

    public KMBombInfo Bomb;
    public KMAudio Audio;
    public Renderer Background;

    float RandomXOffset;
    float RandomYOffset;
    float XOffset;
    float YOffset;

    //                  ->    xnor  and  or   xor   nand nor
    char[] Operators = {'⇒', '⊙', '&', '|', '⊕', '↑', '↓'};

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;
        /*
        foreach (KMSelectable object in keypad) {
            object.OnInteract += delegate () { keypadPress(object); return false; };
        }
        */

        //button.OnInteract += delegate () { buttonPress(); return false; };

    }

    void Start () {
      RandomXOffset = UnityEngine.Random.Range(0f, 1f);
      RandomYOffset = UnityEngine.Random.Range(0f, 1f);
      StartCoroutine(MovingSpace());
    }

    void Update () {

    }

    IEnumerator MovingSpace () {
      bool AddOrSubtract = false;
      if (UnityEngine.Random.Range(0, 2) == 1)
        AddOrSubtract = !AddOrSubtract;
      while (true) {
        if (AddOrSubtract) {
          XOffset += RandomXOffset / 100;
          YOffset += RandomYOffset / 100;
        }
        else {
          XOffset -= RandomXOffset / 100;
          YOffset -= RandomYOffset / 100;
        }
        Background.material.SetTextureOffset("_MainTex", new Vector2(XOffset, YOffset));
        yield return new WaitForSeconds(.01f);
      }
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"Use !{0} to do something.";
    #pragma warning restore 414

    IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
    }

    IEnumerator TwitchHandleForcedSolve () {
      yield return null;
    }
}
