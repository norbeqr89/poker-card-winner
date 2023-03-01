// See https://aka.ms/new-console-template for more information
using CommandLineParser.Arguments;
using System;
using System.Drawing;
using System.Text.RegularExpressions;

internal class Program
{
    private static void Main(string[] args)
    {
        // Get arguments
        (string fileNameInput, string fileNameOutput) = ParseArguments(args);

        // Set a new instance for CardGame
        var cardGame = new CardGame();

        // Processing the file
        cardGame.ProccessFile(fileNameInput, fileNameOutput);
    }

    /// <summary>
    /// Get the parameters from args
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static (string, string) ParseArguments(string[] args)
    {
        CommandLineParser.CommandLineParser parser = new CommandLineParser.CommandLineParser();
        ValueArgument<string> inputFileArgument = new ValueArgument<string>('i', "in", "File input data");
        parser.Arguments.Add(inputFileArgument);

        ValueArgument<string> outputFileArgument = new ValueArgument<string>('o', "out", "File output data");
        parser.Arguments.Add(outputFileArgument);

        string inputFile = "data.txt";
        string outputFile = "result.txt";

        parser.ParseCommandLine(args);
        // access the values 
        if (inputFileArgument.Parsed)
        {
            inputFile = inputFileArgument.Value;
        }

        if (outputFileArgument.Parsed)
        {
            outputFile = outputFileArgument.Value;
        }

        return (inputFile, outputFile);
    }
}

public class CardGame
{

    private readonly List<PlayerCardData> PlayersCard;
    private readonly Dictionary<string, byte> CardTypes;
    private readonly Dictionary<string, byte> CardValues;

    public CardGame()
    {
        PlayersCard = new List<PlayerCardData>();

        CardTypes = new Dictionary<string, byte>
            {
                { "D", 1 },
                { "H" , 2 },
                { "S" , 3 },
                { "C" , 4 },
            };

        CardValues = new Dictionary<string, byte>
            {
                { "A", 11 },
                { "J", 11 },
                { "Q", 12 },
                { "K", 13 },
                { "2", 2 },
                { "3", 3 },
                { "4", 4 },
                { "5", 5 },
                { "6", 6 },
                { "7", 7 },
                { "8", 8 },
                { "9", 9 },
                { "10", 10 }
            };
    }

    #region Private Methods

    /// <summary>
    /// Check if card has a valid value
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private bool IsValidCardValue(string card)
    {
        // check card face value
        string cardValue = card[..^1];

        if (!CardValues.ContainsKey(cardValue.ToUpper()))
            return false;

        return true;
    }

    /// <summary>
    /// Check if card has a valid type
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    private bool IsValidCardType(string card)
    {
        // check card type
        string cardType = card.Substring(card.Length - 1);

        if (!CardTypes.ContainsKey(cardType.ToUpper()))
            return false;


        return true;
    }

    /// <summary>
    /// Get the card value by number
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private int GetCardValueByFace(string card)
    {
        if (IsValidCardValue(card) is false)
            throw new ArgumentException("The card must to have some value as : 'A, J, K, Q, 2, 3 until 10, please check your input data'");

        string cardKey = card.Substring(0, card.Length - 1);
        return CardValues.GetValueOrDefault(cardKey);
    }

    /// <summary>
    /// Get the card value by number and type
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private int GetCardValueByType(string card)
    {
        var valueCard = GetCardValueByFace(card);

        if (IsValidCardType(card) is false)
            throw new ArgumentException("The card must to have some value as : 'D, H, S or C, please check your input data'");

        string cardKey = card.Substring(card.Length - 1);
        int valueCardType = CardTypes.GetValueOrDefault(cardKey);

        return valueCard + valueCardType;
    }

