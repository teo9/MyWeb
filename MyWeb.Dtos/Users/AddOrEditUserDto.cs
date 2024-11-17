using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Dtos.Users
{
    public class AddOrEditUserDto
    {

        public long? Id { get; set; }

        [Required]
        public UserDto Item { get; set; }
         
        public bool ChangePassword { get; set; }
        public string PlainTextPassword { get; set; }

        [Required]
        public string[] Permissions { get; set; }
    }
}
