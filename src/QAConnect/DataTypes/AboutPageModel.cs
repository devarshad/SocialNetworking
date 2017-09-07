using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public class AboutPageModel
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Entered phone format is not valid.")]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Skype { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Gender { get; set; }
        public string ProfilePicture { get; set; }
        public string CoverPicture { get; set; }
        public Nullable<System.DateTime> LastStatusOn { get; set; }
        public Nullable<byte> StatusID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<short> Privacy { get; set; }
        public Nullable<short> Rating { get; set; }
        public string NickName { get; set; }
        public string Organization { get; set; }
        public string OrganizationLocation { get; set; }
        public string PositionInOrganization { get; set; }
        public Nullable<System.DateTime> OrganizationJoinDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public Nullable<int> PostalCode { get; set; }
        public string AboutMe { get; set; }
        public string Relationship { get; set; }
        public string DesireToMeet { get; set; }
        public string TimeZone { get; set; }
        public Nullable<short> Language { get; set; }
        public string Interest { get; set; }
        public string Hobbies { get; set; }
        public string Education { get; set; }
        public Nullable<short> Religion { get; set; }
        public string FavAnimals { get; set; }
        public string FavBooks { get; set; }
        public string FavMusics { get; set; }
        public string FavMovie { get; set; }
        public string favArtist { get; set; }
        public Nullable<short> Smoker { get; set; }
        public Nullable<short> Drinker { get; set; }
        public Nullable<System.DateTime> LastUpdatedOn { get; set; }
        public Enums.PageType PageType { get; set; }
    }
}
