﻿using System;
using System.Collections.Generic;
using MonopolyKata.Cards;
using MonopolyKata.CoreComponents;
using MonopolyKata.RentStrategies;
using MonopolyKata.Spaces;
using NUnit.Framework;

namespace MonopolyKata.Tests.CardTests
{
    [TestFixture]
    public class ChanceTests
    {
        private Player player1;
        private Player player2;
        private List<Player> players;
        private Banker banker;
        private LoadedDice dice;
        private PrisonGuard guard;
        private BoardFactory boardFactory;
        private Board board;

        [SetUp]
        public void SetUp()
        {
            player1 = new Player("Horse");
            player2 = new Player("Car");
            players = new List<Player> { player1, player2 };
            banker = new Banker(players, 1500);
            dice = new LoadedDice();
            guard = new PrisonGuard(players, banker, dice);
            boardFactory = new BoardFactory();
            board = boardFactory.Create(banker, players, dice, guard);
        }

        [Test]
        public void BankDividendPaysPlayer50Bucks()
        {
            var bankDividend = new PayableCard("Bank Dividend", banker, 50);
            var previousBalance = banker.GetBalance(player1);
            bankDividend.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance + 50));
        }

        [Test]
        public void TakeAWalkOnTheBoardwalkMovesPlayerToBoardwalk()
        {
            var moveToBoardwalk = new MoveableCard("Take a Walk on the boardwalk", board, banker, 39);
            moveToBoardwalk.Play(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(39));
        }

        [Test]
        public void GoToReadingRailroadShouldPayThePlayer200()
        {
            board.MoveTo(player1, 5);
            board.MoveTo(player1, 36);
            var rideTheReading = new MoveableCard("Ride the Reading Railroad", board, banker, 5);
            var previousBalance = banker.GetBalance(player1);

            rideTheReading.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance + 200));
        }

        [Test]
        public void GoBack3SpacesMovesThePlayerBackwards3Spaces()
        {
            board.MoveTo(player1, 28);
            var goBack3Spaces = new GoBackSpaces(board, 3);

            goBack3Spaces.Play(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(25));
        }

        [Test]
        public void GoBack3SpacesShouldNotPayThePlayer200Dollars()
        {
            board.MoveTo(player1, 13);
            var goBack3Spaces = new GoBackSpaces(board, 3);
            var previousBalance = banker.GetBalance(player1);

            goBack3Spaces.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.LessThanOrEqualTo(previousBalance));
        }

        [Test]
        public void ChairmanOfTheBoardMakesThePlayerPayEachOtherPlayer50Dollars()
        {
            var player3 = new Player("Dog");
            players.Add(player3);
            banker = new Banker(players, 1500);
            var chairmanOfTheBoard = new PayEachPlayer(banker, 50);
            var previousBalance = banker.GetBalance(player1);

            chairmanOfTheBoard.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance - 100));
        }

        [Test]
        public void MoveToTheNextRailroadMovesToTheCorrectRailroad()
        {
            banker = new Banker(players, 1500);
            var railroads = new List<Railroad>();
            var railroadRentStrategy = new RailroadRentStrategy(railroads);
            var moveToNearestRailroad = new MoveToNearestRailroad(board, new[] { 5, 15, 25, 35 }, railroadRentStrategy);
            board.MoveTo(player1, 22);

            moveToNearestRailroad.Play(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(25));
        }

        [Test]
        public void MoveToTheNextRailroadWrapsAroundTheBoard()
        {
            banker = new Banker(players, 1500);
            var railroads = new List<Railroad>();
            var railroadRentStrategy = new RailroadRentStrategy(railroads);
            var moveToNearestRailroad = new MoveToNearestRailroad(board, new[] { 5, 15, 25, 35 }, railroadRentStrategy);
            board.MoveTo(player1, 36);

            moveToNearestRailroad.Play(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(5));
        }

        [Test]
        public void MoveToTheNextRailroadDoublesRent()
        {
            banker = new Banker(players, 1500);

            var railroads = new List<Railroad>();
            var railroadRentStrategy = new RailroadRentStrategy(railroads);
            var readingRailroad = new Railroad("Reading Railroad", banker, railroadRentStrategy);
            var pennsylvaniaRailroad = new Railroad("Pennsylvania Railroad", banker, railroadRentStrategy);
            var bORailroad = new Railroad("B & O Railroad", banker, railroadRentStrategy);
            var shortLineRailroad = new Railroad("Short Line", banker, railroadRentStrategy);
            railroads.AddRange(new[] { readingRailroad, pennsylvaniaRailroad, bORailroad, shortLineRailroad });
            
            var spaces = new Dictionary<Int32, IBoardSpace>
            {
                { 5, readingRailroad },
                { 15, pennsylvaniaRailroad },
                { 25, bORailroad },
                { 35, shortLineRailroad }
            };

            var moveToNearestRailroad = new MoveToNearestRailroad(board, new[] { 5, 15, 25, 35 }, railroadRentStrategy);
            board.SetSpaces(spaces);
            board.MoveTo(player2, 15);
            board.MoveTo(player1, 7);
            var previousBalance = banker.GetBalance(player1);
           
            moveToNearestRailroad.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance - 50));
        }

        [Test]
        public void MoveToNearestUtilityForcesPlayerToPay10TimesRollAmount()
        {
            banker = new Banker(players, 1500);
            board = boardFactory.Create(banker, players, dice, guard);

            var utilities = new List<Utility>();
            var utilityRentStrategy = new UtilityRentStrategy(utilities, dice);
            var electric = new Utility("Electric Company", banker, utilityRentStrategy);
            var water = new Utility("Water Works", banker, utilityRentStrategy);
            utilities.AddRange(new[] { electric, water });

            var spaces = new Dictionary<Int32, IBoardSpace>
            {
                { 12, electric },
                { 28, water }
            };

            board.SetSpaces(spaces);
            board.MoveTo(player2, 12);
            board.MoveTo(player1, 7);
            var previousBalance = banker.GetBalance(player1);
            
            dice.SetNumberToRoll(new[] { 4, 1 });
            dice.Roll();
            var moveToNearestUtility = new MoveToNearestUtility(board, new[] { 12, 28 }, utilityRentStrategy);
            
            moveToNearestUtility.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance - 50));
        }

        [Test]
        public void MoveToNearestUtilityGivesTheOwner10TimesDiceAmount()
        {
            banker = new Banker(players, 1500);
            board = boardFactory.Create(banker, players, dice, guard);

            var utilities = new List<Utility>();
            var utilityRentStrategy = new UtilityRentStrategy(utilities, dice);
            var electric = new Utility("Electric Company", banker, utilityRentStrategy);
            var water = new Utility("Water Works", banker, utilityRentStrategy);
            utilities.AddRange(new[] { electric, water });

            var spaces = new Dictionary<Int32, IBoardSpace>
            {
                { 12, electric },
                { 28, water }
            };

            board.SetSpaces(spaces);
            board.MoveTo(player2, 12);
            board.MoveTo(player1, 7);
            var previousBalance = banker.GetBalance(player2);

            dice.SetNumberToRoll(new[] { 4, 1 });
            dice.Roll();
            var moveToNearestUtility = new MoveToNearestUtility(board, new[] { 12, 28 }, utilityRentStrategy);

            moveToNearestUtility.Play(player1);

            Assert.That(banker.GetBalance(player2), Is.EqualTo(previousBalance + 50));
        }

        [Test]
        public void GoToJailSendsThePlayerDirectlyToJail()
        {
            var goToJail = new GoToJailCard(board, 30);

            goToJail.Play(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(10));
        }

        [Test]
        public void GoToJailDoesNotPassGo()
        {
            board.MoveTo(player1, 36);
            var goToJail = new GoToJailCard(board, 30);
            var previousBalance = banker.GetBalance(player1);

            goToJail.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance));
        }

        [Test]
        public void AdvanceToGoGivesThePlayer200Dollars()
        {
            board.MoveTo(player1, 7);
            var advanceToGo = new AdvanceToGo(board, 0);
            var previousBalance = banker.GetBalance(player1);

            advanceToGo.Play(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance + 200));
        }

        [Test]
        public void APlayerThatGetsTheGetOutOfJailFreeCardDoesntStayInJail()
        {
            var getOutOfJailFree = new GetOutOfJailFree(guard);
            var turns = new PlayerTurnCounter(players);
            dice.SetNumberToRoll(new[] { 15, 15, 4, 1 });
            var game = new Game(players, dice, board, banker, turns, guard);

            getOutOfJailFree.Play(player1);
            game.TakeTurn(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(15));
        }
    }
}