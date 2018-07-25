using OnlineStore.DAO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.model
{
    public class Product
    {
        public string price                  { get; set; }
        public string nameProduct            { get; set; }
        public string description            { get; set; }
        public List<string> listPhoto        { get; set; }
        public string id                     { get; set; }
        public List<ModelDatePrice> dataTime { get; set; }

        public Product(string price, string description, List<string> listPhoto, string nameProduct, string id)
        {
            this.price       = price;
            this.description = description;
            this.listPhoto   = listPhoto;
            this.nameProduct = nameProduct;
            this.id          = id;
        }
    }
}
