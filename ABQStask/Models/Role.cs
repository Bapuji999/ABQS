using System.ComponentModel.DataAnnotations;

namespace ABQStask.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string RollName { get; set; }
    }
}
