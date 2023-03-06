using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Classes.V1
{
	public class V1MarketplacePost
	{
        private Guid _id;
        private string _sellersName;
        private V1Book _book;
        private string _condition;
        private decimal _price;
        private string _currency;
        private string _description;
        private DateTime _dateCreated;
        private DateTime _dateModified;
        private string _status;
        private string _buyersName;


		public V1MarketplacePost(string sellerName, V1Book book, string cond, decimal price, string currency,
                                 string description, DateTime created, DateTime modified, string status, string buyerName)
		{
            _id = new Guid();
            _sellersName = sellerName;
            _book = book;
            _condition = cond;
            _price = price;
            _currency = currency;
            _description = description;
            _dateCreated = created;
            _dateModified = modified;
            _status = status;
            _buyersName = buyerName;
		}

        [JsonProperty("id")]
        public Guid Id { get; }

        [JsonProperty("sellersName")]
        public string? SellersName { get; set; }

        [JsonProperty("book")]
        public V1Book? Book { get; set; }

        [JsonProperty("condition")]
        public string? Condition { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("dateModified")]
        public DateTime DateModified { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("buyersName")]
        public string? BuyersName { get; set; }
    }
}

