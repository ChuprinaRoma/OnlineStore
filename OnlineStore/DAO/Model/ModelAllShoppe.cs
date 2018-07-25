using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.DAO.Model
{
    public class ModelAllShoppe
    {
        public string id                   { get; set; }
        public string nameShope            { get; set; }
        public List<ModelProductDAO> shope { get; set; }
    }
}
