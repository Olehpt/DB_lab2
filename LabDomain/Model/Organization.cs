using System;
using System.Collections.Generic;

namespace LabDomain.Model;

public partial class Organization
{
    public int OrganizationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public string? Info { get; set; }

    public string Email { get; set; } = null!;

    public DateOnly FoundedDate { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Author> Authors { get; set; } = new List<Author>();
}
