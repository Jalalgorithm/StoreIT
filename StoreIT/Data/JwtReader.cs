using System.Security.Claims;

namespace StoreIT.Data
{
    public class JwtReader
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return 0;
            }

            var claim = identity.Claims.FirstOrDefault(c => c.Type.ToLower() == "id");

            if (claim == null)
            {
                return 0;
            }

            int id;

            try
            {
                id = int.Parse(claim.Value);
            }
            catch (Exception)
            {
                return 0;
            }

            return id;
        }

        public static string GetUserRole(ClaimsPrincipal user)
        {
            var identity = user.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return "";
            }

            var claim = identity.Claims.FirstOrDefault(c => c.Type.ToLower() == "role");

            if(claim == null)
            {
                return "";
            }

            string role;

            try
            {
                role = Convert.ToString(claim.Value);   
            }
            catch (Exception)
            {
                return "";
            }

            return role;
        }
    }
}
