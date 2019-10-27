using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoRunes
{
    struct Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Runes
    {
        public static Position TransferPositionResolution(Position oldPos, double oldWidth, double oldHeight, double newWidth, double newHeight)
        {
            Position newPos;

            newPos.x = (int) (oldPos.x * (newWidth / oldWidth));
            newPos.y = (int) (oldPos.y * (newHeight / oldHeight));
            return newPos;
        }

        static public List<Runes> buttons = new List<Runes>
        {
            new Runes(RuneType.Button, new List<String>() { "save" }, new Position(500, 161)),
            new Runes(RuneType.Button, new List<String>() { "edit_rune" }, new Position(545, 854)),
            new Runes(RuneType.Button, new List<String>() { "square" }, new Position(227, 842)),
        };

        static public List<Runes> runes = new List<Runes>
        {
            new Runes(RuneType.PrimarySection, new List<String>() { "precision" }, new Position(163, 265)),
            new Runes(RuneType.PrimarySection, new List<String>() { "domination" }, new Position(214, 265)),
            new Runes(RuneType.PrimarySection, new List<String>() { "sorcery" }, new Position(262, 265)),
            new Runes(RuneType.PrimarySection, new List<String>() { "resolve" }, new Position(315, 265)),
            new Runes(RuneType.PrimarySection, new List<String>() { "inspiration" }, new Position(360, 265)),
            
            new Runes(RuneType.Primary, new List<String>() { "press the attack", "electrocute" }, new Position(169, 410)),
            new Runes(RuneType.Primary, new List<String>() { "lethal tempo", "predator" }, new Position(233, 410)),
            new Runes(RuneType.Primary, new List<String>() { "fleet footwork", "dark harvest" }, new Position(291, 410)),
            new Runes(RuneType.Primary, new List<String>() { "conqueror", "hail of blades" }, new Position(354, 410)),
            
            new Runes(RuneType.Primary, new List<String>() { "summon aery", "grasp of the undying", "glacial augment" }, new Position(180, 410)),
            new Runes(RuneType.Primary, new List<String>() { "arcane comet", "aftershock", "kleptomancy" }, new Position(266, 410)),
            new Runes(RuneType.Primary, new List<String>() { "phase rush", "guardian", "unsealed spellbook" }, new Position(346, 410)),
            
            new Runes(RuneType.Primary, new List<String>() { "overheal", "cheap shot", "nullifying orb", "demolish", "hextech flashtraption" }, new Position(182, 536)),
            new Runes(RuneType.Primary, new List<String>() { "triumph", "taste of blood", "manaflow band", "font of life", "magical footwear" }, new Position(266, 536)),
            new Runes(RuneType.Primary, new List<String>() { "presence of mind", "sudden impact", "nimbus cloak", "shield bash", "perfect timing" }, new Position(348, 536)),
            
            new Runes(RuneType.Primary, new List<String>() { "legend: alacrity", "zombie ward", "transcendence", "conditioning", "future's market" }, new Position(182, 645)),
            new Runes(RuneType.Primary, new List<String>() { "legend: tenacity", "ghost poro", "celerity", "second wind", "minion dematerializer" }, new Position(266, 645)),
            new Runes(RuneType.Primary, new List<String>() { "legend: bloodline", "eyeball collection", "absolute focus", "bone plating", "biscuit delivery" }, new Position(348, 645)),
            
            new Runes(RuneType.Primary, new List<String>() { "coup de grace", "scorch", "overgrowth", "cosmic insight" }, new Position(182, 757)),
            new Runes(RuneType.Primary, new List<String>() { "cut down", "waterwalking", "revitalize", "approach velocity" }, new Position(266, 757)),
            new Runes(RuneType.Primary, new List<String>() { "last stand", "gathering storm", "unflinching", "time warp tonic" }, new Position(348, 757)),
            
            new Runes(RuneType.Primary, new List<String>() { "ravenous hunter" }, new Position(170, 757)),
            new Runes(RuneType.Primary, new List<String>() { "ingenious hunter" }, new Position(233, 757)),
            new Runes(RuneType.Primary, new List<String>() { "relentless hunter" }, new Position(296, 757)),
            new Runes(RuneType.Primary, new List<String>() { "ultimate hunter" }, new Position(359, 757)),

            new Runes(RuneType.SecondarySection, new List<String>() { "precision" }, new Position(579, 267)), // Their position depends on the first rune chosen
            new Runes(RuneType.SecondarySection, new List<String>() { "domination" }, new Position(642, 267)),
            new Runes(RuneType.SecondarySection, new List<String>() { "sorcery" }, new Position(703, 267)),
            new Runes(RuneType.SecondarySection, new List<String>() { "resolve" }, new Position(763, 267)),
            new Runes(RuneType.SecondarySection, new List<String>() { "inspiration" }, new Position(763 + 50, 267)),

            new Runes(RuneType.Secondary, new List<String>() { "overheal", "cheap shot", "nullifying orb", "demolish", "hextech flashtraption" }, new Position(586, 380)),
            new Runes(RuneType.Secondary, new List<String>() { "triumph", "taste of blood", "manaflow band", "font of life", "magical footwear" }, new Position(670, 380)),
            new Runes(RuneType.Secondary, new List<String>() { "presence of mind", "sudden impact", "nimbus cloak", "shield bash", "perfect timing" }, new Position(756, 380)),

            new Runes(RuneType.Secondary, new List<String>() { "legend: alacrity", "zombie ward", "transcendence", "conditioning", "future's market" }, new Position(586, 475)),
            new Runes(RuneType.Secondary, new List<String>() { "legend: tenacity", "ghost poro", "celerity", "second wind", "minion dematerializer" }, new Position(670, 475)),
            new Runes(RuneType.Secondary, new List<String>() { "legend: bloodline", "eyeball collection", "absolute focus", "bone plating", "biscuit delivery" }, new Position(756, 475)),

            new Runes(RuneType.Secondary, new List<String>() { "coup de grace", "scorch", "overgrowth", "cosmic insight" }, new Position(586, 570)),
            new Runes(RuneType.Secondary, new List<String>() { "cut down", "waterwalking", "revitalize", "approach velocity" }, new Position(670, 570)),
            new Runes(RuneType.Secondary, new List<String>() { "last stand", "gathering storm", "unflinching", "time warp tonic" }, new Position(756, 570)),

            new Runes(RuneType.Secondary, new List<String>() { "ravenous hunter" }, new Position(576, 572)),
            new Runes(RuneType.Secondary, new List<String>() { "ingenious hunter" }, new Position(639, 572)),
            new Runes(RuneType.Secondary, new List<String>() { "relentless hunter" }, new Position(700, 572)),
            new Runes(RuneType.Secondary, new List<String>() { "ultimate hunter" }, new Position(764, 572)),

            new Runes(RuneType.Shard, new List<String>() { "diamond1" }, new Position(588, 646)),
            new Runes(RuneType.Shard, new List<String>() { "axe1" }, new Position(671, 646)),
            new Runes(RuneType.Shard, new List<String>() { "time1" }, new Position(753, 646)),

            new Runes(RuneType.Shard, new List<String>() { "diamond2" }, new Position(588, 700)),
            new Runes(RuneType.Shard, new List<String>() { "shield2" }, new Position(671, 700)),
            new Runes(RuneType.Shard, new List<String>() { "circle2" }, new Position(753, 700)),

            new Runes(RuneType.Shard, new List<String>() { "heart3" }, new Position(588, 755)),
            new Runes(RuneType.Shard, new List<String>() { "shield3" }, new Position(671, 755)),
            new Runes(RuneType.Shard, new List<String>() { "circle3" }, new Position(753, 755)),
        };

        public enum RuneType {
            PrimarySection,
            SecondarySection,
            Primary,
            Secondary,
            Shard,
            Button,
        }

        public Runes(RuneType type, List<String> names, Position position)
        {
            this.type = type;
            this.names = names;
            this.position = position;
        }

        public Runes copy()
        {
            return new Runes(this.type, this.names, this.position);
        }

        public RuneType type;
        public List<String> names;
        public Position position;
    }

    class ProfileRunes
    {
        public Runes primary;
        public Runes secondary;
        public List<Runes> runes;

        public ProfileRunes(Runes p, Runes s, List<Runes> runes)
        {
            this.primary = p;
            this.secondary = s;
            this.runes = runes;
        }
    }
}
