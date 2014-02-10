﻿using System;
using System.Collections.Generic;

namespace MonopolyKata
{
    public class Banker
    {
        public Dictionary<Player, Int32> accounts;

        public Banker(List<Player> players)
        {
            accounts = new Dictionary<Player, Int32>();

            foreach (var player in players)
                accounts.Add(player, 1500);
        }
        
        public void Credit(Player player, Int32 amount)
        {
            accounts[player] += amount;
        }

        public void Debit(Player player, Int32 amount)
        {
            accounts[player] -= amount;
        }

        public Int32 GetBalance(Player player)
        {
            return accounts[player];
        }
    }
}