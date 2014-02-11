﻿using System.Collections.Generic;

namespace MonopolyKata
{
    public class CommunityChestFactory
    {
        public CommunityChest Create(Banker banker, Board board)
        {
            var cards = new Queue<ICard>();
            var communityChest = new CommunityChest();

            cards = MakeCards(banker, board);
            communityChest.SetCards(cards);
            return communityChest;
        }

        private Queue<ICard> MakeCards(Banker banker, Board board)
        {
            var christmasFund = new PayableCard("Xmas Fund", banker, 100);
            var inheritance = new PayableCard("Inheritance", banker, 100);
            var soldStock = new PayableCard("Sale of Stock", banker, 45);
            var bankError = new PayableCard("Bank Error", banker, 200);
            var receiveForServices = new PayableCard("Receive for Services", banker, 25);
            var beautyContestWinnings = new PayableCard("Second Place in a Beauty Contest", banker, 10);
            var taxRefund = new PayableCard("Tax Refund", banker, 20);
            var lifeInsurance = new PayableCard("Life Insurance", banker, 100);

            var hospitalBill = new ChargableCard("Pay Hospital", banker, 100);
            var doctorsFee = new ChargableCard("Doctor's Fee", banker, 50);
            var schoolTax = new ChargableCard("School Tax", banker, 150);

            var grandOpera = new CollectFromEachPlayer(banker);

            var goToJail = new GoToJailCard(board);
            
            //go to jail
            //get out of jail free
            //advance to go

            var cards = new Queue<ICard>();
            cards.Enqueue(christmasFund);
            cards.Enqueue(inheritance);
            cards.Enqueue(soldStock);
            cards.Enqueue(bankError);
            cards.Enqueue(receiveForServices);
            cards.Enqueue(beautyContestWinnings);
            cards.Enqueue(taxRefund);
            cards.Enqueue(lifeInsurance);
            cards.Enqueue(hospitalBill);
            cards.Enqueue(doctorsFee);
            cards.Enqueue(schoolTax);
            cards.Enqueue(grandOpera);
            cards.Enqueue(goToJail);

            return cards;
        }
    }
}
