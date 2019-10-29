using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gma.System.MouseKeyHook;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace AutoRunes
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const String LeagueOfLegendsWindowName = "LeagueClientUx";
        readonly WindowManager windowManager = new WindowManager();
        readonly MouseManager mouseManager = new MouseManager();
        readonly Parser parser = new Parser();

        public MainWindow()
        {
            File.AppendAllText("C:\\users\\timot\\output", "salut");
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1 && args[1].Contains("https://www.mobafire.com/league-of-legends/build/"))
            {
                foreach(var i in args)
                {
                    File.AppendAllText("C:\\users\\timot\\output", i);
                }
                // args[2] is inGame
                bool inChampSelect = args[2] == "True";
                autorunes(args[1], inChampSelect, true, true, 200);
                Environment.Exit(0);
            }

            InitializeComponent();

/*            windowManager.AssignWindow(LeagueOfLegendsWindowName);
            mouseManager.enableMouseClicks();
            mouseManager.addListener(onMouseClick);*/
        }

        private void onMouseClick(object sender, MouseEventExtArgs e)
        {
            WindowManager.Rect rect = windowManager.GetWindowPos();
            MouseManager.MousePoint point = mouseManager.GetCursorPosition();

            Debug.WriteLine((point.X - rect.Left).ToString() + " " + (point.Y - rect.Top).ToString());
        }

        private void clickRune(Runes rune, bool isInChampSelect, int sleepTime, bool indentLeft = false)
        {
            Runes realRune;
            
            if (rune.type != Runes.RuneType.Button)
            {
                realRune = Runes.GetRune(rune.type, rune.names[0]);
            }
            else
            {
                realRune = Runes.GetRune(rune.type, rune.names[0]);
            }

            WindowManager.Rect rect = windowManager.GetWindowPos();

            if (indentLeft)
            {
                realRune.position.x -= 50;
            }

            Position convertedPos = Runes.TransferPositionResolution(realRune.position, 1600, 900, rect.Right - rect.Left, rect.Bottom - rect.Top);

            if (isInChampSelect == true && !realRune.position.alreadyTranslated)
            {
                convertedPos.x += (int) (0.085 * (rect.Right - rect.Left));
            }

            mouseManager.SetCursorPosition(rect.Left + convertedPos.x, rect.Top + convertedPos.y);
            mouseManager.Click();
            Thread.Sleep(sleepTime);
        }

        private void autorunes(string url, bool inChampSelect, bool clickEditRune, bool clickSave, int sleepTime)
        {
            if (!windowManager.AssignWindow(LeagueOfLegendsWindowName)) return;
            windowManager.SetFocus();

            ProfileRunes runes;

            try
            {
                runes = parser.Parse(url);
            }
            catch
            {
                return;
            }

            if (clickEditRune)
            {
                clickRune(Runes.GetRune(Runes.RuneType.Button, "edit_rune"), inChampSelect, sleepTime);
            }

            clickRune(Runes.GetRune(Runes.RuneType.Button, "square"), inChampSelect, sleepTime);

            runes.primary.type = Runes.RuneType.PrimarySection;
            clickRune(runes.primary, inChampSelect, sleepTime);
            for (int i = 0; i < 4; i++)
            {
                runes.runes[i].type = Runes.RuneType.Primary;
                clickRune(runes.runes[i], inChampSelect, sleepTime);
            }

            bool indentLeft = false;
            if (
                Runes.GetRuneIndex(Runes.RuneType.PrimarySection, runes.primary.names[0])
                    <
                Runes.GetRuneIndex(Runes.RuneType.PrimarySection, runes.secondary.names[0])
            )
            {
                indentLeft = true;
            }

            runes.secondary.type = Runes.RuneType.SecondarySection;
            clickRune(runes.secondary, inChampSelect, sleepTime, indentLeft);
            for (int i = 4; i < 4 + 2; i++)
            {
                runes.runes[i].type = Runes.RuneType.Secondary;
                clickRune(runes.runes[i], inChampSelect, sleepTime);
            }
            for (int i = 6; i < 6 + 3; i++)
            {
                runes.runes[i].type = Runes.RuneType.Shard;
                clickRune(runes.runes[i], inChampSelect, sleepTime);
            }
            if (clickSave == true)
            {
                clickRune(Runes.GetRune(Runes.RuneType.Button, "save"), inChampSelect, sleepTime);
            }
        }

        private void onFocusClick(object sender, RoutedEventArgs ev)
        {
            int sleepTime = (int) buildSpeed.Value;
            autorunes(urlText.Text, champSelect.IsChecked.Value, isInRunePage.IsEnabled == true && isInRunePage.IsChecked == false, clickOnSave.IsChecked.Value, sleepTime);
        }

        private void checkState(object sender, RoutedEventArgs e)
        {
            lolOpen.IsChecked = !(!windowManager.isUsable() && windowManager.AssignWindow(LeagueOfLegendsWindowName));
        }

        private void buildSpeedChange(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (buildSpeedText != null)
                buildSpeedText.Text = ((int) e.NewValue).ToString() + "ms between clicks";
        }
    }
}
