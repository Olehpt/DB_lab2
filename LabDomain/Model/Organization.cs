using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabDomain.Model;

public partial class Organization
{
    public int OrganizationId { get; set; }
    [MinLength(4, ErrorMessage = "Name must be at least 4 characters long")]
    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public string? Info { get; set; }
    [EmailAddress]
    [RegularExpression(@"^[^@\s]+@gmail\.com$", ErrorMessage = "Email must be valid")]
    public string Email { get; set; } = null!;

    public DateOnly FoundedDate { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
