using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KytaryEshop.Areas.Identity.Data;

// Add profile data for application users by adding properties to the UzivatelIdentita class
public class UzivatelIdentita : IdentityUser
{
    public string KrestniJmeno { get; set; }
    public string Prijmeni { get; set; }
    public string Adresa { get; set; }
}

