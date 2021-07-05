namespace MoonlightEventGameBoard
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class Game
    {
        public Game()
        {
            this.Categories = new List<GameCategory>(
                new[]
                {
                    new GameCategory(
                        "Fotos",
                        "Red",
                        new[]
                        {
                            new CategoryLevel(20, new[] { "L1_Desc1", "L1_Desc2", "L1_Desc3", "L1_Desc4", }),
                            new CategoryLevel(40, new[] { "L2_Desc1", "L2_Desc2", "L2_Desc3", }),
                            new CategoryLevel(70, new[] { "L3_Desc1", "L3_Desc2", }),
                            new CategoryLevel(150, new[] { "L4_Desc1", })
                        }.ToList()),
                    new GameCategory(
                        "Rätsel",
                        "Green",
                        new[]
                        {
                            new CategoryLevel(20, new[] { "L1_Desc1", "L1_Desc2", "L1_Desc3", "L1_Desc4", }),
                            new CategoryLevel(40, new[] { "L2_Desc1", "L2_Desc2", "L2_Desc3", }),
                            new CategoryLevel(70, new[] { "L3_Desc1", "L3_Desc2", }),
                            new CategoryLevel(150, new[] { "L4_Desc1", })
                        }.ToList()),
                    new GameCategory(
                        "Aktives",
                        "RoyalBlue",
                        new[]
                        {
                            new CategoryLevel(20, new[] { "L1_Desc1", "L1_Desc2", "L1_Desc3", "L1_Desc4", }),
                            new CategoryLevel(40, new[] { "L2_Desc1", "L2_Desc2", "L2_Desc3", }),
                            new CategoryLevel(70, new[] { "L3_Desc1", "L3_Desc2", }),
                            new CategoryLevel(150, new[] { "L4_Desc1", })
                        }.ToList()),
                    new GameCategory(
                        "Kreatives",
                        "Orange",
                        new[]
                        {
                            new CategoryLevel(20, new[] { "L1_Desc1", "L1_Desc2", "L1_Desc3", "L1_Desc4", }),
                            new CategoryLevel(40, new[] { "L2_Desc1", "L2_Desc2", "L2_Desc3", }),
                            new CategoryLevel(70, new[] { ChallengeTexts.K_8, "L3_Desc2", }),
                            new CategoryLevel(150, new[] { "L4_Desc1", })
                        }.ToList()),
                });
        }

        public List<GameCategory> Categories { get; }

        public bool ShowDialog { get; set; }

        public Challenge? DialogChallenge { get; set; }

        public void OpenDialog(Challenge challenge)
        {
            this.DialogChallenge = challenge;
            this.ShowDialog = true;
        }

        public void CloseDialog(bool challengeDone)
        {
            if (this.DialogChallenge != null)
            {
                this.DialogChallenge.IsDone = challengeDone;
            }

            this.DialogChallenge = null;
            this.ShowDialog = false;
        }
    }

    public class GameCategory
    {
        public GameCategory(string title, string color, List<CategoryLevel> levels)
        {
            this.Title = title;
            this.Color = color;
            this.Levels = levels;
            this.Levels.ForEach(
                x =>
                {
                    x.correspondingCategory = this;
                });
        }

        public string Title { get; set; }

        public string Color { get; }

        public List<CategoryLevel> Levels { get; }
    }

    public class CategoryLevel
    {
        internal GameCategory correspondingCategory = null!;

        public CategoryLevel(int challengesScore, IEnumerable<string> descriptions)
        {
            this.ChallengesScore = challengesScore;
            this.Challenges = descriptions.Select(x => new Challenge(this, this.ChallengesScore, x)).ToList();
        }

        public int ChallengesScore { get; }

        public List<Challenge> Challenges { get; }

        public bool LevelCompleted => this.Challenges.All(x => x.IsDone);

        public bool CanToggleChallenges => this.CalcCanToggleChallenges();

        private bool CalcCanToggleChallenges()
        {
            var index = this.correspondingCategory.Levels.IndexOf(this);

            var previousDone = index == 0 ? true : this.correspondingCategory.Levels[index - 1].LevelCompleted;

            var followingNotStarted = index == this.correspondingCategory.Levels.Count - 1
                ? true
                : this.correspondingCategory.Levels[index + 1].Challenges.All(x => !x.IsDone);

            return previousDone && followingNotStarted;
        }

        public double LevelSpaceMutliplier()
        {
            switch (this.Challenges.Count)
            {
                case 4:
                    return 1.3;
                case 3:
                    return 1.15;
                case 2:
                    return 0.85;
                case 1:
                    return 0.7;
            }

            return 1;
        }
    }

    public class Challenge
    {
        private CategoryLevel correspondingLevel;

        public Challenge(CategoryLevel correspondingLevel, int score, string description)
        {
            this.correspondingLevel = correspondingLevel;
            this.Score = score;
            this.Description = description;
        }

        public int Score { get; }

        public string Description { get; }

        public bool IsDone { get; set; }

        public bool CanBeToggled => this.correspondingLevel.CanToggleChallenges;
    }

    public static class ChallengeTexts
    {
        public static string F_1 = @"Macht ein Foto, bei dem die ganze Kleingruppe eine Rockband pantomimisch darstellt.";
        public static string F_2 = @"Findet etwas, auf dem die Zahl 7 steht und macht ein Bild davon.";
        public static string F_3 = @"Macht ein Foto, auf dem eure Kleingruppe auf einem Zebrastreifen steht.";
        public static string F_4 = @"Macht ein Foto, auf dem ihr mit mindestens zwei Fahrrädern und einem Auto drauf zu sehen seid.";
        public static string F_5 = @"Macht ein Foto von einem Fahrzeug, das kein Auto oder Fahrrad ist.";
        public static string F_6 = @"Macht ein Foto, mit mindestens drei verschiedenen Sportgegenständen (z.B. Fußball, Volleyball, Indiaca...).";
        public static string F_7 = @"Macht ein Bild von zwei Teammitgliedern auf einem Baum.";
        public static string F_8 = @"Macht ein Foto, auf dem eure ganze Kleingruppe in (/an/um) einem Einkaufswagen zu sehen ist.";
        public static string F_9 = @"Joker: Glückwunsch, diese Aufgabe habt ihr automatisch geschafft.";
        public static string F_10 = @"Macht ein Bild, auf dem mindestens 3 Personen komplett im Wasser sind (bis auf Kopf).";

        public static string R_1 = @"Zwei Väter und zwei Söhne stellen sich nebeneinander vor einem großen Spiegel. Doch im Spiegelbild sind nur drei Personen zu sehen. Wie kann das sein?";
        public static string R_2 = @"Welcher Vogel kann seinen eigenen Namen rufen?";
        public static string R_3 = @"Wie kann man 1 Liter Wasser in einem Sieb transportieren?";
        public static string R_4 = @"Löst dieses Kreuzworträtsel für Kinder. (Anlage R4)";
        public static string R_5 = @"Finde die 12 Fehler. (Anlage R5)";
        public static string R_6 = @"Welche 3 Würfel wurden falsch dargestellt? (Anlage R6)";
        public static string R_7 = @"Welche Wörter können thematisch zum Bild passen? (Anlage R7)";
        public static string R_8 = @"Löst das Sudoku  (Anlage R8).";
        public static string R_9 = @"Welcher bekannte Spruch verbirgt sich in dieser Wortschlange? (Anlage R9)";
        public static string R_10 = @"In den exklusiven Buchstabenclub dürfen nur sehr privilegierte Buchstaben eintreten. Beim letzten Treffen hat sich ein Buchstabe Zutritt verschafft, der nicht zur auserwählten Schicht gehört. Welcher Buchstabe war das?<br />P G D B L C S  (Anlage R10)";

        public static string A_1 = @"Messt eine Strecke von 15 Metern ab. Jeder muss einmal in der Schubkarrenposition auf die andere Seite kommen.";
        public static string A_2 = @"Schreibt alle eure Geburtstage (z.B. 13.05.2003) und Hausnummern (z.B. 10) auf einen Zettel und rechnet die Zahl aus (12+5+2003+10 = 2030). Am Schluss müssen alle Zahlen miteinander addiert werden für ein Gruppenergebnis. Ohne Handy und Taschenrechner.";
        public static string A_3 = @"Baut mind. 2 Papierflieger, die jeweils 7m weit fliegen. Erst wenn beide über 7 Meter geflogen sind habt ihr die Aufgabe gelöst. Die, die gebastelt haben dürfen nicht werfen.";
        public static string A_4 = @"Legt mit all euren Schuhen aus der Kleingruppe ein Herz und verknotet alle miteinander mit einem Seil oder den Schnürsenkeln. Jeder Schuh muss einen Knoten dran haben.";
        public static string A_5 = @"Macht als Kleingruppe insgesamt nacheinander X Liegestützen (X=20 * Personenanzahl). Mindestens 3 Personen müssen beteiligt sein.";
        public static string A_6 = @"Joker: Glückwunsch, diese Aufgabe habt ihr automatisch geschafft.";
        public static string A_7 = @"Spielt Pantomime zu folgenden Begriffen, die der Spielleiter jeweils einer Person geheim sagt. Wenn ihr alle 4 Begriffe erraten habt, habt ihr die Aufgabe geschafft. Begriffe stehen im Spielheft.";
        public static string A_8 = @"Jeder aus der Gruppe muss sich folgende Gegenstände suchen um die Aufgabe zu schaffen (blauer Gegenstand, roter Gegenstand, gelber Gegenstand,). Es dürfen nicht zweimal dieselben Gegenstände innerhalb einer Kleingruppe verwendet werden.";
        public static string A_9 = @"Baut einen kleinen Hindernissparcour (mindestens 15m + 3 Hindernisse). Jeder der Gruppe muss einmal blind durch den Parcour durchgeleitet werden- nur durch die Stimme der anderen.";
        public static string A_10 = @"Macht ein Foto mit eurem Ortsschild am Ortseingang.";

        public static string K_1 = @"Malt dieses Bild vollständig aus. Benutzt dafür eure schwache Hand. (Anlage K1)";
        public static string K_2 = @"Baut aus drei kleinen Stöcken (oder Ästen. Hauptsache aus der Natur) ein Dreibein, dass von alleine steht und macht ein Foto davon.";
        public static string K_3 = @"Baut das CVJM-Dreieck nach inkl. Schriftzug (mit Gegenständen eurer Wahl. Nicht malen)";
        public static string K_4 = @"Joker: Glückwunsch, diese Aufgabe habt ihr automatisch geschafft.";
        public static string K_5 = @"Nehmt als Kleingruppe ein mit Wasser gegurgeltes Lied auf. Alle haben Wasser im Mund und gurgeln dasselbe Lied. Nehmt das ganze per Video auf und zeigt es einer Person, die nicht zu eurer Kleingruppe gehört. Wenn sie euer Lied richtig errät, ist die Aufgabe geschafft.";
        public static string K_6 = @"Vervollständigt dieses Bild richtig. (Anlage K6)";
        public static string K_7 = @"Schreibt einen lieben Brief an die beiden Badentreffmoderatoren, in dem ihr die beiden überschwänglich lobt (Mind. 100 Wörter). Folgende Wörter müssen vorkommen: 'stark', 'überragend', 'Wetter', 'egal', 'Deutschland ist stabil'). Schickt den Brief an: CVJM Baden, z.H.v. Matthias Kerschbaum, Mühlweg 10, 76703 Kraichtal";
        public static string K_8 =
            @"Schreibt ein Gedicht über das Thema ""Deutschland ist bei der EM ausgeschieden"" und tragt es einer Person vor, die nicht zu eurer Kleingruppe gehört. Erst wenn sie das Gedicht vollständig gehört hat und am Schluss sagt ""das war wirklich ein sehr schönes Gedicht"" ist die Aufgabe geschafft.";
        public static string K_9 = @"Faltet eine Origami Taube nach dieser Anleitung. (Anlage K9)";
        public static string K_10 = @"Bemalt mind. 3 Personen aus eurer Kleingruppe mit Schlamm im Gesicht.";
    }
}