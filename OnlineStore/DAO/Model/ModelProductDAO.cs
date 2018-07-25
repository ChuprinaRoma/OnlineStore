using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.DAO.Model
{
    public class ModelProductDAO
    {

        [Key]
        [ForeignKey("ModelAllShoppe")]
        public string id { get; set; }
        public string nameProduct { get; set; }
        public string description { get; set; }
        public List<ModelDatePrice> ModelDatePrice { get; set; }
        public List<ModelPhoto> listPhoto { get; set; }
    }
}

