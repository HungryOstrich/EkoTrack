using Microsoft.AspNetCore.Mvc.Rendering;
using EkoTrack.Models;
using System.Collections.Generic;

namespace EkoTrack.Models
{
    public class OrganizationUser
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }

        // Users currently in the organization
        public List<AppUser> CurrentUsers { get; set; } = new List<AppUser>();

        // ID of the user selected in the dropdown to be added
        public string SelectedUserId { get; set; }

        // List of users available to be added (excluding current members)
        public IEnumerable<SelectListItem> AvailableUsers { get; set; }
    }
}