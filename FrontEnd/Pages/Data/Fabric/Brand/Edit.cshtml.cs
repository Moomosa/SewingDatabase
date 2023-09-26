using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontEnd.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SewingModels.Models;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
    [Authorize(Roles = "User,Admin")]
    public class EditModel : BaseEditModel<FabricBrand>
    {
        public EditModel(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
    }
}
