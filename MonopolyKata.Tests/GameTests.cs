﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MonopolyKata.Tests
{
    [TestFixture]
    public class GameTests
    {
        private Player player1;
        private Player player2;
        private Random random;
        private Dice dice;
        private List<Player> players;

        [SetUp]
        public void SetUp()
        {
            random = new Random();
            dice = new Dice(random);
            player1 = new Player(dice, "Horse");
            player2 = new Player(dice, "Car");
            players = new List<Player>();

            players.Add(player1);
            players.Add(player2);
        }

        [Test]
        public void GameShouldNotAllowLessThan2Players()
        {
            var players = new List<Player>() { player1 };

            Assert.That(() => new Game(players, random), Throws.Exception.TypeOf<NotEnoughPlayersException>());
        }

        [Test]
        public void GameShouldNotAllowMoreThan8Players()
        {
            var player3 = new Player(dice, "Dog");
            var player4 = new Player(dice, "Thimble");
            var player5 = new Player(dice, "Top Hat");
            var player6 = new Player(dice, "Cat");
            var player7 = new Player(dice, "Shoe");
            var player8 = new Player(dice, "Ship");
            var player9 = new Player(dice, "Wheelbarrow");
            var players = new List<Player>() { player1, player2, player3, player4, player5, player6, player7, player8, player9 };
            
            Assert.That(() => new Game(players, random), Throws.Exception.TypeOf<TooManyPlayersException>());
        }

        [Test]
        public void PlayerOrderShouldBeRandomlyGeneratedAtTheStartOfGame()
        {
            var carCount = 0;
            var horseCount = 0;

            for (var i = 0; i < 100; i++)
            {
                var game = new Game(players, random);

                if (game.Players.First().Name == "Car")
                    carCount++;
                else
                    horseCount++;
            }

            Assert.That(carCount, Is.GreaterThan(0));
            Assert.That(horseCount, Is.GreaterThan(0));
        }

        [Test]
        public void GameShouldLetPlayersTake20TurnsEach()
        {
            var game = new Game(players, random);

            for (var i = 0; i < 20; i++)
                game.Play();

            Assert.That(player1.Position < 40 && player1.Position > -1);
            Assert.That(player2.Position < 40 && player2.Position > -1);
        }
    }
}
