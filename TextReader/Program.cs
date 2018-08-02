using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading;
using System.Windows.Forms;

namespace TextReader
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var selector = new Dictionary<string, Action<Action>>
            {
                {"1. Start", DetectCombinations.Do},
                {"Q. Quit", Exit}
            };

            Action<Action> action = null;

            while (action == null)
            {
                Console.WriteLine("Please select one of these:");
                foreach (var selectorKey in selector.Keys)
                    Console.WriteLine(selectorKey);
                var ch = Console.ReadKey(true).KeyChar;
                action = selector
                    .Where(p => p.Key.StartsWith(ch.ToString()))
                    .Select(p => p.Value).FirstOrDefault();
            }

            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----Application started-----");
            Console.WriteLine("-----------------------------");

            action(Application.Exit);

            Application.Run(new ApplicationContext());
        }

        private static void Exit(Action quit)
        {
            Application.Exit();
        }
    }

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
