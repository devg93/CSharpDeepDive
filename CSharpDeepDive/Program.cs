using CSharpDeepDive;

public class Program
{

    public static async Task Main(string[] args)
    {

        var asyncTask = new Asynchronous();


        //    foreach (var res in await asyncTask.GetDataAsync3("aaaabc"))
        //    {
        //        Console.WriteLine(res);
        //    }


        // Console.WriteLine(string.Join("", await asyncTask.GetDataAsync3("aaaabc")));

        // Console.WriteLine(string.Join("",await asyncTask.GetDataAsyncWhenAll()));

           Console.WriteLine(string.Join("",await asyncTask.GetDataAsyncWhenAny()));

    

    }
}