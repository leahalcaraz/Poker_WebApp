using Poker_WebApp.Model;
using System;
using System.Configuration;

namespace Poker_WebApp
{
    public class PokerProgram
    {
        #region Default
        private static readonly string defaultNumberOfPlayers = ConfigurationManager.AppSettings["NumberOfPlayers"].ToString();
        #endregion
        public static void Main()
        {
            var poker = new PokerGame(int.Parse(defaultNumberOfPlayers));
            poker.InitializeDeck();
            poker.InitializeGame();
            Console.WriteLine(poker.Hands());
            Console.WriteLine(poker.Winner());
            Console.ReadLine();
        }
    }
}

