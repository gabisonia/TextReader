using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;

namespace TextReader
{
    internal class DetectCombinations
    {
        public static void Do(Action quit)
        {
            var map = new Dictionary<Combination, Action>
            {
                 {Combination.FromString("Control+C"), () => SayCopiedText()},
                 {Combination.FromString("Escape"), quit},
            };

            Console.WriteLine("Press ESC to exit.");
            Hook.GlobalEvents().OnCombination(map);
        }

        public static void SayCopiedText()
        {
            Thread.Sleep(500);
            var text = Clipboard.GetText();
            var speechSynthesizerObj = new SpeechSynthesizer();

            speechSynthesizerObj.SpeakAsync(text);
        }
    }
}
