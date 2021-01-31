using System;
using System.ComponentModel.DataAnnotations;

namespace Storez.Api.Models
{
    public class OrderViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string CustomerNumber { get; set; }

        public override string ToString()
        {
            return $"Order:{Id},Customer: {CustomerNumber}";
        }
    }
}