    /// <summary>
    /// Get winners by face value
    /// </summary>
    /// <returns></returns>
    private List<PlayerCardData> GetWinnersByFace()
    {
        int maxValue = 0;
        int i = 0;

        var winnersPlayers = new List<PlayerCardData>();

        while (i <= PlayersCard.Count - 1 && maxValue <= PlayersCard[i].SumCardsValue)
        {

            winnersPlayers.Add(PlayersCard[i]);
            maxValue = PlayersCard[i].SumCardsValue;
            i++;
        }

        return winnersPlayers;
    }

    /// <summary>
    /// Get winners by face and suit value
    /// </summary>
    /// <param name="players"></param>
    /// <returns></returns>
    private static List<PlayerCardData> GetWinnersByFaceAndSuit(List<PlayerCardData> players)
    {
        int maxValue = 0;
        int i = 0;

        var winnersPlayers = new List<PlayerCardData>();

        while (i <= players.Count - 1 && maxValue <= players[i].SumCardsValuePlusType)
        {

            winnersPlayers.Add(players[i]);
            maxValue = players[i].SumCardsValuePlusType;
            i++;
        }

        return winnersPlayers;
    }

    #endregion

    #region Public Methods
    /// <summary>
    /// Read the input data
    /// </summary>
    /// <param name="fileName"></param>
    /// <exception cref="ArgumentException"></exception>
    public void ReadInput(string fileName = "data.txt")
    {
        // get file by name

        // read data of 7 rows
        var inputData = File.ReadLines(@$"{fileName}");

        if (!inputData.Any())
            throw new ArgumentException("File is empty");

        foreach (var line in inputData)
        {

            var t = Regex.Match(line, "([a-zA-Z0-9 ]+?):(([a-zA-Z0-9 ]+?),){4}([a-zA-Z0-9 ]{2})");

            if (!t.Success)
                throw new ArgumentException("Some row in the file is wrong");

            var dataSplit = line.Replace(" ", "").Split(':');

            var playerName = dataSplit[0];

            var cards = (dataSplit[1]).ToUpper().Split(',').ToList();

            if (cards is not null)
            {
                cards.Sort((x, y) => GetCardValueByFace(y).CompareTo(GetCardValueByFace(x)));

                var playerData = new PlayerCardData
                {
                    PlayerName = playerName,
                    SumCardsValue = cards.Take(3).Sum((x) => GetCardValueByFace(x)),
                    SumCardsValuePlusType = cards.Take(3).Sum((x) => GetCardValueByType(x)),
                };

                PlayersCard.Add(playerData);
            }
        }

        PlayersCard.Sort((x, y) => y.SumCardsValue.CompareTo(x.SumCardsValue));
    }

    /// <summary>
    /// Get the final winner
    /// </summary>
    /// <returns></returns>
    public string GetWinner()
    {
        var winnersPlayers = GetWinnersByFace();

        if (winnersPlayers.Count > 1)
        {
            winnersPlayers = GetWinnersByFaceAndSuit(winnersPlayers);

            if (winnersPlayers.Count > 1)
            {
                return $"{string.Join(",", winnersPlayers.Select(x => x.PlayerName).ToArray())}:{winnersPlayers[0].SumCardsValuePlusType}";
            }
            else
            {
                return $"{winnersPlayers[0].PlayerName}:{winnersPlayers[0].SumCardsValuePlusType}";
            }
        }
        else
        {
            return $"{winnersPlayers[0].PlayerName}:{winnersPlayers[0].SumCardsValue}";
        }
    }

    public void ProccessFile(string fileName, string fileNameOutput)
    {
        try
        {
            // read the data
            ReadInput(fileName);

            // process and get the winner
            string winnerResult = GetWinner();

            // Create the file.
            File.WriteAllText(fileNameOutput, winnerResult);
        }
        catch (Exception e)
        {
            File.WriteAllText(fileNameOutput, $"Exception: {e.Message}");
        }
    }
    #endregion
}

public struct PlayerCardData
{
    public string PlayerName;
    public int SumCardsValue;
    public int SumCardsValuePlusType;
}