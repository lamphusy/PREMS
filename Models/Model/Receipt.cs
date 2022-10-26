using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    [Table("Receipt")]
    public class Receipt
    {
        [Key]
        public string IDReceipt { get; set; }
      
        public string IDAccount { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; }
        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [ForeignKey("IDAccount")]
        public virtual ORegister ORegister { get; set; }
    }
}
