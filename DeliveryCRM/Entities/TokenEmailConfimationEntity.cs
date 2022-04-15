using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCRM.Entities
{
    public class TokenEmailConfirmationEntity
    {
        [Key]
        public string Token { get; set; }
        public bool? Activated { get; set; }
    }
}
