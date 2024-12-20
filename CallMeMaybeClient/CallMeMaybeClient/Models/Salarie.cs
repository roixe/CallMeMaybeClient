using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMeMaybeClient.Models
{
    public class Salarie
    {
        public int id { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string telFixe { get; set; }
        public string telMobile { get; set; }
        public string email { get; set; }
        public int idService { get; set; }
        public int idSite { get; set; }

        // Navigation properties 
        public string serviceNom { get; set; }
        public string siteNom { get; set; }
    }
}
