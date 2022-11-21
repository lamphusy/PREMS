using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class ReceiptViewModel
    {
        
        public string IDReceipt { get; set; }

        public string IDAccount { get; set; }
        [DataType(DataType.DateTime)]

        public DateTime PaymentDate { get; set; }

        [DataType(DataType.Currency)]
        public double Price { get; set; }

        [DefaultValue("")]
        public string BankCode { get; set; }

        public string IDOrganization { get; set; }
        
        public string OrganizationName { get; set; }

        public string AccountName { get; set; }

        public string AccountUsername { get; set; }

    }
}
