using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallMeMaybeClient.Services
{

        public static class RoleManager
        {


            public enum Role
            {
                User,
                Admin
            }

            public static Role CurrentRole { get; private set; } = Role.User; // Par défaut : utilisateur standard

            public static void SetRole(Role role)
            {
                CurrentRole = role;
            }

        public static bool IsAdmin()
        {
            return CurrentRole == Role.Admin;
        }
    }

    
}
