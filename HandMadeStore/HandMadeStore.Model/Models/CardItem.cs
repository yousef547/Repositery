using HandMadeStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandMadeStore.Model.Models
{
    public class CardItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [Range(1, int.MaxValue)]
        public int Count { get; set; }
    }
}
