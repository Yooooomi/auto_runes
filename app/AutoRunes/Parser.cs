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

            string html = client.DownloadString(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNodeCollection primariesNode = doc.DocumentNode.SelectNodes("//div[@class='new-runes__title']");
            string primaryTier = primariesNode[0].Attributes["path"].Value;
            string secondaryTier = primariesNode[1].Attributes["path"].Value;

            HtmlNodeCollection primaryRunes = doc.DocumentNode.SelectNodes("(//div[@class='new-runes__primary']//div[@class='new-runes__item']/span[string-length(text()) > 0])[position() < 5]");
            HtmlNodeCollection secondaryRunes = doc.DocumentNode.SelectNodes("(//div[@class='new-runes__secondary']//div[@class='new-runes__item']/span[string-length(text()) > 0])[position() < 3]");
            HtmlNodeCollection shards = doc.DocumentNode.SelectNodes("(//div[@class='new-runes__shards']//span[@shard-type])[position() < 4]");

            List<string> primaryRuneNames = new List<string>();
            List<string> secondaryRuneNames = new List<string>();
            List<string> shardNames = new List<string>();

            if (primaryRunes != null)
            {
                foreach (var rune in primaryRunes)
                {
                    primaryRuneNames.Add(rune.InnerText);
                }
            }

            if (secondaryRunes != null)
            {
                foreach (var rune in secondaryRunes)
                {
                    secondaryRuneNames.Add(rune.InnerText);
                }
            }

            if (shards != null)
            {
                foreach (var shard in shards)
                {
                    shardNames.Add(shard.Attributes["shard-type"].Value);
                }
            }

            List<Runes> finalRunes = new List<Runes>();

            foreach (var i in primaryRuneNames)
            {
                Runes rune = Runes.GetRune(Runes.RuneType.Primary, i).copy();
                finalRunes.Add(rune);
            }
            foreach (var i in secondaryRuneNames)
            {
                Runes rune = Runes.GetRune(Runes.RuneType.Secondary, i).copy();
                finalRunes.Add(rune);
            }
            for (int i = 0; i < shardNames.Count; i++)
            {
                string shardName = shardNames[i].ToLower() + (i + 1);
                Runes shard = Runes.GetRune(Runes.RuneType.Shard, shardName).copy();
                finalRunes.Add(shard);
            }

            Runes primarySection = Runes.GetRune(Runes.RuneType.PrimarySection, primaryTier).copy();
            Runes secondarySection = Runes.GetRune(Runes.RuneType.SecondarySection, secondaryTier).copy();

            return new ProfileRunes(
                primarySection,
                secondarySection,
                finalRunes
            );
        }
    }
}
