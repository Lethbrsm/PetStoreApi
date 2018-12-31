using LiteDB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PetStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace PetStore.Controllers
{
    public class OrderController : ApiController
    {
        // This API call adds one item to the current order for a specific customer.
        [HttpPost()]
        public IHttpActionResult Post(Item item)
        {
            IHttpActionResult ret = Ok();

            // Begins the outside API call to retrieve item names and prices.
            using (var client = new HttpClient())
            { 
                client.BaseAddress = new Uri("https://vrwiht4anb.execute-api.us-east-1.amazonaws.com/default/product/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync(item.id).Result;

                if (Res.IsSuccessStatusCode)
                {
                    
                    string pricing = Res.Content.ReadAsStringAsync().Result;

                    var jobject = JObject.Parse(pricing);
                    var prices = JsonConvert.DeserializeObject<Item>(jobject["body"].ToString());
                    
                    // If the user has entered an invalid product ID, return a bad request and ask for correct ID.
                    if (prices == null)
                    {
                        return Content(System.Net.HttpStatusCode.BadRequest, "Please enter a valid Product ID");
                    }
                    else
                    {
                        item.name = prices.name;
                        item.price = prices.price;
                    }
                    
                }
                else
                {
                    ret = NotFound();
                }
            }
            
            // Creates the embedded database to be used to store item information.
            using (var db = new LiteDatabase(@"MyDB.db"))
            {
                var items = db.GetCollection<Item>("items");
                var checkDuplicate = items.Find(Query.EQ("name", item.name));
                List<Item> dupe = checkDuplicate.ToList();

                // Checks if the user has previously put in an order for the same item, and updates the quantity.
                if (dupe.Count == 1)
                {
                    int previousQuantity = dupe[0].quantity;
                    items.Delete(item.id);
                    item.quantity = item.quantity + previousQuantity;
                    items.Insert(item);
                }
                else
                {
                    items.Insert(item);
                }      
            }

            return ret;
        }
        // This API call returns the summary of all items that have been ordered so far.
        [HttpGet()]
        public IHttpActionResult Get()
        {
            IHttpActionResult ret = NotFound();

            // Retrieves the current database to get all orders so far.
            using (var db = new LiteDatabase(@"MyDB.db"))
            {
                var items = db.GetCollection<Item>("items");
                var Tester = items.Find(Query.All());
                List<Item> thing = Tester.ToList();

                // Uses the TotalPrice method to sum all of the item prices that have been ordered.
                Item finalPriceItem = new Item();
                double total = PriceInfo.TotalPrice(thing);
                finalPriceItem.price = total;
                thing.Add(finalPriceItem);

                // Returns the final list of items that have been ordered along with the sum price.
                ret = Ok(thing);

            }
            return ret;
        }
        // This API call is used to remove all items in the database when a new user opens the order page.
        [HttpDelete()]
        public IHttpActionResult Delete()
        {
            IHttpActionResult ret = NotFound();
            using (var db = new LiteDatabase(@"MyDB.db"))
            {
                var checkExists = db.GetCollection<Item>("items");
                db.DropCollection("items");
                ret = Ok();
            }
            return ret;
        }



    }
}
