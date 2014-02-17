﻿using System;
using System.Collections.Generic;
using System.Linq;
using MonopolyKata.CoreComponents;
using MonopolyKata.Spaces;

namespace MonopolyKata.RentStrategies
{
    public class UtilityRentStrategy
    {
        private IEnumerable<Utility> utilities;
        private IDice dice;
        private Boolean oneTimeRentBonus;

        public UtilityRentStrategy(IEnumerable<Utility> utilities, IDice dice)
        {
            this.utilities = utilities;
            this.dice = dice;
        }

        public Int32 CalculateRent(BuyableSpace space, Player player)
        {
            var rent = dice.Value * 4;

            if (AllUtilitiesAreOwned() || oneTimeRentBonus)
                rent = dice.Value * 10;

            oneTimeRentBonus = false;

            return rent;
        }
        
        public void SetOneTimeRentBonus()
        {
            oneTimeRentBonus = true;
        }

        private Boolean AllUtilitiesAreOwned()
        {
            return utilities.All(x => x.Owner != null);
        }
    }
}
