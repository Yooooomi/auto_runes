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
using System.Linq;

namespace AutoRunes
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const String LeagueOfLegendsWindowName = "LeagueClientUx";

        WindowManager windowManager = new WindowManager();
        MouseManager mouseManager = new MouseManager();
        Parser parser = new Parser();

        public MainWindow()
        {
            InitializeComponent();

            /*windowManager.AssignWindow(LeagueOfLegendsWindowName);
            mouseManager.enableMouseClicks();
            mouseManager.addListener(onMouseClick);*/
        }

        private void onMouseClick(object sender, MouseEventExtArgs e)
        {
            WindowManager.Rect rect = windowManager.GetWindowPos();
            MouseManager.MousePoint point = mouseManager.GetCursorPosition();

            Debug.WriteLine((point.X - rect.Left).ToString() + " " + (point.Y - rect.Top).ToString());
        }

        private void clickRune(Runes rune)
        {
            Runes realRune;
            
            if (rune.type != Runes.RuneType.Button)
            {
                realRune = Runes.runes.Find(e => e.names[0] == rune.names[0] && e.type == rune.type);
            }
            else
            {
                realRune = Runes.buttons.Find(e => e.names[0] == rune.names[0] && e.type == rune.type);
            }

            WindowManager.Rect rect = windowManager.GetWindowPos();
            Position position = realRune.position;

            Position convertedPos = Runes.TransferPositionResolution(realRune.position, 1600, 900, rect.Right - rect.Left, rect.Bottom - rect.Top);

            if (champSelect.IsChecked == true)
            {
                convertedPos.x += (int) (0.085 * (rect.Right - rect.Left));
            }

            mouseManager.SetCursorPosition(rect.Left + convertedPos.x, rect.Top + convertedPos.y);
            mouseManager.Click();
            Thread.Sleep((int) buildSpeed.Value);
        }

        private void onFocusClick(object sender, RoutedEventArgs ev)
        {
            if (!windowManager.isUsable() && !windowManager.AssignWindow(LeagueOfLegendsWindowName)) return;

            windowManager.SetFocus();

            ProfileRunes runes = parser.Parse(urlText.Text);

            runes.primary.type = Runes.RuneType.PrimarySection;
            clickRune(runes.primary);
            for (int i = 0; i < 4; i++)
            {
                runes.runes[i].type = Runes.RuneType.Primary;
                clickRune(runes.runes[i]);
            }

            if (
                Runes.runes.FindIndex(e => e.names[0] == runes.primary.names[0] && e.type == runes.primary.type)
                    <
                Runes.runes.FindIndex(e => e.names[0] == runes.secondary.names[0] && e.type == runes.secondary.type)
            )
            {
                runes.secondary.position.x -= 50;
            }

            runes.secondary.type = Runes.RuneType.SecondarySection;
            clickRune(runes.secondary);
            for (int i = 4; i < 4 + 2; i++)
            {
                runes.runes[i].type = Runes.RuneType.Secondary;
                clickRune(runes.runes[i]);
            }
            for (int i = 6; i < 6 + 3; i++)
            {
                runes.runes[i].type = Runes.RuneType.Shard;
                clickRune(runes.runes[i]);
            }
            if (clickOnSave.IsChecked == true)
            {
                clickRune(Runes.buttons.Find(e => e.type == Runes.RuneType.Button && e.names.Contains("save")));
            }
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
