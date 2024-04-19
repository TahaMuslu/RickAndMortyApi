namespace Apposite.Domain.Enums
{
    public enum RoleList
    {
        Admin,
        User
    }

    public class RoleListHelper
    {
        public static string AllList()
        {
            var roles = Enum.GetValues(typeof(RoleList)).Cast<RoleList>().Select(v => v.ToString()).ToArray();
            return string.Join(",", roles);
        }
    }

}


