﻿using System.Collections.Generic;
using NUnit.Framework;

namespace MonopolyKata.Tests
{
    [TestFixture]
    public class PropertyTests
    {
        private Player player1;
        private Player player2;
        private List<Player> players;
        private Banker banker;
        private Property mediterranean;
        private Property baltic;

        [SetUp]
        public void SetUp()
        {
            player1 = new Player("Horse");
            player2 = new Player("Car");
            players = new List<Player> { player1, player2 };
            banker = new Banker(players);
            var purples = new List<Property>();
            mediterranean = new Property("Mediterranean Avenue", 60, 2, banker, purples);
            baltic = new Property("Baltic Avenue", 60, 4, banker, purples);

            purples.AddRange(new[] { mediterranean, baltic });
        }

        [Test]
        public void LandingOnAnUnownedPropertyWillDeductThePurchaseAmountFromThePlayer()
        {
            var previousBalance = banker.GetBalance(player1);
            baltic.SpaceAction(player1);

            Assert.That(banker.GetBalance(player1), Is.EqualTo(previousBalance - 60));
        }

        [Test]
        public void LandingOnAnUnownedPropertyWillMakeThatPlayerTheOwner()
        {
            var previousBalance = banker.GetBalance(player1);

            baltic.SpaceAction(player1);
            var positionOwner = baltic.Owner;

            Assert.That(positionOwner, Is.EqualTo(player1));
        }

        [Test]
        public void LandingOnAPropertyIOwnDoesNothing()
        {
            baltic.SpaceAction(player1);
            baltic.SpaceAction(player1);

            var afterLandingOnMySpace = banker.GetBalance(player1);

            Assert.That(afterLandingOnMySpace, Is.EqualTo(1440));
        }

        [Test]
        public void LandingOnAPropertyOwnedByAnotherPlayerDeductsRentFromMyAccount()
        {
            baltic.SpaceAction(player2);
            var beforeLandingOnSpace = banker.GetBalance(player1);

            baltic.SpaceAction(player1);
            var afterLandingOnMySpace = banker.GetBalance(player1);

            Assert.That(afterLandingOnMySpace, Is.EqualTo(beforeLandingOnSpace - baltic.BaseRent));
        }

        [Test]
        public void IfAnotherPlayerLandsOnMyPropertyMyAccountIsCreditedWithRent()
        {
            baltic.SpaceAction(player2);
            var beforePropertyIsLandedOn = banker.GetBalance(player2);
            
            baltic.SpaceAction(player1);
            var afterPropertyIsLandedOn = banker.GetBalance(player2);

            Assert.That(afterPropertyIsLandedOn, Is.EqualTo(beforePropertyIsLandedOn + baltic.BaseRent));
        }

        [Test]
        public void IfAPlayerHasAMonopolyOfAColorRentDoubles()
        {
            mediterranean.SpaceAction(player2);
            baltic.SpaceAction(player2);
            var beforePropertyIsLandedOn = banker.GetBalance(player2);
          
            baltic.SpaceAction(player1);
            var afterPropertyIsLandedOn = banker.GetBalance(player2);

            Assert.That(afterPropertyIsLandedOn, Is.EqualTo(beforePropertyIsLandedOn + 8));
        }
    }
}
