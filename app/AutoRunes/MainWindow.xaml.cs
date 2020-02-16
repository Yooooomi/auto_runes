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
        const string LeagueOfLegendsWindowName = "LeagueClientUx";
        readonly WindowManager windowManager = new WindowManager();
        readonly MouseManager mouseManager = new MouseManager();
        readonly Parser parser = new Parser();

        public MainWindow()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length > 1 && args[1].Contains("https://www.mobafire.com/league-of-legends/build/"))
            {
                // args[2] is inGame
                bool inChampSelect = args[2] == "True";
                autorunes(args[1], inChampSelect, true, true, 200);
                Environment.Exit(0);
            }

            InitializeComponent();

            windowManager.AssignWindow(LeagueOfLegendsWindowName);
            mouseManager.enableMouseClicks();
            //mouseManager.addListener(onMouseClick);
        }

        private void onMouseClick(object sender, MouseEventExtArgs e)
        {
            WindowManager.Rect rect = windowManager.GetWindowPos();
            MouseManager.MousePoint point = mouseManager.GetCursorPosition();

            Debug.WriteLine((point.X - rect.Left).ToString() + " " + (point.Y - rect.Top).ToString());
        }

        private void clickRune(Runes rune, bool isInChampSelect, int sleepTime, bool indentLeft = false)
        {
            WindowManager.Rect rect = windowManager.GetWindowPos();

            if (indentLeft)
            {
                rune.position.x -= 50;
            }

            Position convertedPos = Runes.TransferPositionResolution(rune.position, 1600, 900, rect.Right - rect.Left, rect.Bottom - rect.Top);

            if (isInChampSelect == true && !rune.position.alreadyTranslated)
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

                runes = parser.Parse(url);

            if (clickEditRune)
            {
                clickRune(Runes.GetRune(Runes.RuneType.Button, "edit_rune"), inChampSelect, sleepTime);
            }

            clickRune(Runes.GetRune(Runes.RuneType.Button, "square"), inChampSelect, sleepTime);

            clickRune(runes.primary, inChampSelect, sleepTime);
            foreach (Runes i in runes.primaryRunes)
            {
                clickRune(i, inChampSelect, sleepTime);
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

            clickRune(runes.secondary, inChampSelect, sleepTime, indentLeft);
            foreach (Runes i in runes.secondaryRunes)
            {
                clickRune(i, inChampSelect, sleepTime);
            }
            foreach (Runes i in runes.shards)
            {
                clickRune(i, inChampSelect, sleepTime);
            }

            if (clickSave == true)
            {
                clickRune(Runes.GetRune(Runes.RuneType.Button, "save"), inChampSelect, sleepTime * 2);
                if (inChampSelect)
                {
                    clickRune(Runes.GetRune(Runes.RuneType.Button, "close"), inChampSelect, sleepTime);
                }
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
