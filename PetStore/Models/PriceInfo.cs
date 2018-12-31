
using System.Collections.Generic;


namespace PetStore.Models
{
    // Used to sum all of the prices of the items ordered by a customer.
    public class PriceInfo
    {
        public static double TotalPrice(List<Item> items)
        {
            double total = 0;
            // Loop through each item in the list provided from the database and add to the total sum. 
            foreach (Item item in items)
            {
                if((item.price < 0) || (item.quantity < 1))
                {
                    throw new System.ArgumentOutOfRangeException();
                }
                total = total + (item.price * item.quantity);
            }
            return total;
        }
    }
}