using IPLocator.Models.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IPLocator.Models;

public partial class UserResponseCount : BaseEntity
{

    public long ResponseCount { get; set; }

    public Guid IdentityUserId { get; set; }
    
    public DateTime lastClearTime { get; set; }

}
