namespace UserService.Models
{
  internal static class MyResults{
      public static IResult dbIsNotAvaiable(string exeptionMessage){
          Console.WriteLine($"Fatal error during proccess of connecting to dataBase:{exeptionMessage}");
          return Results.Problem(
                  detail: "Can't connect to userService database.",
                  statusCode: StatusCodes.Status503ServiceUnavailable
                  );
      }   

  } 


}
