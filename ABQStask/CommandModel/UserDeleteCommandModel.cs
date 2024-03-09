using System.ComponentModel.DataAnnotations;

namespace ABQStask.CommandModel
{
    public class UserDeleteCommandModel
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsSelected { get; set; }
    }
}
