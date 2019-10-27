using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.Diagnostics;

namespace AutoRunes
{
    class Parser
    {
        public Parser()
        {
        }

        public ProfileRunes Parse(string url)
        {
            WebClient client = new WebClient();

            String html = client.DownloadString(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection primariesNode = doc.DocumentNode.SelectNodes("//div[@class='" + "new-runes__title" + "']");
            String primaryTier = primariesNode[0].Attributes["path"].Value;
            String secondaryTier = primariesNode[1].Attributes["path"].Value;

            Debug.WriteLine(primaryTier);
            Debug.WriteLine(secondaryTier);

            HtmlNodeCollection runes = doc.DocumentNode.SelectNodes("(//div[@class='new-runes__item']//span[string-length(text()) > 0])[position() < 7]");
            List<String> runeNames = new List<String>();

            foreach (var rune in runes)
            {
                runeNames.Add(rune.InnerText);
            }

            List<String> shardNames = new List<String>();

            HtmlNodeCollection shards = doc.DocumentNode.SelectNodes("(//div[@class='new-runes__shards']//span[@shard-type])[position() < 4]");
            foreach (var shard in shards)
            {
                shardNames.Add(shard.Attributes["shard-type"].Value);
            }

            List<Runes> finalRunes = new List<Runes>();

            foreach (var i in runeNames)
            {
                finalRunes.Add(Runes.runes.Find(e => e.names.Contains(i.ToLower())).copy());
            }
            for (int i = 0; i < shardNames.Count; i++)
            {
                Debug.WriteLine(shardNames[i].ToLower() + (i + 1).ToString());
                finalRunes.Add(Runes.runes.Find(e => e.names.Contains(shardNames[i].ToLower() + (i + 1).ToString())).copy());
            }

            return new ProfileRunes(
                Runes.runes.Find(e => e.type == Runes.RuneType.PrimarySection && e.names.Contains(primaryTier.ToLower())).copy(),
                Runes.runes.Find(e => e.type == Runes.RuneType.SecondarySection && e.names.Contains(secondaryTier.ToLower())).copy(),
                finalRunes
            );
        }
    }
}
