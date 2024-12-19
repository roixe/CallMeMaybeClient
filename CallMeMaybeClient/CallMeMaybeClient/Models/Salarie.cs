using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMeMaybeClient.Models
{
    public class Salarie
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string TelFixe { get; set; }
        public string TelMobile { get; set; }
        public string Email { get; set; }
        public int IdService { get; set; }
        public int IdSite { get; set; }

        // Navigation properties 
        public string ServiceNom { get; set; }
        public string SiteNom { get; set; }
    }
}
