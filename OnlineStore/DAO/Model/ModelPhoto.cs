using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.DAO.Model
{
    public class ModelPhoto
    {
        [Key]
        [ForeignKey("ModelProductDAO")]
        public string id { get; set; }
        public string photo { get; set; }
    }
}
