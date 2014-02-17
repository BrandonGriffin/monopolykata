﻿using System.Collections.Generic;
using MonopolyKata.CoreComponents;
using NUnit.Framework;

namespace MonopolyKata.Tests.CoreComponentsTests
{
    [TestFixture]
    public class PrisonGuardTests
    {
        private Player player1;
        private Player player2;
        private List<Player> players;
        private LoadedDice dice;
        private Banker banker;
        private PrisonGuard guard;
        private Board board;
        private PlayerTurnCounter turns;
        private Game game;

        [SetUp]
        public void SetUp()
        {
            dice = new LoadedDice();
            player1 = new Player("Horse");
            player2 = new Player("Car");
            players = new List<Player> { player1, player2 };
            banker = new Banker(players, 1500);
            var boardFactory = new BoardFactory();
            guard = new PrisonGuard(players, banker, dice);
            board = boardFactory.Create(banker, players, dice, guard);
            turns = new PlayerTurnCounter(players);
        }

        [Test]
        public void PlayersInJailDontMoveWhenTheyRoll()
        {
            board.Move(player1, 30);
            var rolls = new[] { 3, 2 };
            dice.SetNumberToRoll(rolls);
            game = new Game(players, dice, board, banker, turns, guard);
            
            game.TakeTurn(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(10));
        }

        [Test]
        public void APlayerCanPay50DollarsAtTheStartOfATurnToGetOutOfJail()
        {
            board.Move(player1, 30);
            guard.Bribe(player1);
            var rolls = new[] { 3, 2 };
            dice.SetNumberToRoll(rolls);
            game = new Game(players, dice, board, banker, turns, guard);

            game.TakeTurn(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(15));
        }

        [Test]
        public void APlayerGetsOutOfJailForRollingDoubles()
        {
            board.Move(player1, 30);
            var rolls = new[] { 3, 3 };
            dice.SetNumberToRoll(rolls);
            game = new Game(players, dice, board, banker, turns, guard);

            game.TakeTurn(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(16));
        }

        [Test]
        public void APlayerDoesNotgetAnExtraTurnForDoublesWhileInJail()
        {
            board.Move(player1, 30);
            var rolls = new[] { 3, 3, 4, 2, 5 };
            dice.SetNumberToRoll(rolls);
            game = new Game(players, dice, board, banker, turns, guard);

            game.TakeTurn(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(16));
        }

        [Test]
        public void APlayerGetsOutOfJailAfter3Turns()
        {
            board.Move(player1, 30);
            var rolls = new[] { 3, 2, 4, 2, 5, 2, 4, 5 };
            dice.SetNumberToRoll(rolls);
            game = new Game(players, dice, board, banker, turns, guard);

            game.TakeTurn(player1);
            game.TakeTurn(player1);
            game.TakeTurn(player1);

            Assert.That(board.GetPosition(player1), Is.EqualTo(17));
        }
    }
}