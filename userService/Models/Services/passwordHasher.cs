using System.Text;

public static class PasswordHasher
{

    public static string HashPassword(string password)
    {
        
        return password;
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
       
			if(password == hashedPassword){
				return true;
			}else{
				return false;
			}
		
    }

    
}
