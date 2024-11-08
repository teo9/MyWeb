using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Shared.Entity
{
    public class MyEntity : IMyEntity
    {
        [Required]
        [Key]
        public long Id { get; set; }
    }

    public interface IMyEntity
    {
        public long Id { get; set; }
    }
}
