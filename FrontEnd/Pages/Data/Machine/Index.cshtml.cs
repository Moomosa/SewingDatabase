using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SewingModels.Models;
using Microsoft.AspNetCore.Authorization;
using FrontEnd.Common;

namespace FrontEnd.Pages.Data.Machine
{
    [Authorize(Roles = "User,Admin")]
    public class IndexModel : BaseIndexModel<SewingModels.Models.Machine>
    {
        public IndexModel(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            PagePath = "/Data/Machine";
        }
    }
}
