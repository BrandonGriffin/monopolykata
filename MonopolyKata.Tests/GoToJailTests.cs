﻿using System.Collections.Generic;
using NUnit.Framework;

namespace MonopolyKata.Tests
{
    [TestFixture]
    public class GoToJailTests
    {
        [Test]
        public void IfAPlayerLandsOnGoToJailTheyGoToJail()
        {
            var jail = 10;
            var player = new Player("Horse");
            var players = new List<Player> { player };
            var teller = new Banker(players);
            var positionKeeperFactory = new BoardFactory();
            var dice = new LoadedDice();
            var guard = new PrisonGuard(players, teller, dice);
            var positionKeeper = positionKeeperFactory.Create(teller, players, dice, guard);
            var goToJail = new GoToJail(positionKeeper, jail, guard);

            goToJail.SpaceAction(player);

            Assert.That(positionKeeper.GetPosition(player), Is.EqualTo(10));
        }
    }
}
