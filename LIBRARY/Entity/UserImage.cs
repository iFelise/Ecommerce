using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIBRARY.Shared.Entity
{
    public class UserImage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string ImageId { get; set; }

        // Relación 1:1 con User
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User user { get; set; }
    }
}