using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutorunesLauncher
{
    class Program
    {
        public static void Main(string[] args)
        {
            JObject data;
            while ((data = Read()) != null)
            {
                var processed = ProcessMessage(data);
                if (processed)
                {
                    return;
                }
            }
        }

        const String LeagueOfLegendsWindowName = "LeagueClientUx";

        public static bool ProcessMessage(JObject data)
        {
            var message = data["text"].Value<string>();
            var inGame = data["inGame"].Value<bool>();

            if (message == "exit") return true;
            if (message.StartsWith("url:"))
            {
                WindowManager manager = new WindowManager();

                if (!manager.AssignWindow(LeagueOfLegendsWindowName)) return false;

                string url = message.Substring(4);

                if (!url.Contains("https://www.mobafire.com/league-of-legends/build/")) return false;
                Write("Starting !!" + url + " " + inGame.ToString());
                var autorune = Process.Start("C:\\Program Files (x86)\\Yooooomi\\Autorunes\\Autorunes.exe", url + " " + inGame.ToString());
                autorune.WaitForExit();
                Write(autorune.ExitCode.ToString());
                return false;
            }
            return true;
        }

        public static JObject Read()
        {
            var stdin = Console.OpenStandardInput();

            var lengthBytes = new byte[4];
            stdin.Read(lengthBytes, 0, 4);
            int length = BitConverter.ToInt32(lengthBytes, 0);

            var buffer = new char[length];
            using (var reader = new StreamReader(stdin))
            {
                while (reader.Peek() >= 0)
                {
                    reader.Read(buffer, 0, buffer.Length);
                }
            }

            return (JObject) JsonConvert.DeserializeObject<JObject>(new string(buffer));
        }

        public static void Write(JToken data)
        {
            var json = new JObject();
            json["data"] = data;

            var bytes = System.Text.Encoding.UTF8.GetBytes(json.ToString(Formatting.None));

            var stdout = Console.OpenStandardOutput();
            stdout.WriteByte((byte) ((bytes.Length >> 0) & 0xFF));
            stdout.WriteByte((byte) ((bytes.Length >> 8) & 0xFF));
            stdout.WriteByte((byte) ((bytes.Length >> 16) & 0xFF));
            stdout.WriteByte((byte) ((bytes.Length >> 24) & 0xFF));
            stdout.Write(bytes, 0, bytes.Length);
            stdout.Flush();
        }
    }
}
