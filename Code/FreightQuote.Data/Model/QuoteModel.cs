using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FreightQuote.Data
{
    [MetadataType(typeof(QuoteModel))]
    public partial class Quote
    {

    }

    public class QuoteModel
    {
        [Required(ErrorMessage = "Reference No field is required.")]
        [Remote("IsReferenceNoAvailable", "Quote", ErrorMessage = "Quote with same Reference No. already exist.", AdditionalFields = "InitialReferenceNo")]
        public string ReferenceNo { get; set; }

        [Required(ErrorMessage = "Pickup Location field is required.")]
        public string PickupLocation { get; set; }

        [Required(ErrorMessage = "Delivery Location field is required.")]
        public string DeliveryLocation { get; set; }

        [Required(ErrorMessage = "Status field is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Creation Date field is required.")]
        public DateTime CreationDate { get; set; }

        [Required(ErrorMessage = "Ship Date field is required.")]
        public DateTime ShipDate { get; set; }

        [Required(ErrorMessage = "Description field is required.")]
        public string Description { get; set; }
    }
}
