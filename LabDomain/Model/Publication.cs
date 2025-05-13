using System;
using System.Collections.Generic;

namespace LabDomain.Model;

public partial class Publication
{
    public int PublicationId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateOnly PublicationDate { get; set; }

    public int Views { get; set; }

    public int Subject { get; set; }

    public int PublicationType { get; set; }

    public int Author { get; set; }
}
