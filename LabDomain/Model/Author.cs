using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LabDomain.Model;

public partial class Author
{
    public int AuthorId { get; set; }
    [MinLength(4, ErrorMessage = "Name must be at least 4 characters long")]
    public string Name { get; set; } = null!;
    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@gmail\.com$", ErrorMessage = "Email must be valid")]
    public string Email { get; set; } = null!;
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; } = null!;

    public string? Info { get; set; }

    public DateOnly SignUpDate { get; set; }

    public int? Organization { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Organization? OrganizationNavigation { get; set; }

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
