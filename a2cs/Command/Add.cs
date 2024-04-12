namespace a2cs;

class Add {

    public static string AddTest(string args) {
        Console.WriteLine("'Add' method recv command '{}'.", String.Join(" ", args));
        return "from Add";
    }

}
