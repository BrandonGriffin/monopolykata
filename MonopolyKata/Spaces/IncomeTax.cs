﻿using System;

namespace MonopolyKata.Spaces
{
    public class IncomeTax : IBoardSpace
    {
        private Banker banker;
        private Int32 maxAmount;
        private Int32 percent;

        public IncomeTax(Banker banker, Int32 maxAmount, Int32 percent)
        {
            this.banker = banker;
            this.maxAmount = maxAmount;
            this.percent = percent;
        }

        public void LandOnSpace(String player)
        {
            banker.Debit(player, maxAmount, percent);
        }
    }
}
