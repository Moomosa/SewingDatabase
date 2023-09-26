using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Fabric.Brand
{
    [Authorize(Roles = "User,Admin")]
    public class IndexModel : BaseIndexModel<FabricBrand>
    {

        public IndexModel(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            PagePath = "/Data/Fabric/Brand";
        }
    }
}
