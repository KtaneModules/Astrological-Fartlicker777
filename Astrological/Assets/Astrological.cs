using System.Collections;
using System.Linq;
using UnityEngine;
using KModkit;

public class Astrological : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;
   public Renderer Background;
   public TextMesh[] OperatorSymbolsTM;
   public SpriteRenderer[] AstrologySymbols;
   public Sprite[] Constellations;
   public GameObject[] CheckAndX;
   public KMSelectable[] Buttons;

   int[] AstrologySigns = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
   int Batteries, BatteryHolders, Portplates, TwoFactor, ParallelPort, SerialPort, DVIPort, RCAPort, PS2Port, RJPort, Vowels, Consonants, Ports;
   int jon = -1000000; //jon
   int LogCheck = 0;
   int Stage = 0;

   float Hue = 0f;
   float Opacity = 1f;
   float RandomXOffset;
   float RandomYOffset;
   float Saturation = .2f;
   float Value = .059f;
   float XOffset;
   float YOffset;

   string[] HoroscopeNames = { "Aries", "Taurus", "Gemini", "Cancer", "Leo", "Virgo", "Libra", "Scorpio", "Saggitarius", "Capricorn", "Aquarius", "Pisces" };
   string Indicators;

   bool[] Validity = new bool[12];
   bool[] ValiditySecondFloor = new bool[12];
   bool[] ValidityThirdFloor = new bool[12];
   bool[] CheckPresses = new bool[3];
   bool AddOrSubtract = false;
   bool HitCheck = true;
   bool HasLoggedStage2;

   //                     ->    xnor  and  or   xor   nand nor
   char[] Operators = {/*'⇒', */'⊙', '&', '|', '⊕', '↑', '↓' };

   static int moduleIdCounter = 1;
   int moduleId;
   private bool moduleSolved;

   void Awake () {
      moduleId = moduleIdCounter++;

      foreach (KMSelectable Button in Buttons) {
         Button.OnInteract += delegate () { ButtonPress(Button); return false; };
      }

   }

   void Start () {
      RandomXOffset = UnityEngine.Random.Range(0f, 1f);
      RandomYOffset = UnityEngine.Random.Range(0f, 1f);
      if (UnityEngine.Random.Range(0, 2) == 1) {
         AddOrSubtract = !AddOrSubtract;
      }
      StartCoroutine(MovingSpace());

      Batteries = Bomb.GetBatteryCount();
      BatteryHolders = Bomb.GetBatteryHolderCount();
      Portplates = Bomb.GetPortPlateCount();
      TwoFactor = Bomb.GetTwoFactorCounts();
      ParallelPort = Bomb.GetPortCount(Port.Parallel);
      SerialPort = Bomb.GetPortCount(Port.Serial);
      DVIPort = Bomb.GetPortCount(Port.DVI);
      RCAPort = Bomb.GetPortCount(Port.StereoRCA);
      PS2Port = Bomb.GetPortCount(Port.PS2);
      RJPort = Bomb.GetPortCount(Port.RJ45);
      Ports = Bomb.GetPortCount();
      for (int i = 0; i < 6; i++) {
         if ("AEIOU".Contains(Bomb.GetSerialNumber()[i])) {
            Vowels++;
         }
         if ("BCDFGHJKLMNPQRSTVWXYZ".Contains(Bomb.GetSerialNumber()[i])) {
            Consonants++;
         }
      }
      Indicators = Bomb.GetIndicators().Join("");

      System.DateTime curTime = System.DateTime.Now;
      switch (curTime.Month) {
         case 3:
            if (curTime.Day < 21) {
               Validity[11] = !Validity[11];
               Validity[0] = !Validity[0];

               if (ParallelPort > 0 || PS2Port > 0 || SerialPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[9] = !Validity[9];
                  Validity[7] = !Validity[7];
               }
            }
            else {
               Validity[0] = !Validity[0];
               Validity[4] = !Validity[4];

               if (Indicators.Any(x => "ARIES".Contains(x))) {
                  Validity[2] = !Validity[2];
               }
               if (Bomb.GetSerialNumber().Any(x => "ARIES".Contains(x))) {
                  Validity[11] = !Validity[11];
               }
            }
            break;

         case 4:
            if (curTime.Day < 20) {
               Validity[0] = !Validity[0];
               Validity[4] = !Validity[4];

               if (Indicators.Any(x => "ARIES".Contains(x))) {
                  Validity[2] = !Validity[2];
               }
               if (Bomb.GetSerialNumber().Any(x => "ARIES".Contains(x))) {
                  Validity[11] = !Validity[11];
               }
            }
            else {
               Validity[1] = !Validity[1];
               Validity[5] = !Validity[5];

               if (Bomb.GetSerialNumber().Any(x => "TAURUS".Contains(x))) {
                  Validity[3] = !Validity[3];
                  Validity[8] = !Validity[8];
               }
            }
            break;

         case 5:
            if (curTime.Day < 21) {
               Validity[1] = !Validity[1];
               Validity[5] = !Validity[5];

               if (Bomb.GetSerialNumber().Any(x => "TAURUS".Contains(x))) {
                  Validity[3] = !Validity[3];
                  Validity[8] = !Validity[8];
               }
            }
            else {
               Validity[2] = !Validity[2];
               Validity[6] = !Validity[6];

               if (Bomb.GetSerialNumber().Any(x => "GEMINI".Contains(x))) {
                  Validity[9] = !Validity[9];
               }
               if (Indicators.Any(x => "GEMINI".Contains(x))) {
                  Validity[0] = !Validity[0];
               }
            }
            break;

         case 6:
            if (curTime.Day < 21) {
               Validity[2] = !Validity[2];
               Validity[6] = !Validity[6];

               if (Bomb.GetSerialNumber().Any(x => "GEMINI".Contains(x))) {
                  Validity[9] = !Validity[9];
               }
               if (Indicators.Any(x => "GEMINI".Contains(x))) {
                  Validity[0] = !Validity[0];
               }
            }
            else {
               Validity[3] = !Validity[3];
               Validity[7] = !Validity[7];

               if (Bomb.GetSerialNumber().Any(x => "CANCER".Contains(x))) {
                  Validity[1] = !Validity[1];
                  Validity[10] = !Validity[10];
               }
            }
            break;

         case 7:
            if (curTime.Day < 23) {
               Validity[3] = !Validity[3];
               Validity[7] = !Validity[7];

               if (Bomb.GetSerialNumber().Any(x => "CANCER".Contains(x))) {
                  Validity[1] = !Validity[1];
                  Validity[10] = !Validity[10];
               }
            }
            else {
               Validity[4] = !Validity[4];
               Validity[8] = !Validity[8];

               if (ParallelPort > 0 || SerialPort > 0 || RCAPort > 0) {
                  Validity[6] = !Validity[6];
               }
               if (Indicators.Any(x => "LEO".Contains(x))) {
                  Validity[0] = !Validity[0];
               }
            }
            break;

         case 8:
            if (curTime.Day < 23) {
               Validity[4] = !Validity[4];
               Validity[8] = !Validity[8];

               if (ParallelPort > 0 || SerialPort > 0 || RCAPort > 0) {
                  Validity[6] = !Validity[6];
               }
               if (Indicators.Any(x => "LEO".Contains(x))) {
                  Validity[0] = !Validity[0];
               }
            }
            else {
               Validity[5] = !Validity[5];
               Validity[9] = !Validity[9];

               if (Bomb.GetSerialNumber().Any(x => "VIRGO".Contains(x))) {
                  Validity[7] = !Validity[7];
               }
               if (Indicators.Any(x => "VIRGO".Contains(x))) {
                  Validity[1] = !Validity[1];
               }
            }
            break;

         case 9:
            if (curTime.Day < 23) {
               Validity[5] = !Validity[5];
               Validity[9] = !Validity[9];

               if (Bomb.GetSerialNumber().Any(x => "VIRGO".Contains(x))) {
                  Validity[7] = !Validity[7];
               }
               if (Indicators.Any(x => "VIRGO".Contains(x))) {
                  Validity[1] = !Validity[1];
               }
            }
            else {
               Validity[6] = !Validity[6];
               Validity[10] = !Validity[10];

               if (ParallelPort > 0 || SerialPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[4] = !Validity[4];
               }
               if (Indicators.Any(x => "LIBRA".Contains(x))) {
                  Validity[2] = !Validity[2];
               }
            }
            break;

         case 10:
            if (curTime.Day < 23) {
               Validity[6] = !Validity[6];
               Validity[10] = !Validity[10];

               if (ParallelPort > 0 || SerialPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[4] = !Validity[4];
               }
               if (Indicators.Any(x => "LIBRA".Contains(x))) {
                  Validity[2] = !Validity[2];
               }
            }
            else {
               Validity[7] = !Validity[7];
               Validity[11] = !Validity[11];

               if (Bomb.GetSerialNumber().Any(x => "SCORPIO".Contains(x))) {
                  Validity[5] = !Validity[5];
               }
               if (Indicators.Any(x => "SCORPIO".Contains(x))) {
                  Validity[3] = !Validity[3];
               }
            }
            break;

         case 11:
            if (curTime.Day < 22) {
               Validity[7] = !Validity[7];
               Validity[11] = !Validity[11];

               if (Bomb.GetSerialNumber().Any(x => "SCORPIO".Contains(x))) {
                  Validity[5] = !Validity[5];
               }
               if (Indicators.Any(x => "SCORPIO".Contains(x))) {
                  Validity[3] = !Validity[3];
               }
            }
            else {
               Validity[8] = !Validity[8];
               Validity[1] = !Validity[1];

               if (ParallelPort > 0 || SerialPort > 0 || PS2Port > 0 || RJPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[4] = !Validity[4];
               }
               if (Indicators.Any(x => "SAGGITARIUS".Contains(x))) {
                  Validity[10] = !Validity[10];
               }
            }
            break;

         case 12:
            if (curTime.Day < 22) {
               Validity[8] = !Validity[8];
               Validity[1] = !Validity[1];

               if (ParallelPort > 0 || SerialPort > 0 || PS2Port > 0 || RJPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[4] = !Validity[4];
               }
               if (Indicators.Any(x => "SAGGITARIUS".Contains(x))) {
                  Validity[10] = !Validity[10];
               }
            }
            else {
               Validity[9] = !Validity[9];
               Validity[2] = !Validity[2];

               if (ParallelPort > 0 || SerialPort > 0 || RJPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[11] = !Validity[11];
                  Validity[5] = !Validity[5];
               }
            }
            break;

         case 1:
            if (curTime.Day < 20) {
               Validity[9] = !Validity[9];
               Validity[2] = !Validity[2];

               if (ParallelPort > 0 || SerialPort > 0 || RJPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[11] = !Validity[11];
                  Validity[5] = !Validity[5];
               }
            }
            else {
               Validity[10] = !Validity[10];
               Validity[3] = !Validity[3];

               if (ParallelPort > 0 || SerialPort > 0 || PS2Port > 0 || RJPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[6] = !Validity[6];
               }
               if ("AQUARIUS".Any(x => Indicators.Contains(x))) {
                  Validity[8] = !Validity[8];
               }
            }
            break;

         case 2:
            if (curTime.Day < 19) {
               Validity[10] = !Validity[10];
               Validity[3] = !Validity[3];

               if (ParallelPort > 0 || SerialPort > 0 || PS2Port > 0 || RJPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[6] = !Validity[6];
               }
               if ("AQUARIUS".Any(x => Indicators.Contains(x))) {
                  Validity[8] = !Validity[8];
               }
            }
            else {
               Validity[11] = !Validity[11];
               Validity[0] = !Validity[0];

               if (ParallelPort > 0 || PS2Port > 0 || SerialPort > 0 || RCAPort > 0 || DVIPort > 0) {
                  Validity[9] = !Validity[9];
                  Validity[7] = !Validity[7];
               }
            }
            break;
      }

      if (BatteryHolders * 2 == Batteries && Batteries != 0) {
         Validity[0] = !Validity[0];
      }
      if (TwoFactor > 0) {
         Validity[1] = !Validity[1];
      }

      int GeminiCount = 0;
      if (Bomb.GetSerialNumber().Any(x => "2".Contains(x))) {
         GeminiCount++;
      }
      if (Batteries != BatteryHolders) {
         GeminiCount++;
      }
      if (curTime.Day % 2 == 0) {
         GeminiCount++;
      }
      if (DVIPort > 1 || PS2Port > 1 || ParallelPort > 1 || SerialPort > 1 || RJPort > 1 || RCAPort > 1) {
         GeminiCount++;
      }

      if (GeminiCount == 2) {
         Validity[2] = !Validity[2];
      }
      if (Bomb.GetSolvableModuleNames().Contains("Dr. Doctor") || eXish()) {
         Validity[3] = !Validity[3];
      }
      if (RJPort != 0) {
         Validity[4] = !Validity[4];
      }
      if (Batteries == BatteryHolders && Batteries != 0) {
         Validity[5] = !Validity[5];
      }
      if (Vowels > Consonants) {
         Validity[6] = !Validity[6];
      }
      if (Bomb.GetSerialNumber().Any(x => "POISON".Contains(x))) {
         Validity[7] = !Validity[7];
      }
      if (Portplates != 0) {
         if (Ports % Portplates == 0) {
            Validity[8] = !Validity[8];
         }
      }
      if (SerialPort > 0) {
         Validity[9] = !Validity[9];
      }
      if (Vowels > 0) {
         Validity[10] = !Validity[10];
      }
      if (Batteries == 0) {
         Validity[11] = !Validity[11];
      }

      Debug.LogFormat("[Astrological #{0}] The following horoscopes are valid in stage 1: ", moduleId);
      for (int i = 0; i < 12; i++) {
         if (Validity[i]) {
            LogCheck++;
         }
      }
      if (LogCheck == 0) {
         Debug.LogFormat("[Astrological #{0}] Apparently, there are no valid horoscopes. ", moduleId);
      }
      else if (LogCheck == 12) {
         Debug.LogFormat("[Astrological #{0}] Apparently, everything is valid. ", moduleId);
      }
      else {
         for (int i = 0; i < 12; i++) {
            if (Validity[i]) {
               Debug.LogFormat("[Astrological #{0}] {1}", moduleId, HoroscopeNames[i]);
            }
         }
      }

      for (int i = 0; i < 12; i++) {
         ValiditySecondFloor[i] = !Validity[i];
      }
      Randomize();
   }

   void Randomize () {
      AstrologySigns.Shuffle();
      Operators.Shuffle();
      OperatorSymbolsTM[0].text = Operators[0].ToString();
      OperatorSymbolsTM[1].text = Operators[1].ToString();
      for (int i = 0; i < 3; i++) {
         AstrologySymbols[i].GetComponent<SpriteRenderer>().sprite = Constellations[AstrologySigns[i]];
      }
      bool CheckRight = false;

      if (UnityEngine.Random.Range(0, 2) == 1) {
         CheckRight = true;
      }

      if (CheckRight) {
         CheckAndX[0].transform.localPosition = new Vector3(0.03f, 0.0175f, -0.0518f);
         CheckAndX[1].transform.localPosition = new Vector3(-0.03f, 0.0175f, -0.0518f);
      }
      else {
         CheckAndX[0].transform.localPosition = new Vector3(-0.03f, 0.0175f, -0.0518f);
         CheckAndX[1].transform.localPosition = new Vector3(0.03f, 0.0175f, -0.0518f);
      }

      if (CheckRight) {
         Debug.LogFormat("[Astrological #{0}] The current expression is {1} {2} ({3} {4} {5})", moduleId, HoroscopeNames[AstrologySigns[0]], Operators[0], HoroscopeNames[AstrologySigns[1]], Operators[1], HoroscopeNames[AstrologySigns[2]]);
      }
      else {
         Debug.LogFormat("[Astrological #{0}] The current expression is ({1} {2} {3}) {4} {5}", moduleId, HoroscopeNames[AstrologySigns[0]], Operators[0], HoroscopeNames[AstrologySigns[1]], Operators[1], HoroscopeNames[AstrologySigns[2]]);
      }

      HitCheck = CheckRight ? AnswerGenerator(1, 2, 0, 1, Stage) : AnswerGenerator(0, 1, 2, 0, Stage);

      if (HitCheck) {
         Debug.LogFormat("[Astrological #{0}] This is valid, hit the green button.", moduleId);
      }
      else {
         Debug.LogFormat("[Astrological #{0}] This in invalid, hit the red button.", moduleId);
      }
   }

   void ButtonPress (KMSelectable Button) {
      Button.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Button.transform);
      if (moduleSolved) {
         return;
      }
      if (Button == Buttons[0]) {
         if (HitCheck) {
            CheckPresses[Stage] = true;
            Stage++;
         }
         else {
            GetComponent<KMBombModule>().HandleStrike();
            Debug.LogFormat("[Astrological #{0}] You have pressed the invalid button, resetting the mod.", moduleId);
            Debug.LogFormat("[Astrological #{0}] -------------------------------------------------------", moduleId);
            StartCoroutine(Strike());
         }
      }
      else {
         if (!HitCheck) {
            CheckPresses[Stage] = false;
            Stage++;
         }
         else {
            GetComponent<KMBombModule>().HandleStrike();
            StartCoroutine(Strike());
         }
      }
      if (Stage == 3) {
         GetComponent<KMBombModule>().HandlePass();
         moduleSolved = true;
         Audio.PlaySoundAtTransform("Shutdown", this.transform);
         StartCoroutine(BackgroundColorChanger());
         StartCoroutine(SpriteHider());
         //OperatorSymbolsTM[0].text = String.Empty;
         //OperatorSymbolsTM[1].text = String.Empty;
         //for (int i = 0; i < 3; i++)
         //AstrologySymbols[i].gameObject.SetActive(false);
      }
      else {
         Randomize();
      }
   }

   IEnumerator BackgroundColorChanger () {
      while (true) {
         for (int i = 0; i < 256; i++) {
            Value -= 0.0027f;
            Background.material.color = Color.HSVToRGB(Hue, Saturation, Value);
            yield return new WaitForSeconds(0.1f);
         }
      }
   }

   IEnumerator SpriteHider () {
      while (true) {
         for (int i = 0; i < 3; i++) {
            AstrologySymbols[i].color = new Color(0, 0, 0, Opacity);
         }
         OperatorSymbolsTM[0].color = new Color(0, 0, 0, Opacity);
         OperatorSymbolsTM[1].color = new Color(0, 0, 0, Opacity);
         Opacity -= .01f;
         yield return new WaitForSeconds(0.01f);
      }
   }

   IEnumerator Strike () {
      Stage = 0;
      float TempHue = 1f;
      float TempSat = 1f;
      float TempVal = 1f;
      Background.material.color = Color.HSVToRGB(TempHue, TempSat, TempVal);
      while (TempHue > 0) {
         Background.material.color = Color.HSVToRGB(TempHue, TempSat, TempVal);
         TempHue -= .01f;
         TempSat -= .01f;
         TempVal -= .01f;
         yield return new WaitForSeconds(.1f);
      }
      Background.material.color = Color.HSVToRGB(Hue, Saturation, Value);
   }

   bool AnswerGenerator (int x, int y, int z, int w, int StageNumber) {
      //                                 ->    xnor  and  or   xor   nand nor
      bool Temp = false; //char[] Operators = {'⇒', '⊙', '&', '|', '⊕', '↑', '↓'};
      bool SecondTemp = true;
      if (StageNumber == 1) {
         goto Stage2;
      }
      else if (StageNumber == 2) {
         goto Stage3;
      }
      switch (Operators[w]) {
         //case '⇒':
         //  if (Validity[AstrologySigns[x]] && !Validity[AstrologySigns[y]])
         //  Temp = false;
         //break;
         case '⊙':
            Temp = !(Validity[AstrologySigns[x]] ^ Validity[AstrologySigns[y]]);
            break;
         case '&':
            Temp = Validity[AstrologySigns[x]] & Validity[AstrologySigns[y]];
            break;
         case '|':
            Temp = Validity[AstrologySigns[x]] | Validity[AstrologySigns[y]];
            break;
         case '⊕':
            Temp = Validity[AstrologySigns[x]] ^ Validity[AstrologySigns[y]];
            break;
         case '↑':
            Temp = !(Validity[AstrologySigns[x]] & Validity[AstrologySigns[y]]);
            break;
         case '↓':
            Temp = !(Validity[AstrologySigns[x]] | Validity[AstrologySigns[y]]);
            break;
      }
      switch (Operators[1 - w]) {
         //case '⇒':
         //  if (Validity[AstrologySigns[x]] && !Validity[AstrologySigns[y]])
         //  Temp = false;
         //break;
         case '⊙':
            SecondTemp = !(Temp ^ Validity[AstrologySigns[z]]);
            break;
         case '&':
            SecondTemp = Temp & Validity[AstrologySigns[z]];
            break;
         case '|':
            SecondTemp = Temp | Validity[AstrologySigns[z]];
            break;
         case '⊕':
            SecondTemp = Temp ^ Validity[AstrologySigns[z]];
            break;
         case '↑':
            SecondTemp = !(Temp & Validity[AstrologySigns[z]]);
            break;
         case '↓':
            SecondTemp = !(Temp | Validity[AstrologySigns[z]]);
            break;
      }
      return SecondTemp;

      Stage2:
      if (!HasLoggedStage2) {
         LogCheck = 0;
         Debug.LogFormat("[Astrological #{0}] The following horoscopes are valid in stage 2: ", moduleId);
         for (int i = 0; i < 12; i++) {
            if (!Validity[i]) {
               LogCheck++;
            }
         }
         if (LogCheck == 0) {
            Debug.LogFormat("[Astrological #{0}] Apparently, there are no valid horoscopes. ", moduleId);
         }
         else if (LogCheck == 12) {
            Debug.LogFormat("[Astrological #{0}] Apparently, everything is valid. ", moduleId);
         }
         else {
            for (int i = 0; i < 12; i++) {
               if (!Validity[i]) {
                  Debug.LogFormat("[Astrological #{0}] {1}", moduleId, HoroscopeNames[i]);
               }
            }
         }
         HasLoggedStage2 = true;
      }

      switch (Operators[w]) {
         //case '⇒':
         //  if (Validity[AstrologySigns[x]] && !Validity[AstrologySigns[y]])
         //  Temp = false;
         //break;
         case '⊙':
            Temp = !(ValiditySecondFloor[AstrologySigns[x]] ^ ValiditySecondFloor[AstrologySigns[y]]);
            break;
         case '&':
            Temp = ValiditySecondFloor[AstrologySigns[x]] & ValiditySecondFloor[AstrologySigns[y]];
            break;
         case '|':
            Temp = ValiditySecondFloor[AstrologySigns[x]] | ValiditySecondFloor[AstrologySigns[y]];
            break;
         case '⊕':
            Temp = ValiditySecondFloor[AstrologySigns[x]] ^ ValiditySecondFloor[AstrologySigns[y]];
            break;
         case '↑':
            Temp = !(ValiditySecondFloor[AstrologySigns[x]] & ValiditySecondFloor[AstrologySigns[y]]);
            break;
         case '↓':
            Temp = !(ValiditySecondFloor[AstrologySigns[x]] | ValiditySecondFloor[AstrologySigns[y]]);
            break;
      }
      switch (Operators[1 - w]) {
         //case '⇒':
         //  if (Validity[AstrologySigns[x]] && !Validity[AstrologySigns[y]])
         //  Temp = false;
         //break;
         case '⊙':
            SecondTemp = !(Temp ^ ValiditySecondFloor[AstrologySigns[z]]);
            break;
         case '&':
            SecondTemp = Temp & ValiditySecondFloor[AstrologySigns[z]];
            break;
         case '|':
            SecondTemp = Temp | ValiditySecondFloor[AstrologySigns[z]];
            break;
         case '⊕':
            SecondTemp = Temp ^ ValiditySecondFloor[AstrologySigns[z]];
            break;
         case '↑':
            SecondTemp = !(Temp & ValiditySecondFloor[AstrologySigns[z]]);
            break;
         case '↓':
            SecondTemp = !(Temp | ValiditySecondFloor[AstrologySigns[z]]);
            break;
      }
      return SecondTemp;
      Stage3:
      int Offset = Bomb.GetSerialNumberNumbers().Last() + 1;
      if (CheckPresses[0] && CheckPresses[1]) {
         Debug.LogFormat("[Astrological #{0}] The used operator to get the stage three value is AND.", moduleId);
         for (int i = 0; i < 12; i++) {
            ValidityThirdFloor[i] = Validity[i] & ValiditySecondFloor[(i + Offset) % 12];
         }
      }
      else if (!CheckPresses[0] && CheckPresses[1]) {
         Debug.LogFormat("[Astrological #{0}] The used operator to get the stage three value is OR.", moduleId);
         for (int i = 0; i < 12; i++) {
            ValidityThirdFloor[i] = Validity[i] | ValiditySecondFloor[(i + Offset) % 12];
         }
      }
      else if (CheckPresses[0] && !CheckPresses[1]) {
         Debug.LogFormat("[Astrological #{0}] The used operator to get the stage three value is XOR.", moduleId);
         for (int i = 0; i < 12; i++) {
            ValidityThirdFloor[i] = Validity[i] ^ ValiditySecondFloor[(i + Offset) % 12];
         }
      }
      else {
         Debug.LogFormat("[Astrological #{0}] The used operator to get the stage three value is XNOR.", moduleId);
         for (int i = 0; i < 12; i++) {
            ValidityThirdFloor[i] = Validity[i] == ValiditySecondFloor[(i + Offset) % 12] ? true : false;
         }
      }

      Debug.LogFormat("[Astrological #{0}] The following horoscopes are valid in stage 3: ", moduleId);
      LogCheck = 0;
      for (int i = 0; i < 12; i++) {
         if (ValidityThirdFloor[i]) {
            LogCheck++;
         }
      }
      if (LogCheck == 0) {
         Debug.LogFormat("[Astrological #{0}] Apparently, there are no valid horoscopes. ", moduleId);
      }
      else if (LogCheck == 12) {
         Debug.LogFormat("[Astrological #{0}] Apparently, everything is valid. ", moduleId);
      }
      else {
         for (int i = 0; i < 12; i++) {
            if (ValidityThirdFloor[i]) {
               Debug.LogFormat("[Astrological #{0}] {1}", moduleId, HoroscopeNames[i]);
            }
         }
      }

      switch (Operators[w]) {
         //case '⇒':
         //  if (Validity[AstrologySigns[x]] && !Validity[AstrologySigns[y]])
         //  Temp = false;
         //break;
         case '⊙':
            Temp = !(ValidityThirdFloor[AstrologySigns[x]] ^ ValidityThirdFloor[AstrologySigns[y]]);
            break;
         case '&':
            Temp = ValidityThirdFloor[AstrologySigns[x]] & ValidityThirdFloor[AstrologySigns[y]];
            break;
         case '|':
            Temp = ValidityThirdFloor[AstrologySigns[x]] | ValidityThirdFloor[AstrologySigns[y]];
            break;
         case '⊕':
            Temp = ValidityThirdFloor[AstrologySigns[x]] ^ ValidityThirdFloor[AstrologySigns[y]];
            break;
         case '↑':
            Temp = !(ValidityThirdFloor[AstrologySigns[x]] & ValidityThirdFloor[AstrologySigns[y]]);
            break;
         case '↓':
            Temp = !(ValidityThirdFloor[AstrologySigns[x]] | ValidityThirdFloor[AstrologySigns[y]]);
            break;
      }
      switch (Operators[1 - w]) {
         //case '⇒':
         //  if (Validity[AstrologySigns[x]] && !Validity[AstrologySigns[y]])
         //  Temp = false;
         //break;
         case '⊙':
            SecondTemp = !(Temp ^ ValidityThirdFloor[AstrologySigns[z]]);
            break;
         case '&':
            SecondTemp = Temp & ValidityThirdFloor[AstrologySigns[z]];
            break;
         case '|':
            SecondTemp = Temp | ValidityThirdFloor[AstrologySigns[z]];
            break;
         case '⊕':
            SecondTemp = Temp ^ ValidityThirdFloor[AstrologySigns[z]];
            break;
         case '↑':
            SecondTemp = !(Temp & ValidityThirdFloor[AstrologySigns[z]]);
            break;
         case '↓':
            SecondTemp = !(Temp | ValidityThirdFloor[AstrologySigns[z]]);
            break;
      }
      return SecondTemp;
   }

   bool eXish () {
      // || ( > 5 && double.Parse(Bomb.QueryWidgets("volt", "").Substring(12).Remove("\"}")) % 1 != 0)
      if (Bomb.QueryWidgets("volt", "").Count() != 0) {
         double Crackass = double.Parse(Bomb.QueryWidgets("volt", "")[0].Substring(12).Replace("\"}", ""));
         if (Crackass > 5 && Crackass % 1 != 0) {
            return true;
         }
      }
      return false;
   }

   IEnumerator MovingSpace () {
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
   private readonly string TwitchHelpMessage = @"Use !{0} green/g/r/red to hit that button.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      yield return null;
      Command = Command.Trim().ToUpper();
      if (Command == "GREEN" || Command == "G") {
         Buttons[0].OnInteract();
         yield return new WaitForSecondsRealtime(.1f);
      }
      else if (Command == "RED" || Command == "R") {
         Buttons[1].OnInteract();
         yield return new WaitForSecondsRealtime(.1f);
      }
      else {
         yield return "sendtochaterror I don't understand!";
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      while (!moduleSolved) {
         yield return ProcessTwitchCommand(HitCheck ? "green" : "red");
      }
   }
}
