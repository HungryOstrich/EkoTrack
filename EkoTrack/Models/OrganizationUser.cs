using Microsoft.AspNetCore.Mvc.Rendering;
using EkoTrack.Models;
using System.Collections.Generic;

namespace EkoTrack.Models
{
    public class OrganizationUser
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public List<AppUser> CurrentUsers { get; set; } = new List<AppUser>();
        public string SelectedUserId { get; set; }
        public IEnumerable<SelectListItem> AvailableUsers { get; set; }
    }
}