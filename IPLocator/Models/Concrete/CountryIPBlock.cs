using IPLocator.Models.Concrete;
using System;
using System.Collections.Generic;

namespace IPLocator.Models;

public partial class CountryIPBlock : BaseEntity
{

    public string CountryCode { get; set; } = null!;

    public string Cidrmask { get; set; } = null!;

    public string Aclass { get; set; } = null!;

    public string Bclass { get; set; } = null!;

    public string Cclass { get; set; } = null!;

    public string Dclass { get; set; } = null!;

    public string Subnet { get; set; } = null!;
}
