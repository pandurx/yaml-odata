using backstage_reader;

namespace backstage_hr;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var bsReadWrite = new BackstageReader();
        var userList = bsReadWrite.GetListOfBackstageUserEntities();
        bsReadWrite.WriteUserEntitiesToYaml(userList); // write to yaml
    }
}
