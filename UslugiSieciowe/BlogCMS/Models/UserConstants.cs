namespace BlogCMS.Models
{
    public class UserConstants
    {
        public static List<LoginModel> Users = new()
        {
                new LoginModel(){ Username="Grzegorz",Password="TajneHaslo_1234",Role="Admin"}
        };
    }
}
