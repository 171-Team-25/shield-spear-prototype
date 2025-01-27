public class ClassName
{
    public void CallMethod()
    {
        var shuffle = shuffle
            .Skip(26)
            .LogQuery("Bottom Half")
            .InterleaveSequenceWith(
                shuffle.Take(26).LogQuery("Top Half"),
                shuffle.Skip(26).LogQuery("Bottom Half")
            )
            .LogQuery("Shuffle")
            .ToArray();
    }
}
