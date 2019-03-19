﻿using coliks.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coliks.Controllers;

namespace coliks.shared
{
    public static class customersFunctions
    {
        /// <summary>
        /// Get last customer's purchases
        /// There are two way:
        ///  - If exists a vaucher -> then get last customer's purchases from the last discount ("bon d'achat")
        ///  - If a vaucher don't exists -> then get all customer's purchases
        /// </summary>
        /// <param name="purchases"></param>
        /// <returns></returns>
        public static List<Purchases> getLastPurchases(ICollection<Purchases> purchases)
        {
            List<Purchases> elementsAfterLastDiscount = new List<Purchases>();

            //return an empty list if no purchases exists
            if (purchases == null)
                return new List<Purchases>();

            // order all purchases by data to make sure to have the latest purchases after the vaucher
            List<Purchases> _purchases = purchases.OrderBy(c => c.Date).ToList();

            //get the index for the last vaucher
            int index = _purchases.FindIndex(a => a.Description == "*** Bon d-achat de 50.- ***");

            if (index != -1) // a vaucher exists
            {
                // get the last vaucher
                int lastIndexDiscount = _purchases.IndexOf(purchases.Where(c => c.Description == "*** Bon d-achat de 50.- ***").Last());

                // get last purchases from the last vaucher
                for (int i = lastIndexDiscount + 1; i < purchases.Count(); i++)
                {
                    elementsAfterLastDiscount.Add(_purchases[i]);
                }
            }
            else // no vaucher
            {
                //get all purchases from list
                elementsAfterLastDiscount = _purchases;
            }

            return elementsAfterLastDiscount;
        }

        /// <summary>
        /// Calculate the total of all purchases
        /// </summary>
        /// <param name="purchases"></param>
        /// <returns></returns>
        public static double? totalPurchases(List<Purchases> purchases)
        {
            return purchases.Sum(item => item.Amount);
        }
    }
}
